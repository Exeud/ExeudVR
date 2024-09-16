/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using System;
using ExeudVR.SharedAssets;
using System.Collections;

namespace ExeudVR
{
    /// <summary>
    /// Handles all mouse and keyboard inputs, orients and propels the user in the space.
    /// Connects the user to the objects and tools around them and connects to other components.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Controllers/DesktopController.md"/>

    public class DesktopController : MonoBehaviour
    {
        // Singleton pattern
        private static DesktopController _instance;
        public static DesktopController Instance { get { return _instance; } }


        // Inspector Variables
        [Tooltip("Character physics container")]
        [SerializeField] private GameObject CharacterVehicle;

        [Tooltip("Mouse sensitivity"), Range(0.5f, 5.0f)]
        [SerializeField] private float mouseSensitivity = 2.0f;

        [Tooltip("(Optional) Joysick for character movement on mobile devices")]
        [SerializeField] private Canvas MobileJoystick;

        [Tooltip("Joystick sensitivity, when present")]
        [SerializeField] private float joystickMultiplier = 3f;

        [Tooltip("Which layers should the cursor interact with")]
        [SerializeField] private LayerMask pointerLayerMask;


        // Public Attributes
        public GameObject CurrentObject { get; private set; }

        public float CurrentDistance { get; private set; }

        public Vector3 CurrentHitPoint { get; private set; }

        public bool IsGameMode { get; set; }

        public delegate void CursorInteraction(AvatarHandlingData interactionData);
        public event CursorInteraction OnNetworkInteraction;


        #region ----- Private Variables ------

        private Camera _camera;

        private XRState xrState = XRState.NORMAL;
        private VariableJoystick variableJoystick;

        private bool wasKinematic = false;
        private FixedJoint attachJoint;

        private float runFactor = 1.0f;
        private float jumpCool = 1.0f;

        private readonly float minimumX = -360f;
        private readonly float maximumX = 360f;

        private readonly float minimumY = -90f;
        private readonly float maximumY = 90f;

        private float rotationX = 0f;
        private float rotationY = 0f;

        private Quaternion startRotation;
        private Quaternion currentHeading;
    
        private bool isMouseDown = false;
        private bool isDragging = false;

        private float globalInvertMouse = 1.0f;
        private bool runOne;

        private SharedAsset currentSharedAsset;
        private GameObject activeMesh;

        private float jumpTick;
        private float triggerTickStart = 0;
        private float triggerTickEnd = 0;

        private bool isNetworkConnected = false;

        public bool buttonDown { get; set; }
        private PressableButton currentButton;


        #endregion ----- Private Variables ------


        #region ----- Unity Functions ------

        private void Awake()
        {
            _instance = this;
            _camera = GetComponent<Camera>();
        }

        void Start()
        {
            runOne = true;
            jumpTick = Time.time;

            startRotation = transform.rotation;
            currentHeading = startRotation;

            attachJoint = GetComponent<FixedJoint>();
            
            if (MobileJoystick != null)
            {
                variableJoystick = MobileJoystick.GetComponentInChildren<VariableJoystick>();
                variableJoystick.UpdateJoystickVisibility();
            }

            PlatformManager.Instance.OnStateChange += OnXRChange;
            if (NetworkIO.Instance)
            {
                NetworkIO.Instance.OnConnectionChanged += UpdateConnectionStatus;
            }
        }

        private void Update()
        {
            if (xrState != XRState.NORMAL) { return; }
            SetCameraRotation();

            // make observation
            GameObject viewedObject = ScreenRaycast();
            if (viewedObject != null)
            {
                CurrentObject = viewedObject;
            }
            else
            {
                // remember, this is a Unity null, not a complete null
                CurrentObject = null;
            }
        }

        void FixedUpdate()
        {
            if (xrState != XRState.NORMAL) { return; }

            // set character pose
            MoveBodyWithKeyboard();
            MoveBodyWithJoystick();
        }

        private void OnDisable()
        {
            PlatformManager.Instance.OnStateChange -= OnXRChange;
        }

        private void OnXRChange(XRState state)
        {
            xrState = state;
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.SetCursorParameters(xrState);
            }

            if (variableJoystick != null)
            {
                variableJoystick.UpdateJoystickVisibility();
            }
        }

        private void UpdateConnectionStatus(bool state)
        {
            isNetworkConnected = state;
        }

        #endregion ----- Unity Functions ------


        #region ----- Input Handling -----

        void OnGUI()
        {
            if (xrState != XRState.NORMAL) { return; }

            CursorManager.Instance.HandleCursorFocus(CurrentObject);

            Event e = Event.current;

            // mouse events
            if (e.isMouse)
            {
                if (e.button == 0)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        isMouseDown = true;
                    }
                    else if (e.type == EventType.MouseUp)
                    {
                        isMouseDown = false;
                    }
                }

                if (e.clickCount == 2)
                {
                    CursorManager.Instance.DoubleClick();
                    isMouseDown = false;
                }

                if (e.type == EventType.MouseDown && e.button == 0 && CurrentObject != null)
                {
                    if (CurrentObject.TryGetComponent(out SharedAsset sharedAsset))
                    {
                        if (!sharedAsset.IsBeingHandled)
                        {
                            currentSharedAsset = sharedAsset;
                        }
                        else
                        {
                            Debug.Log("This object is in use.");
                        }
                    }

                    if (CurrentObject.TryGetComponent(out PressableButton button))
                    {
                        ActivateButton(button);
                    }
                    else if (!CurrentObject.GetComponent<XRController>() && CurrentObject.TryGetComponent(out RigidDynamics rd))
                    {
                        PickUpObject(rd);
                    }
                    
                }
                else if (e.type == EventType.MouseUp && e.button == 0)
                {
                    if (isDragging)
                    {
                        ReleaseObject();
                    }
                    else if (buttonDown && currentButton != null)
                    {
                        ReleaseButton(currentButton);
                    }
                }
            }

            // keyboard events
            else if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.I)
                {
                    globalInvertMouse *= -1.0f;
                }

                if (e.keyCode == KeyCode.Space)
                {
                    JumpSwim();
                }

                if (e.keyCode == KeyCode.LeftShift)
                {
                    runFactor = 2.0f;
                }

                if (e.keyCode == KeyCode.CapsLock)
                {
                    if (runFactor == 1.0f)
                        runFactor = 2.0f;
                    else
                        runFactor = 1.0f;
                }

            }
            else if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.LeftShift)
                {
                    runFactor = 1.0f;
                }
            }
        }

        #endregion ----- Input Handling -----


        #region ----- Character Movement ------    

        private Quaternion GetCameraRotationFromMouse(float sensitivity, float invertMouse)
        {
#if UNITY_EDITOR
            // BUG: the sensitivity is much lower in the editor. this is a hack
            //sensitivity *= 3.0f;
#endif
            if (runOne)
            {
                rotationX = Input.GetAxis("Mouse X");
                rotationY = Input.GetAxis("Mouse Y");
                runOne = false;
            }
            else
            {
                rotationX += (Input.GetAxis("Mouse X") * invertMouse) * sensitivity * 1.0f;
                rotationY += (Input.GetAxis("Mouse Y") * invertMouse) * sensitivity * 1.0f;
            }
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            return RelativeQuatFromIncrements(rotationX, rotationY);
        }

        private Quaternion RelativeQuatFromIncrements(float rotX, float rotY)
        {
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
            currentHeading = startRotation * xQuaternion * yQuaternion;
            return currentHeading;
        }

        private void MoveBodyWithJoystick()
        {
            if (variableJoystick == null) { return; }

            float x = variableJoystick.Horizontal * 0.5f * Time.deltaTime * joystickMultiplier;
            float z = variableJoystick.Vertical * 0.5f * Time.deltaTime * joystickMultiplier;

            // conditions for no action
            if (x == 0 && z == 0) return;

            // camera forward and right vectors
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // project forward and right vectors on the horizontal plane
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // this is the direction in the world space we want to move:
            var desiredMoveDirection = forward * z + right * x;
            CharacterVehicle.transform.Translate(desiredMoveDirection);
            
        }

        private void MoveBodyWithKeyboard()
        {
            float x = Input.GetAxis("Horizontal") * Time.smoothDeltaTime * runFactor;
            float z = Input.GetAxis("Vertical") * Time.smoothDeltaTime * runFactor;

            // conditions for no action
            if (CharacterVehicle == null) return;
            if (x == 0 && z == 0) return;

            //project forward and right vectors on the horizontal plane (y = 0)
            Vector3 personForward = CharacterVehicle.transform.InverseTransformDirection(transform.forward);
            Vector3 personRight = CharacterVehicle.transform.InverseTransformDirection(transform.right);
            personForward.y = 0;
            personRight.y = 0;
            personForward.Normalize();
            personRight.Normalize();

            //this is the direction in the world space we want to move:
            var desiredMoveDirection = personForward * z + personRight * x;
            CharacterVehicle.transform.Translate(desiredMoveDirection);
        }

        private void SetCameraRotation()
        {
            float dragMod = isDragging ? -1.0f * globalInvertMouse: 1.0f;

            if (IsGameMode)
            {
                Quaternion camQuat = GetCameraRotationFromMouse(mouseSensitivity, globalInvertMouse);
                StartCoroutine(RotateCamera(camQuat, 1.0f));
            }
            else
            {
                if (isMouseDown)
                {
                    Quaternion camQuat = GetCameraRotationFromMouse(mouseSensitivity, -1.0f * dragMod * globalInvertMouse);
                    StartCoroutine(RotateCamera(camQuat, 1.0f));
                }
            }
        }

        private void JumpSwim()
        {
            if (CharacterVehicle == null) { return; }

            bool isSwimming = CharacterVehicle.transform.position.y < -10f;

            if (isSwimming && Time.time - jumpTick > (jumpCool / 10.0f))
            {
                jumpTick = Time.time;
                Vector3 swimForce = new Vector3(0f, 150f, 0f) + (transform.forward * 10f);
                CharacterVehicle.GetComponent<Rigidbody>().AddForce(swimForce, ForceMode.Impulse);
            }
            else if (Time.time - jumpTick > jumpCool)
            {
                jumpTick = Time.time;
                Vector3 jumpForce = new Vector3(0f, 350f, 0f) + transform.forward * 50f;
                CharacterVehicle.GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.Impulse);
            }
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        private IEnumerator RotateCamera(Quaternion targetRot, float speed)
        {
            float rotationTimer = 0.0f;

            while (rotationTimer < 1.0f)
            {
                rotationTimer += Time.smoothDeltaTime * speed;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationTimer * speed);
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion ----- Character Movement ------


        #region ----- Object Interaction ------

        private GameObject PickUpObject(RigidDynamics rd)
        {
            if (rd != null)
            {
                activeMesh = rd.gameObject;

                Rigidbody actRB = activeMesh.GetComponent<Rigidbody>();
                actRB.isKinematic = false;
                actRB.MovePosition(transform.position);


                attachJoint.connectedBody = actRB;

                if (currentSharedAsset && isNetworkConnected)
                {
                    currentSharedAsset.IsBeingHandled = true;
                    InvokeAcquisitionEvent(currentSharedAsset.Id, activeMesh.transform);
                }

                // flag caught by fixed update
                isDragging = true;
                return activeMesh;
            }
            else
            {
                activeMesh = null;
                return null;
            }
        }

        private void ReleaseObject()
        {
            isDragging = false;

            if (activeMesh.TryGetComponent(out RigidDynamics dynamics))
            {
                Rigidbody activeRB = activeMesh.GetComponent<Rigidbody>();

                attachJoint.connectedBody = null;
                activeRB.useGravity = dynamics.UsesGravity;

                ThrowData td = dynamics.Throw;

                activeRB.isKinematic = wasKinematic;
                activeRB.AddForce(td.LinearForce, ForceMode.Impulse);
                activeRB.AddTorque(td.AngularForce, ForceMode.Impulse);

                // network release
                if (currentSharedAsset && isNetworkConnected)
                {
                    InvokeReleaseEvent(currentSharedAsset.Id, activeMesh, td);
                    currentSharedAsset.IsBeingHandled = false;
                    currentSharedAsset = null;
                }
            }

            if (activeMesh.TryGetComponent(out ControlDynamics cd))
            {
                cd.FinishInteraction();
            }

            activeMesh = null;
        }

        private void InvokeAcquisitionEvent(string target, Transform interactionTransform)
        {
            AcquireData newAcquisition = new AcquireData
            {
                AcqTime = DateTime.UtcNow.Ticks,
                ObjectPosition = interactionTransform.position,
                ObjectRotation = interactionTransform.rotation
            };

            var interactionEvent = BuildEventFrame(target, AvatarInteractionEventType.AcquireData, newAcquisition, null);
            OnNetworkInteraction?.Invoke(interactionEvent);
        }

        private void InvokeReleaseEvent(string target, GameObject interactionObject, ThrowData throwData)
        {
            ReleaseData newRelease = new ReleaseData
            {
                ReleaseTime = DateTime.UtcNow.Ticks,
                ReleasePosition = interactionObject.transform.position,
                ReleaseRotation = interactionObject.transform.rotation,
                ForceData = throwData
            };

            var interactionEvent = BuildEventFrame(target, AvatarInteractionEventType.ReleaseData, null, newRelease);
            OnNetworkInteraction?.Invoke(interactionEvent);
        }

        private AvatarHandlingData BuildEventFrame(string targetId, AvatarInteractionEventType eventType, AcquireData acqDataFrame = null, ReleaseData relDataFrame = null)
        {
            AvatarHandlingData eventFrame = new AvatarHandlingData
            {
                Hand = ControllerHand.NONE,
                TargetId = targetId,
                Distance = ManipulationDistance.None,
                EventType = eventType,
                AcquisitionEvent = acqDataFrame,
                ReleaseEvent = relDataFrame
            };
            return eventFrame;
        }

        private GameObject GetActiveMesh(GameObject ooi)
        {
            // priority: self, parent, child
            return ooi.TryGetComponent(out RigidDynamics r_1) ? r_1.gameObject :
                ooi.transform.parent.TryGetComponent(out RigidDynamics r_2) ? r_2.gameObject :
                ooi.GetComponentInChildren<RigidDynamics>().gameObject;
        }

        private void ActivateButton(PressableButton pb)
        {
            if ((Time.time - triggerTickStart) > 0.5f)
            {
                triggerTickStart = Time.time;
                buttonDown = true;
                currentButton = pb;
                pb.ButtonPressed.Invoke();
            }
        }

        private void ReleaseButton(PressableButton pb)
        {
            if ((Time.time - triggerTickEnd) > 0.5f)
            {
                triggerTickEnd = Time.time;
                buttonDown = false;
                currentButton = null;
                pb.ButtonReleased.Invoke();
            }
        }

        private GameObject ScreenRaycast(bool fromTouch = false)
        {
            if (!_camera.isActiveAndEnabled) return null;

            if (isDragging) return CurrentObject;

            Ray ray;

            if (IsGameMode)
            {
                ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            }
            else
            {
                ray = _camera.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            }

            if (Physics.Raycast(ray, out RaycastHit pointerHit, 60.0f, pointerLayerMask))
            {
                CurrentHitPoint = pointerHit.point;
                CurrentDistance = pointerHit.distance;
                return pointerHit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }

        #endregion ----- Object Interaction ------

    }
}
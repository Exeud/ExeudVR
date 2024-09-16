/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using WebXR;
using ExeudVR.SharedAssets;

#if UNITY_EDITOR || !UNITY_WEBGL
using UnityEngine.XR;
#endif

namespace ExeudVR
{
    /// <summary>
    /// Interaction handler for XR controllers on the web. Uses the thumbstick, grip and trigger buttons 
    /// to control the character, including jumping and swimming. Connect interaction events to the 
    /// virtual world's collision and data layers. Warning: modifying this without proper knowledge can 
    /// really mess things up.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Controllers/XRController.md"/>
    /// </summary>
    public class XRController : MonoBehaviour
    {
        [SerializeField] private ControllerHand hand;
        [SerializeField] private bool debugHand;
        [SerializeField] private GameObject CharacterRoot;

        [SerializeField] private float MaxInteractionDistance = 15.0f;
        [SerializeField] private GameObject handModel;
        [SerializeField] private XRPointer xrPointer;

        [SerializeField] private Animator anim;
        [SerializeField] private GameObject handIKAnchor;


        #region >   Public Variables, Functions and Attributes

        public enum ButtonTypes
        {
            Trigger = 0,
            Grip = 1,
            Thumbstick = 2,
            Touchpad = 3,
            ButtonA = 4,
            ButtonB = 5
        }
        public enum AxisTypes
        {
            Trigger,
            Grip
        }

        // WebXR Actions
        public Action<bool> OnControllerActive;
        public Action<bool> OnHandActive;
        public Action<WebXRHandData> OnHandUpdate;


        // flag used by ObjectInterface - true when the hand is bound to an object
        public bool IsUsingInterface { get; private set; }

        // flag set by ControlDynamics - true when the controller is articulating equipment
        public bool IsControllingObject { get; private set; }


        // Hand anchor used by rigged body model
        public GameObject HandAnchor
        {
            get { return handIKAnchor; }
            set { handIKAnchor = value; }
        } 

        // Hand model used by ObjectInterface
        public GameObject HandModel
        {
            get { return handModel; }
            set { handModel = value; }
        }

        public ControllerHand SetCurrentInterface(bool isControlDelegated, ObjectInterface objInt)
        {
            IsUsingInterface = isControlDelegated;
            currentInterface = objInt;
            return isControlDelegated ? hand : ControllerHand.NONE;
        }

        public void SetGripPose(string newGripPose)
        {
            gripPose = string.IsNullOrEmpty(newGripPose) ? "holdIt" : newGripPose;
            anim.SetTrigger(gripPose);
        }

        /// <summary>
        /// Function used in two-handed interaction, allowing 
        /// this hand to adopt a secondary, object-locked grip.
        /// </summary>
        /// <param name="jointIndex"></param>
        /// <param name="connectedBody"></param>
        public void ModifyJoint(int jointIndex, Rigidbody connectedBody = null)
        {
            attachJoint[jointIndex].connectedBody = connectedBody;
        }

        // events, use as hooks for controller button functions
        public event BodyController.ObjectTrigger OnObjectTrigger;
        public event BodyController.ObjectGrip OnObjectGrip;

        public delegate void ButtonPressed(float buttonValue);
        public event ButtonPressed AButtonEvent;
        public event ButtonPressed BButtonEvent;

        public delegate void HandInteraction(AvatarHandlingData interactionData);
        public event HandInteraction OnHandInteraction;

        #endregion Public Variables, Functions and Attributes


        #region >   Private Variables

        private WebXRState xrState = WebXRState.NORMAL;

        private float trigger;
        private float squeeze;
        private float thumbstick;
        private float thumbstickX;
        private float thumbstickY;
        private float touchpad;
        private float touchpadX;
        private float touchpadY;
        private float buttonA;
        private float buttonB;

        private Dictionary<ButtonTypes, WebXRControllerButton> buttonStates = new Dictionary<ButtonTypes, WebXRControllerButton>();

        private bool controllerActive = false;
        private bool handActive = false;

        private FixedJoint[] attachJoint;

        private Rigidbody currentNearRigidBody = null;
        private Rigidbody currentFarRigidBody = null;

        private ObjectInterface currentInterface;

        private List<Rigidbody> nearcontactRigidBodies = new List<Rigidbody>();
        private List<Rigidbody> farcontactRigidBodies = new List<Rigidbody>();

        private SharedAsset currentNearSharedAsset;
        private SharedAsset currentFarSharedAsset;

        private string prevMeshName = "";

        // Axis trigger thresholds
        float trigThresUp = 0.90f;
        float trigThresDn = 0.10f;
        float gripThresUp = 0.90f;
        float gripThresDn = 0.10f;
        float thumbThresUp = 0.90f;
        float thumbThresDn = 0.10f;

        private float prevRightThX = 0f;
        private float prevTrig = 0f;
        private float prevGrip = 0f;

        private bool touchingButton;
        private bool pointingAtButton;
        private PressableButton currentButton;


        // Object Handling
        private bool distanceManip = false;
        private string gripPose;

        private float actionTick = 0f;
        private float triggerEnterTick = 0f;
        private float triggerExitTick = 0f;

        private float jumpTick;
        private float seaLevel = -4.5f;

        private Camera _camera;
        private Vector3 defaultPositionOffset;
        private Quaternion defaultRotationOffset;

        #endregion Private Variables


        #region >   Unity Functions


        private void OnDisable()
        {
            WebXRManager.OnXRChange -= OnXRChange;
            WebXRManager.OnControllerUpdate -= OnControllerUpdate;
            WebXRManager.OnHandUpdate -= OnHandUpdateInternal;

            SetControllerActive(false);
            SetHandActive(false);
        }

        private void OnEnable()
        {
            WebXRManager.OnXRChange += OnXRChange;
            WebXRManager.OnControllerUpdate += OnControllerUpdate;
            WebXRManager.OnHandUpdate += OnHandUpdateInternal;

            SetControllerActive(false);
            SetHandActive(false);
        }


        void Start()
        {
            defaultPositionOffset = transform.localPosition;
            defaultRotationOffset = transform.localRotation;

            attachJoint = new FixedJoint[] { GetComponents<FixedJoint>()[0], GetComponents<FixedJoint>()[1] };
            _camera = CharacterRoot.GetComponentInChildren<Camera>();
            

            if (!debugHand)
            {
                SetGripPose("relax");
                ToggleHandObjects(xrState == WebXRState.VR);
            }
            jumpTick = Time.time;
        }


        void FixedUpdate()
        {
            if (xrState == WebXRState.NORMAL && !debugHand)
            {
                transform.localPosition = ((_camera.transform.right * defaultPositionOffset.x) + (CharacterRoot.transform.up * defaultPositionOffset.y));
                transform.rotation = Quaternion.Euler(new Vector3(0f, _camera.transform.transform.eulerAngles.y, 0f)) * defaultRotationOffset;
                return;
            }

            // left stick controls movement
            if (hand == ControllerHand.LEFT)
            {
                if (Math.Abs(thumbstickX) > thumbThresDn || Math.Abs(thumbstickY) > thumbThresDn)
                {
                    MoveVehicleWithJoystick(thumbstickX, thumbstickY, 2.0f);
                }
            }
            // right stick turns in 60 degree steps and forwards-backwards
            else if (hand == ControllerHand.RIGHT)
            {
                if (Mathf.Abs(thumbstickX) > thumbThresUp && prevRightThX <= thumbThresUp)
                {
                    RotateVehicleWithJoystick(thumbstickX);
                }
                prevRightThX = Mathf.Abs(thumbstickX);

                if (Math.Abs(thumbstickY) > thumbThresDn)
                {
                    MoveVehicleWithJoystick(0.0f, thumbstickY, 2.0f);
                }
            }

            // trigger for distance interaction
            float trigVal = GetAxis(AxisTypes.Trigger);
            if (trigVal > trigThresUp && prevTrig <= trigThresUp)
            {
                PickupFar();

                if (IsUsingInterface)
                {
                    UseObjectTrigger(trigVal);
                }
            }
            else if (trigVal < trigThresDn && prevTrig >= trigThresDn)
            {
                if (distanceManip)
                {
                    DropFar();
                }
                else if (currentButton != null)
                {
                    DropFar();
                }
            }

            // grip for near interaction
            float gripVal = GetAxis(AxisTypes.Grip);
            if (gripVal > gripThresUp && prevGrip <= gripThresUp)
            {
                PickupNear();

                if (IsUsingInterface)
                {
                    UseObjectGrip(true);
                }
            }
            else if (gripVal < gripThresDn && prevGrip >= gripThresDn)
            {
                if (IsUsingInterface)
                {
                    UseObjectGrip(false);
                }
                DropNear();
            }

            if (thumbstick == 1)
            {
                JumpSwim();
            }

            if (IsUsingInterface)
            {
                if (GetButtonDown(ButtonTypes.ButtonA))
                {
                    AButtonEvent.Invoke(1.0f);
                }
                else if (GetButtonUp(ButtonTypes.ButtonA))
                {
                    AButtonEvent.Invoke(0.0f);
                }

                if (GetButtonDown(ButtonTypes.ButtonB))
                {
                    BButtonEvent.Invoke(1.0f);
                }
                else if (GetButtonUp(ButtonTypes.ButtonB))
                {
                    BButtonEvent.Invoke(0.0f);
                }
            }

            prevTrig = trigVal;
            prevGrip = gripVal;

            // handle far interaction
            SetActiveFarMesh();
        }

#if UNITY_EDITOR || !UNITY_WEBGL
        private InputDevice? inputDevice;
        private int buttonsFrameUpdate = -1;

        private void LateUpdate()
        {
            TryUpdateButtons();
        }
#endif


        #endregion Unity Functions


        #region >   State Functions


        private void OnHandUpdateInternal(WebXRHandData handData)
        {
            if (handData.hand == (int)hand)
            {
                if (!handData.enabled)
                {
                    SetHandActive(false);
                    return;
                }

                SetControllerActive(false);
                SetHandActive(true);

                transform.localPosition = handData.joints[0].position;
                transform.localRotation = handData.joints[0].rotation;

                trigger = handData.trigger;
                squeeze = handData.squeeze;

                OnHandUpdate?.Invoke(handData);
            }
        }

        private void SetControllerActive(bool active)
        {
            if (controllerActive != active)
            {
                controllerActive = active;
                OnControllerActive?.Invoke(controllerActive);
            }
        }

        private void SetHandActive(bool active)
        {
            if (handActive == active)
            {
                return;
            }
            handActive = active;
            OnHandActive?.Invoke(handActive);
        }

        private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            xrState = state;
            ToggleHandObjects(xrState == WebXRState.VR);
        }

        private void ToggleHandObjects(bool isVR)
        {
            // Hide the children
            for (int t = 0; t < transform.childCount; t++)
            {
                transform.GetChild(t).gameObject.SetActive(isVR);
            }

            // Toggle near interaction triggers
            GetComponent<CapsuleCollider>().enabled = isVR;
        }

        #endregion State Functions


        #region >   Button Functionality


        private void TryUpdateButtons()
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            if (buttonsFrameUpdate == Time.frameCount)
            {
                return;
            }
            buttonsFrameUpdate = Time.frameCount;
            if (!WebXRManager.Instance.isSubsystemAvailable && inputDevice != null)
            {
                inputDevice.Value.TryGetFeatureValue(CommonUsages.trigger, out trigger);
                inputDevice.Value.TryGetFeatureValue(CommonUsages.grip, out squeeze);
                if (trigger <= 0.02)
                {
                    trigger = 0;
                }
                else if (trigger >= 0.98)
                {
                    trigger = 1;
                }

                if (squeeze <= 0.02)
                {
                    squeeze = 0;
                }
                else if (squeeze >= 0.98)
                {
                    squeeze = 1;
                }

                Vector2 axis2D;
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis2D))
                {
                    thumbstickX = axis2D.x;
                    thumbstickY = axis2D.y;
                }
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.secondary2DAxis, out axis2D))
                {
                    touchpadX = axis2D.x;
                    touchpadY = axis2D.y;
                }
                bool buttonPressed;
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out buttonPressed))
                {
                    thumbstick = buttonPressed ? 1 : 0;
                }
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.secondary2DAxisClick, out buttonPressed))
                {
                    touchpad = buttonPressed ? 1 : 0;
                }
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed))
                {
                    buttonA = buttonPressed ? 1 : 0;
                }
                if (inputDevice.Value.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressed))
                {
                    buttonB = buttonPressed ? 1 : 0;
                }
            }
#endif
        }

        private void OnControllerUpdate(WebXRControllerData controllerData)
        {
            if (controllerData.hand == (int)hand)
            {
                if (!controllerData.enabled)
                {
                    SetControllerActive(false);
                    return;
                }

                transform.localRotation = controllerData.rotation;
                transform.localPosition = controllerData.position;

                trigger = controllerData.trigger;
                squeeze = controllerData.squeeze;
                thumbstick = controllerData.thumbstick;
                thumbstickX = controllerData.thumbstickX;
                thumbstickY = controllerData.thumbstickY;
                touchpad = controllerData.touchpad;
                touchpadX = controllerData.touchpadX;
                touchpadY = controllerData.touchpadY;
                buttonA = controllerData.buttonA;
                buttonB = controllerData.buttonB;

                SetControllerActive(true);
            }
        }

        private float GetAxis(AxisTypes action)
        {
            TryUpdateButtons();
            switch (action)
            {
                case AxisTypes.Grip:
                    return squeeze;
                case AxisTypes.Trigger:
                    return trigger;
            }
            return 0;
        }

        private bool GetButtonDown(ButtonTypes action)
        {
            TryUpdateButtons();
            if (!buttonStates.ContainsKey(action))
            {
                return false;
            }
            return buttonStates[action].down;
        }

        private bool GetButtonUp(ButtonTypes action)
        {
            TryUpdateButtons();
            if (!buttonStates.ContainsKey(action))
            {
                return false;
            }
            return buttonStates[action].up;
        }

        public float GetButtonIndexValue(int index)
        {
            TryUpdateButtons();
            switch (index)
            {
                case 0:
                    return trigger;
                case 1:
                    return squeeze;
                case 2:
                    return touchpad;
                case 3:
                    return thumbstick;
                case 4:
                    return buttonA;
                case 5:
                    return buttonB;
            }
            return 0;
        }

        public float GetAxisIndexValue(int index)
        {
            TryUpdateButtons();
            switch (index)
            {
                case 0:
                    return touchpadX;
                case 1:
                    return touchpadY;
                case 2:
                    return thumbstickX;
                case 3:
                    return thumbstickY;
            }
            return 0;
        }


        #endregion Button Functionality


        #region >   Character Movement

        private void MoveVehicleWithJoystick(float xax, float yax, float multiplier = 1.0f)
        {
            float x = xax * Time.deltaTime;
            float z = yax * Time.deltaTime;
            Camera referenceCam = _camera;

            // get camera-aligned directions
            Vector3 worldForward = CharacterRoot.transform.InverseTransformDirection(referenceCam.transform.forward);
            Vector3 worldRight = CharacterRoot.transform.InverseTransformDirection(referenceCam.transform.right);

            // flatten and normalise
            worldForward.y = 0;
            worldRight.y = 0;
            worldForward.Normalize();
            worldRight.Normalize();

            //direction in world space we want to move
            Vector3 desiredMoveDirection = worldForward * z + worldRight * x;
            CharacterRoot.transform.Translate(desiredMoveDirection * multiplier);
        }

        private void RotateVehicleWithJoystick(float value)
        {
            if (value == 0) { return; }

            if (value > 0)
            {
                CharacterRoot.transform.RotateAround(_camera.transform.position, Vector3.up, 60);
            }
            else
            {
                CharacterRoot.transform.RotateAround(_camera.transform.position, Vector3.up, -60);
            }
        }

        private void JumpSwim()
        {
            bool isSwimming = CharacterRoot.transform.position.y < seaLevel;

            if (isSwimming && Time.time - jumpTick > 0.2f)
            {
                jumpTick = Time.time;
                Vector3 swimForce = new Vector3(0f, 150f, 0f) + (CharacterRoot.transform.forward * 50f);
                CharacterRoot.GetComponent<Rigidbody>().AddForce(swimForce, ForceMode.Acceleration);
            }
            else if (Time.time - jumpTick > 2.0f)
            {
                jumpTick = Time.time;
                Vector3 jumpForce = new Vector3(0f, 350f, 0f) + (CharacterRoot.transform.forward * 50f);
                CharacterRoot.GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.Impulse);
            }
        }


        #endregion Character Movement


        #region >   Interaction Functions

        public void PickupFar()
        {
            // button actions take priority
            if (touchingButton || pointingAtButton)
            {
                if (currentButton && (Time.time - actionTick) > 0.5f)
                {
                    actionTick = Time.time;
                    currentButton.ButtonPressed?.Invoke();
                }
                return;
            }

            // stop here if not handling a moveable object
            currentFarRigidBody = GetDistantRigidBody();
            if (!currentFarRigidBody) return;

            GameObject currentFarObject = currentFarRigidBody.gameObject;

            // skip near-interaction-only objects
            if (currentFarObject.GetComponent<ControlDynamics>()) { return; }

            // pick up
            currentFarRigidBody.MovePosition(transform.position);
            attachJoint[1].connectedBody = currentFarRigidBody;
            distanceManip = true;

            // identify shared asset and assign currentTargetId
            SharedAsset sharedAsset = currentFarObject.GetComponent<SharedAsset>();
            if (sharedAsset != null)
            {
                if (!sharedAsset.IsBeingHandled)
                {
                    currentFarSharedAsset = sharedAsset;
                    currentFarSharedAsset.IsBeingHandled = true;

                    InvokeAcquisitionEvent(currentFarSharedAsset.Id, currentFarRigidBody.gameObject.transform, ManipulationDistance.Far);
                }
                else
                {
                    // asset is being used
                    Debug.Log("This object is being used by someone else.");
                }
            }

            SetGripPose("pointAtIt");
        }

        public void DropFar()
        {
            if (touchingButton || pointingAtButton)
            {
                if (currentButton)
                {
                    currentButton.ButtonReleased?.Invoke();
                }
                return;
            }

            // if no object sensed, stop 
            if (!currentFarRigidBody) return;

            ThrowData newThrow;
            attachJoint[1].connectedBody = null;
            farcontactRigidBodies.Clear();
            distanceManip = false;

            if (currentFarRigidBody.gameObject.TryGetComponent(out RigidDynamics rd))
            {
                currentFarRigidBody.useGravity = rd.UsesGravity;
                newThrow = rd.Throw;
            }
            else
            {
                newThrow = new ThrowData()
                {
                    LinearForce = currentFarRigidBody.velocity,
                    AngularForce = currentFarRigidBody.angularVelocity
                };
            }

            currentFarRigidBody.AddForce(newThrow.LinearForce, ForceMode.Impulse);
            currentFarRigidBody.AddTorque(newThrow.AngularForce, ForceMode.Impulse);

            SetGripPose("relax");

            // network release
            if (currentFarSharedAsset != null)
            {
                currentFarSharedAsset.IsBeingHandled = false;
                InvokeReleaseEvent(currentFarSharedAsset.Id, currentFarRigidBody.gameObject, ManipulationDistance.Far, newThrow);
                currentFarSharedAsset = null;
            }

            currentFarRigidBody = null;
        }

        private void BeginAttractFar(Rigidbody targetRB)
        {
            if (targetRB.gameObject.TryGetComponent(out Wieldable grabber))
            {
                currentFarRigidBody.useGravity = false;
                attachJoint[1].connectedBody = null;
                grabber.BeginAttraction(hand, transform, AttractFar);
            }
        }

        public void AttractFar(Wieldable sender, string pose)
        {
            if (currentFarRigidBody && prevGrip > 0.8f)
            {
                currentFarRigidBody.isKinematic = true;

                // local release far
                farcontactRigidBodies.Clear();
                currentFarRigidBody = null;
                distanceManip = false;

                // network release far
                if (currentFarSharedAsset)
                {
                    currentFarSharedAsset.IsBeingHandled = false;
                    InvokeReleaseEvent(currentFarSharedAsset.Id, sender.gameObject, ManipulationDistance.Far, new ThrowData());
                }

                // use the force
                gripPose = sender.AttractObject(hand, transform, PickupNear);
                SetGripPose(gripPose);
            }
            else
            {
                DropFar();
            }
        }

        private void UseObjectGrip(bool state)
        {
            OnObjectGrip?.Invoke(currentInterface, state);
        }

        private void UseObjectTrigger(float triggerValue)
        {
            OnObjectTrigger?.Invoke(currentInterface, triggerValue);
        }


        public void PickupNear()
        {
            currentNearRigidBody = GetNearRigidBody();

            if (!currentNearRigidBody)
            {
                if (currentFarRigidBody != null)
                {
                    BeginAttractFar(currentFarRigidBody);
                    return;
                }
                else
                {
                    return;
                }
            }

            GameObject currentNearObject = currentNearRigidBody.gameObject;

            if (currentNearObject.TryGetComponent(out Wieldable g))
            {
                if (!g.CanBeGrabbed(hand, transform))
                {
                    return;
                }
            }

            // keep hands kinematic
            if (currentNearObject.TryGetComponent(out XRController otherHand))
            {
                // space for two-handed gestures or actions
                return;
            }
            else
            {
                // pick up object
                currentNearRigidBody.MovePosition(transform.position);
                attachJoint[0].connectedBody = currentNearRigidBody;
                currentNearRigidBody.isKinematic = false;

                // set default grip pose
                SetGripPose(gripPose == "relax" ? "holdIt" : gripPose);
            }

            // check if controlling a fixed object
            if (currentNearObject.GetComponent<ControlDynamics>())
            {
                IsControllingObject = true;
            }

            // finally, share activity on the network
            if (currentNearObject.TryGetComponent(out SharedAsset sharedAsset))
            {
                if (!sharedAsset.IsBeingHandled)
                {
                    currentNearSharedAsset = sharedAsset;
                    currentNearSharedAsset.IsBeingHandled = true;

                    InvokeAcquisitionEvent(currentNearSharedAsset.Id, currentNearObject.transform, ManipulationDistance.Near);
                }
                else
                {
                    // asset is being being used
                    Debug.Log("This object is in use");
                }
            }
        }

        public void DropNear()
        {
            if (!currentNearRigidBody) return;

            if (currentNearRigidBody.gameObject.TryGetComponent(out Wieldable g))
            {
                if (g.Disengage(hand, transform))
                {
                    return;
                }
            }

            ThrowData newThrow;
            attachJoint[0].connectedBody = null;

            // local throw
            if (currentNearRigidBody.gameObject.TryGetComponent(out RigidDynamics rigidDynamics))
            {
                currentNearRigidBody.useGravity = rigidDynamics.UsesGravity;
                newThrow = rigidDynamics.Throw;
            }
            else
            {
                newThrow = new ThrowData()
                {
                    LinearForce = currentNearRigidBody.velocity,
                    AngularForce = currentNearRigidBody.angularVelocity
                };
            }

            currentNearRigidBody.AddForce(newThrow.LinearForce, ForceMode.Impulse);
            currentNearRigidBody.AddTorque(newThrow.AngularForce, ForceMode.Impulse);

            if (IsControllingObject)
            {
                currentNearRigidBody.gameObject.GetComponent<ControlDynamics>().FinishInteraction();
                IsControllingObject = false;
            }

            // network throw
            if (currentNearSharedAsset != null)
            {
                InvokeReleaseEvent(currentNearSharedAsset.Id, currentNearRigidBody.gameObject, ManipulationDistance.Near, newThrow);
                currentNearSharedAsset.IsBeingHandled = false;
                currentNearSharedAsset = null;
            }

            // clear grip pose
            SetGripPose("relax");
            currentNearRigidBody = null;
        }


        private void InvokeAcquisitionEvent(string target, Transform interactionTransform, ManipulationDistance distance)
        {
            if (!string.IsNullOrEmpty(target))
            {
                AcquireData newAcquisition = new AcquireData
                {
                    AcqTime = DateTime.UtcNow.Ticks,
                    ObjectPosition = interactionTransform.position,
                    ObjectRotation = interactionTransform.rotation
                };

                AvatarHandlingData interactionEvent = BuildEventFrame(target, distance, AvatarInteractionEventType.AcquireData, newAcquisition, null);

                OnHandInteraction?.Invoke(interactionEvent);
            }
        }


        private void InvokeReleaseEvent(string target, GameObject interactionObject, ManipulationDistance distance, ThrowData throwData)
        {
            if (!string.IsNullOrEmpty(target))
            {
                ReleaseData newRelease = new ReleaseData
                {
                    ReleaseTime = DateTime.UtcNow.Ticks,
                    ReleasePosition = interactionObject.transform.position,
                    ReleaseRotation = interactionObject.transform.rotation,
                    ForceData = throwData
                };

                AvatarHandlingData interactionEvent = BuildEventFrame(target, distance, AvatarInteractionEventType.ReleaseData, null, newRelease);

                OnHandInteraction.Invoke(interactionEvent);
            }
        }

        private AvatarHandlingData BuildEventFrame(string targetId, ManipulationDistance distance, AvatarInteractionEventType eventType, AcquireData acqDataFrame = null, ReleaseData relDataFrame = null)
        {
            AvatarHandlingData eventFrame = new AvatarHandlingData
            {
                Hand = hand,
                TargetId = targetId,
                Distance = distance,
                EventType = eventType,
                AcquisitionEvent = acqDataFrame,
                ReleaseEvent = relDataFrame
            };
            return eventFrame;
        }


        #endregion Interaction Functions


        #region >   Low-Level Interaction Methods

        private void SetActiveFarMesh()
        {
            GameObject currentObject = xrPointer.PlacePointer();

            // only active when there is a focused object
            if (currentObject != null)
            {
                string meshName = currentObject.name;

                if (currentObject.layer >= 9)
                {
                    pointingAtButton = false;
                    currentButton = null;

                    if (currentObject.GetComponent<RigidDynamics>())
                    {
                        if (meshName != prevMeshName)
                        {
                            farcontactRigidBodies.Clear();
                            farcontactRigidBodies.Add(currentObject.GetComponent<Rigidbody>());
                        }
                    }

                    if (currentObject.TryGetComponent(out PressableButton pb))
                    {
                        pointingAtButton = true;
                        currentButton = pb;
                    }
                }
                else
                {
                    if (!distanceManip)
                    {
                        farcontactRigidBodies.Clear();
                        prevMeshName = "";
                    }
                }

                prevMeshName = meshName;
            }
            else
            {
                if (!distanceManip)
                {
                    farcontactRigidBodies.Clear();
                    prevMeshName = "";
                }
                pointingAtButton = false;
                currentButton = null;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            // only in VR
            if (xrState == WebXRState.NORMAL && !debugHand) return;
            if ((Time.time - triggerEnterTick) < 0.1f) return;

            if (other.gameObject.layer >= 9)
            {
                if (other.gameObject.TryGetComponent(out PressableButton pb) &&
                    (Time.time - triggerEnterTick) > 0.1f)
                {
                    touchingButton = true;
                    currentButton = pb;
                    pb.ButtonPressed?.Invoke();
                }

                if (other.gameObject.GetComponent<RigidDynamics>())
                {
                    var rb = other.gameObject.GetComponent<Rigidbody>();
                    if (!nearcontactRigidBodies.Contains(rb))
                    {
                        nearcontactRigidBodies.Add(rb);
                    }
                }
                triggerEnterTick = Time.time;
            }
        }

        void OnTriggerExit(Collider other)
        {
            // only in VR
            if (xrState == WebXRState.NORMAL && !debugHand) return;
            if ((Time.time - triggerExitTick) < 0.1f) return;

            if (other.gameObject.layer >= 9)
            {
                if (other.gameObject.TryGetComponent(out PressableButton pb) &&
                    (Time.time - triggerEnterTick) > 0.2f)
                {
                    currentButton = null;
                    touchingButton = false;
                    pb.ButtonReleased?.Invoke();
                }

                if (other.gameObject.TryGetComponent(out RigidDynamics rd))
                {
                    var rb = other.gameObject.GetComponent<Rigidbody>();
                    if (nearcontactRigidBodies.Contains(rb))
                    {
                        nearcontactRigidBodies.Remove(rb);
                    }
                }
                triggerExitTick = Time.time;
            }
        }


        private Rigidbody GetDistantRigidBody()
        {
            Rigidbody nearestRigidBody = null;
            float minDistance = MaxInteractionDistance;
            float distance = 0.0f;

            foreach (Rigidbody contactBody in farcontactRigidBodies)
            {
                distance = (contactBody.gameObject.transform.position - transform.position).sqrMagnitude;

                if (distance < (minDistance * minDistance))
                {
                    minDistance = distance;
                    nearestRigidBody = contactBody.GetComponent<Rigidbody>();
                }
            }

            return nearestRigidBody;
        }

        private Rigidbody GetNearRigidBody()
        {
            Rigidbody nearestRigidBody = null;
            foreach (Rigidbody contactBody in nearcontactRigidBodies)
            {
                nearestRigidBody = contactBody.GetComponent<Rigidbody>();
            }
            return nearestRigidBody;
        }

        #endregion Low-Level Interaction Methods

    }
}

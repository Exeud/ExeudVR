/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Collections;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using ExeudVR.SharedAssets;
using UnityEngine;
using WebXR;
using System.Collections.Generic;

namespace ExeudVR
{
    /// <summary>
    /// The BodyController is the meeting point for all data and where they get packaged and sent across the network. 
    /// <para /><see href="https://github.com/willguest/ExeudVR/tree/develop/Documentation/Controllers/BodyController.md"/>
    /// </summary>
    public class BodyController : MonoBehaviour
    {
        // Singleton pattern
        private static BodyController _instance;
        public static BodyController Instance { get { return _instance; } }

        // Network Info
        public static string CurrentUserId { get; private set; }
        public static int CurrentNoPeers { get; set; }

        public ExeudVRAvatarController avatar { get; set; }

        public float CharacterHeight { 
            get { return transform.localPosition.y; }
            private set { _ = transform.localPosition.y; } 
        }

        // Network hook
        [DllImport("__Internal")]
        private static extern void SendData(string msg);

        // Event delegates
        public delegate void CursorFocus(GameObject focalObject, bool state);
        public delegate void ObjectTrigger(GameObject focalObject, float value);
        public delegate void ObjectGrip(ControllerHand hand, GameObject focalObject, bool state);

        // Body layout
        [SerializeField] private GameObject headObject;
        [SerializeField] private GameObject bodyObject;

        [SerializeField] private XRController leftController;
        [SerializeField] private XRController rightController;

        [SerializeField] private Transform leftPointer;
        [SerializeField] private Transform rightPointer;

        private bool IsConnectionReady = false;

        private Queue<string> nQ = new Queue<string>();
        private int nQc = 0;
        private float lastTick;
        private float frameTick;

        private bool notifyingNetwork = false;

        private void OnEnable()
        {
            WebXRManager.OnXRChange += OnXRChange;
        }

        private void OnDisable()
        {
            WebXRManager.OnXRChange -= OnXRChange;
            MapEvents(false);
        }

        private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            MapControllerEvents(state == WebXRState.VR);

            // toggle hand IK
            if (avatar != null)
            {
                if (state == WebXRState.NORMAL)
                {
                    avatar.RelaxArmRig();
                }
                else
                {
                    avatar.PrepareArmRig();
                }
            }
        }

        public Transform GetBodyReference(string bodyPart)
        {
            switch (bodyPart)
            {
                case "head":
                    return headObject.transform;
                case "body":
                    return bodyObject.transform;
                case "leftHand":
                    return leftController.HandAnchor.transform;
                case "rightHand":
                    return rightController.HandAnchor.transform;

                default:
                    return null;
            }
        }


        private void MapControllerEvents(bool isOn)
        {
            if (isOn)
            {
                rightController.OnObjectGrip += HandleObjectGrip;
                leftController.OnObjectGrip += HandleObjectGrip;

                rightController.OnObjectTrigger += HandleObjectTrigger;
                leftController.OnObjectTrigger += HandleObjectTrigger;

                leftController.OnHandInteraction += PackageEventData;
                rightController.OnHandInteraction += PackageEventData;
            }
            else
            {
                rightController.OnObjectGrip -= HandleObjectGrip;
                leftController.OnObjectGrip -= HandleObjectGrip;

                rightController.OnObjectTrigger -= HandleObjectTrigger;
                leftController.OnObjectTrigger -= HandleObjectTrigger;

                leftController.OnHandInteraction -= PackageEventData;
                rightController.OnHandInteraction -= PackageEventData;
            }
        }

        void MapEvents(bool isOn)
        {
            if (isOn)
            {
                if (AvatarManager.Instance)
                {
                    AvatarManager.Instance.OnDictionaryChanged += playersChanged;
                }
                if (NetworkIO.Instance)
                {
                    NetworkIO.Instance.OnConnectionChanged += SetConnectionReady;
                    NetworkIO.Instance.OnJoinedRoom += InitialiseDataChannel;
                }

                DesktopController.Instance.OnObjectFocus += HandleObjectFocus;
                DesktopController.Instance.OnObjectTrigger += HandleObjectTrigger;
                DesktopController.Instance.OnNetworkInteraction += PackageEventData;

            }
            else
            {
                if (AvatarManager.Instance)
                {
                    AvatarManager.Instance.OnDictionaryChanged -= playersChanged;
                }
                if (NetworkIO.Instance)
                {
                    NetworkIO.Instance.OnConnectionChanged -= SetConnectionReady;
                    NetworkIO.Instance.OnJoinedRoom -= InitialiseDataChannel;
                }

                IsConnectionReady = false;

                DesktopController.Instance.OnObjectFocus -= HandleObjectFocus;
                DesktopController.Instance.OnObjectTrigger -= HandleObjectTrigger;
                DesktopController.Instance.OnNetworkInteraction -= PackageEventData;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            MapEvents(true);
            nQ.Clear();

#if UNITY_EDITOR
            // for debugging
            MapControllerEvents(true);
#endif

            CurrentUserId = "Me";
            CurrentNoPeers = 0;

            StartCoroutine(SendPackets());
        }

        void Update()
        {
            if (IsConnectionReady && CurrentNoPeers > 0)
            {
                frameTick = Time.time;
                if ((frameTick - lastTick) > 0.25f)
                {
                    lastTick = frameTick;
                    SendDataFrame();
                }
            }
        }

        private void HandleObjectGrip(ControllerHand hand, GameObject controllable, bool state)
        {
            if (controllable != null && controllable.TryGetComponent(out ObjectInterface objInt))
            {
                objInt.SetGrip(GetHandController(hand), state);
            }
        }

        private void HandleObjectTrigger(GameObject controllable, float value)
        {
            if (controllable != null && controllable.TryGetComponent(out ObjectInterface objInt))
            {
                objInt.SetTrigger(value);
            }
        }

        private void HandleObjectFocus(GameObject go, bool state)
        {
            if (go != null && go.TryGetComponent(out ObjectInterface objInt))
            {
                objInt.ToggleActivation(go, state);
            }
        }

        private XRController GetHandController(ControllerHand hand)
        {
            return (hand == ControllerHand.RIGHT) ? rightController :
                (hand == ControllerHand.LEFT) ? leftController : null;
        }

        private void InitialiseDataChannel(string userid = "")
        {
            if (!notifyingNetwork)
            {
                StartCoroutine(StartAfterDelay(2.0f));
            }
        }

        private IEnumerator StartAfterDelay(float delay)
        {
            notifyingNetwork = true;
            yield return new WaitForSeconds(delay);

            SetConnectionReady(true);
            notifyingNetwork = false;
        }

        private void playersChanged(int numberofplayers)
        {
            CurrentNoPeers = numberofplayers;

            // send a packet to kick off comms
            if (CurrentNoPeers > 0)
            {
                SendDataFrame();
            }
        }

        private void SetConnectionReady(bool newState)
        {
            CurrentUserId = NetworkIO.Instance.CurrentUserId;
            IsConnectionReady = newState;
        }

        private void PackageEventData(AvatarHandlingData ahdFrame)
        {
            SendDataFrame(AvatarEventType.Interaction, JsonConvert.SerializeObject(ahdFrame));
        }

        public void EnqueuePacket(string message)
        {
            if (IsConnectionReady && CurrentNoPeers > 0)
            {
                nQ.Enqueue(message);
                nQc++;
            }
        }

        private void SendDataFrame(AvatarEventType eventType = AvatarEventType.None, string dataFrame = "")
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                NodeDataFrame dataToSend = new NodeDataFrame
                {
                    Id = CurrentUserId,
                    HeadPosition = headObject.transform.position,
                    HeadRotation = headObject.transform.rotation,
                    LeftHandPosition = leftController.transform.position,
                    RightHandPosition = rightController.transform.position,
                    LeftHandRotation = leftController.transform.rotation,
                    RightHandRotation = rightController.transform.rotation,

                    LeftHandPointer = leftPointer.position,
                    RightHandPointer = rightPointer.position,

                    EventType = eventType,
                    EventData = dataFrame
                };

                EnqueuePacket(JsonConvert.SerializeObject(dataToSend));
            }
        }

        private IEnumerator SendPackets()
        {
            while (true)
            {
                if (nQc > 0)
                {
                    string packet = nQ.Dequeue();
                    SendData(packet);
                    nQc--;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
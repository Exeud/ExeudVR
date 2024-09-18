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

using System.Collections.Generic;

namespace ExeudVR
{
    /// <summary>
    /// The BodyController is the meeting point for all data and where they get packaged and sent across the network. 
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Controllers/BodyController.md"/>
    /// </summary>
    public class BodyController : MonoBehaviour
    {
        // Singleton pattern
        private static BodyController _instance;
        public static BodyController Instance { get { return _instance; } }

        // Network Info
        public static string CurrentUserId { get; private set; }
        public static int CurrentNoPeers { get; set; }

        public ExeudVRAvatarController Avatar { get; set; }

        public float CharacterHeight { 
            get { return transform.localPosition.y; }
            private set { _ = transform.localPosition.y; } 
        }

        // Network hook
        [DllImport("__Internal")]
        private static extern void SendData(string msg);

        // Event delegates
        public delegate void CursorFocus(GameObject focalObject, bool state);
        public delegate void ObjectTrigger(ObjectInterface focalObject, float value);
        public delegate void ObjectGrip(ObjectInterface focalObject, bool state);

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

        private void OnDisable()
        {
            PlatformManager.Instance.OnStateChange -= OnXRChange;
            MapEvents(false);
        }

        private void OnXRChange(XRState state)
        {
            MapControllerEvents(state == XRState.VR);

            // toggle hand IK
            if (Avatar != null)
            {
                if (state == XRState.NORMAL)
                {
                    Avatar.RelaxArmRig();
                }
                else
                {
                    Avatar.PrepareArmRig();
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
                    NetworkIO.Instance.OnJoinedRoom += InitialiseDataChannel;
                }

                CursorManager.Instance.OnObjectFocus += HandleObjectFocus;
                CursorManager.Instance.OnObjectTrigger += HandleObjectTrigger;
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
                    NetworkIO.Instance.OnJoinedRoom -= InitialiseDataChannel;
                }

                IsConnectionReady = false;

                CursorManager.Instance.OnObjectFocus -= HandleObjectFocus;
                CursorManager.Instance.OnObjectTrigger -= HandleObjectTrigger;
                DesktopController.Instance.OnNetworkInteraction -= PackageEventData;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            PlatformManager.Instance.OnStateChange += OnXRChange;

            MapEvents(true);
            nQ.Clear();

#if UNITY_EDITOR
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

        private void HandleObjectGrip(ObjectInterface objInt, bool state)
        {
            if (objInt != null)
            {
                objInt.SetGrip(state);
            }
        }

        private void HandleObjectTrigger(ObjectInterface objInt, float value)
        {
            if (objInt != null)
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

        private void InitialiseDataChannel(string userId)
        {
            CurrentUserId = userId;
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
            IsConnectionReady = newState;
            if (newState) SendDataFrame();
        }

        private void PackageEventData(AvatarHandlingData ahdFrame)
        {
            SendDataFrame(AvatarEventType.Interaction, JsonConvert.SerializeObject(ahdFrame));
        }

        private void EnqueuePacket(string message)
        {
            if (IsConnectionReady)
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
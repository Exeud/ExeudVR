/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace ExeudVR
{
    /// <summary>
    /// The main entry point for P2P network communication. 
    /// <para /><see href="https://github.com/willguest/ExeudVR/tree/develop/Documentation/Multiplayer/NetworkIO.md"/>
    /// </summary>
    public class NetworkIO : MonoBehaviour
    {
        // singleton
        private static NetworkIO _instance;
        public static NetworkIO Instance { get { return _instance; } }

        [DllImport("__Internal")]
        private static extern void PrimeConnection(string sender, string socketURL, int capacity);

        [DllImport("__Internal")]
        private static extern void CeaseConnection();

        [DllImport("__Internal")]
        private static extern void ConfigureAudio();

        public delegate void NetworkUserEvent(string connectionStatus, string userId, string payload);
        public event NetworkUserEvent OnNetworkChanged;

        // interface
        public float NetworkUpdateFrequency { get; private set; }
        public bool ReadyFlag { get; set; }
        public string CurrentUserId { get; private set; }
        public bool IsConnected { get; private set; }

        // inspector objects
        [SerializeField] private string SignalingServerUrl = "https://rtcmulticonnection-sockets.herokuapp.com:443/";
        
        [SerializeField] private Renderer connectionIndicator;

        [SerializeField] private RoomManager roomManager;

        // private variables
        private RtcMultiConnection myConnection;
        private static List<string> connectedUsers;
        private List<string> previousOwnIds;

        private bool readyToReceive = false;
        private bool WaitingForOthers = false;
        
        // events
        public delegate void ConnectionEvent(bool connectionState);
        public event ConnectionEvent OnConnectionChanged;

        public delegate void RoomJoinEvent(string newUserId);
        public event RoomJoinEvent OnJoinedRoom;

        private float connectionStartTick = 0;

        private string myStatus;
        private string userInfo;
        private bool networkUpdateReady = false;


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                //DontDestroyOnLoad(this.gameObject); // option to keep between scenes
            }
        }

        private void Start()
        {
            StartManager.Instance.OnInitialised += OpenConnection;

            connectedUsers = new List<string>();
            previousOwnIds = new List<string>();

            connectionIndicator.material.EnableKeyword("_EMISSION");
            StartCoroutine(FadeToColour(connectionIndicator, Color.gray, 1.0f));

            myStatus = "Awake";
            CurrentUserId = "";
            userInfo = "";
            networkUpdateReady = true;
        }

        private void Update()
        {
            if (ReadyFlag)
            {
                readyToReceive = true;
                ReadyFlag = false;
            }

            if (networkUpdateReady)
            {
                networkUpdateReady = false;
                OnNetworkChanged?.Invoke(myStatus, CurrentUserId, userInfo);
            }
        }

        private void OnDisable()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                CeaseConnection();
            }
        }


        public void OpenJoin()
        {
            roomManager.OpenOrJoinRoom();
        }

        public void CloseRTC()
        {
            roomManager.LeaveRoom();
            CloseConnection();
        }

        public void MakeReady()
        {
            StartCoroutine(FadeToColour(connectionIndicator, Color.yellow, 2f));
            ReadyFlag = true;
        }


        private void OpenConnection()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                PrimeConnection(gameObject.name, SignalingServerUrl, roomManager.MaxPeers);
            }
        }

        private void CloseConnection()
        {
            readyToReceive = false;
            IsConnected = false;
            WaitingForOthers = false;

            connectedUsers.Clear();
            OnConnectionChanged.Invoke(false);

            StartCoroutine(FadeToColour(connectionIndicator, Color.red, 2f));
            AvatarManager.Instance.ResetScene();

            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                CeaseConnection();
            }
        }

        private void ConnectionClosed(string message)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                PrimeConnection(gameObject.name, SignalingServerUrl, roomManager.MaxPeers);
            }

            Debug.Log("Connection was reset");
        }

        private void OnFinishedLoadingRTC(string message)
        {
            try
            {
                myConnection = JsonConvert.DeserializeObject<RtcMultiConnection>(message);
                CurrentUserId = myConnection.Userid;
                if (!previousOwnIds.Contains(CurrentUserId))
                {
                    previousOwnIds.Add(CurrentUserId);
                }

            }
            catch (Exception e)
            {
                Debug.Log("Failed creating connection object:\n" + e.ToString());
                myConnection = null;
                networkUpdateReady = true;
                return;
            }

            myStatus = "Started";
            WaitingForOthers = true;
            networkUpdateReady = true;

            Debug.Log("WebRTC connection ready");
        }


        private void OnDestroy()
        {
            connectedUsers.Clear();
            readyToReceive = false;

            string mins = (Time.time / 60.0f).ToString();
            Debug.Log("Session destroyed after " + mins + " mins");

            StartManager.Instance.OnInitialised -= OpenConnection;
        } 

        private void RemoveAvatar(string avatarId)
        {
            if (!string.IsNullOrEmpty(avatarId))
            {
                DeleteAvatar(avatarId);
            }
        }

        private void ReceivePoseData(string message)
        {
            if (!readyToReceive)
            {
                return;
            }
            else if (string.IsNullOrEmpty(message))
            {
                Debug.Log("Data message was empty");
                return;
            }

            try
            {
                NodeInputData inputData = JsonConvert.DeserializeObject<NodeInputData>(message);
                HandlePoseData(inputData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error in pose deserialisation: " + e.Message);
            }
        }

        private void HandlePoseData(NodeInputData data)
        {
            if (string.IsNullOrEmpty(data.Data))
            {
                Debug.Log("null pose data");
                return;
            }

            AvatarManager.Instance.ProcessAvatarData(data);
        }


        private void OnConnectedToNetwork(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.Log("Connection event was empty");
                return;
            }

            ConnectionData cdata = JsonConvert.DeserializeObject<ConnectionData>(message);
            if (!connectedUsers.Contains(cdata.Userid) && !previousOwnIds.Contains(cdata.Userid))
            {
                Debug.Log("Someone showed up on the network: " + cdata.Userid);
                connectedUsers.Add(cdata.Userid);
            }
            else
            {
                Debug.Log("User '" + cdata.Userid + "' already known or is self");
            }

            StartCoroutine(FadeToColour(connectionIndicator, Color.green, 2f));

            myStatus = "Connected";
            IsConnected = true;
            ReadyFlag = true;
            WaitingForOthers = false;

            OnJoinedRoom.Invoke(CurrentUserId);
            networkUpdateReady = true;
        }

        private void OnDisconnectedFromNetwork(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.Log("Disconnection event was empty");
                return;
            }

            try
            {
                ConnectionData ddata = JsonConvert.DeserializeObject<ConnectionData>(message);

                DeleteAvatar(ddata.Userid);
                myStatus = "Disconnected";
                networkUpdateReady = true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error in disconnection:" + e.Message);
            }
        }

        private void DeleteAvatar(string avatarId)
        {
            if (connectedUsers.Contains(avatarId))
            {
                connectedUsers.Remove(avatarId);
                if (connectedUsers.Count < 1)
                {                
                    StartCoroutine(FadeToColour(connectionIndicator, Color.yellow, 1.0f));
                    myStatus = "Waiting for others";
                }
            }

            Debug.Log("Removing avatar:" + avatarId);
            AvatarManager.Instance.RemovePlayerAvatar(avatarId);
        }

        private IEnumerator FadeToColour(Renderer targetRenderer, Color endColour, float duration)
        {
            Color startColour = targetRenderer.material.color;
            float time = 0;

            while (time < duration)
            {
                Color cl = Color.Lerp(startColour, endColour, time / duration);
                targetRenderer.material.SetColor("_EmissionColor", cl);
                time += Time.deltaTime;
                yield return null;
            }
            
            targetRenderer.material.SetColor("_EmissionColor", endColour);
        }

    }
}
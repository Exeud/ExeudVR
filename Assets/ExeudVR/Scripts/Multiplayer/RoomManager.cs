/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v.2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ExeudVR 
{
    /// <summary>
    /// This class is responsible for managing rooms in a multiplayer space. It handles room creation, joining, and updating room information.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Multiplayer/RoomManager.md"/>
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        private List<RoomObject> rooms;

        // text assets for name generation
        private TextAsset fourLW;
        private TextAsset fiveLW;

        public int MaxPeers
        {
            get { return maxPeers; }
            set { maxPeers = value; }
        }

        private int maxPeers = 6;
        private bool publicRoom = true;
        private string roomString = "";
        private bool joinAnyRoom = false;

        private System.Random randomSeed;

        [DllImport("__Internal")]
        private static extern void RoomCheck(string sender);

        [DllImport("__Internal")]
        private static extern void OpenRoom(string sender, string roomId, int roomSize, bool isPublic);

        [DllImport("__Internal")]
        private static extern void JoinRoom(string sender, string roomId);

        void Start()
        {
            randomSeed = new System.Random();
            fourLW = Resources.Load("4LetterWords") as TextAsset;
            fiveLW = Resources.Load("5LetterWords") as TextAsset;
            rooms = new List<RoomObject>();
        }


    #region Interface

        public void CheckForRooms()
        {
            RoomCheck(gameObject.name);
        }

        public void JoinAnyAvailableRoom()
        {
            joinAnyRoom = true;
            CheckForRooms();
        }

        public void SetRoomSize(int newCapacity)
        {
            maxPeers = newCapacity;
        }

        public void SetRoomMode(bool isPublic)
        {
            publicRoom = isPublic;
        }

        public void CreateRoom()
        {
            OpenRoom(gameObject.name, roomString, maxPeers, publicRoom);
        }

        public void LeaveRoom()
        {
            roomString = string.Empty;
            randomSeed = new System.Random();
        }

    #endregion Interface


        private string GetNewRoomName()
        {
            string[] words = new string[2] { GetRandomWord(fiveLW.bytes, 5), GetRandomWord(fourLW.bytes, 4) };
            return string.Join(" ", words);
        }

        private void SetRoomName(string newName)
        {
            roomString = newName;
        }

        private string GetRandomWord(byte[] source, int wordLength)
        {
            string foundWord = "";
            char[] subset = new char[10];

            Array.Copy(source, randomSeed.Next(1, source.Length - 10), subset, 0, 10);
            string[] midStr = new string(subset).Split(',');

            foreach (string s in midStr)
            {
                if (s.Length == wordLength)
                {
                    foundWord = s;
                    break;
                }
            }
            return foundWord;
        }

        private void OpenOrJoinRoom()
        {
            string roomCandidate = GetAvailableRoom();

            if (!string.IsNullOrEmpty(roomCandidate))
            {
                JoinRoom(gameObject.name, roomCandidate);
            }
            else
            {
                roomString = GetNewRoomName();
                CreateRoom();
            }
        }

        private string GetAvailableRoom()
        {
            string availableRoom = string.Empty;
            for (int r = 0; r < rooms.Count; r++)
            {
                if ((int)rooms[r].Participants.Length < (int)rooms[r].MaxParticipantsAllowed)
                {
                    availableRoom = rooms[r].SessionId;
                    Debug.Log("Available room: " + availableRoom);
                }
            }
            return availableRoom;
        }

        private void RoomCheckComplete(string message)
        {
            if (joinAnyRoom)
            {
                OpenOrJoinRoom();
            }
        }

        private void RoomCheckFailed(string errorMessage)
        {
            Debug.Log("Room check failed: " + errorMessage);
        }

        private void RoomCreated(string roomId)
        {
            roomString = JsonConvert.DeserializeObject<string>(roomId);
            Debug.Log("You made a room of " + roomString);
            NetworkIO.Instance.MakeReady();
        }

        private void RoomJoined(string roomId)
        {
            roomString = JsonConvert.DeserializeObject<string>(roomId);
            Debug.Log("You joined a room of " + roomString);
            NetworkIO.Instance.MakeReady();
        }

        private void RoomIsFull(string roomId)
        {
            string fullRoom = JsonConvert.DeserializeObject<string>(roomId);
            Debug.Log("Room is full: " + fullRoom);
        }

        private void RoomNotFound(string roomId)
        {
            string missingRoom = JsonConvert.DeserializeObject<string>(roomId);
            
            // check for an remove bad room
            for (int r = 0; r < rooms.Count; r++)
            {
                if (rooms[r].SessionId == missingRoom)
                {
                    rooms.RemoveAt(r);
                    Debug.Log("Removed missing room: " + missingRoom);
                }
            }

            OpenOrJoinRoom();
        }


        private void RoomFound(string message)
        {
            RoomObject newRoom = JsonConvert.DeserializeObject<RoomObject>(message);
            bool roomExists = false;

            // If room already exists, update it
            for (int r = 0; r < rooms.Count; r++)
            {
                if (rooms[r].SessionId == newRoom.SessionId)
                {
                    roomExists = true;

                    RoomObject room = rooms[r];
                    room.Owner = newRoom.Owner;
                    room.Participants = newRoom.Participants;
                    room.MaxParticipantsAllowed = newRoom.MaxParticipantsAllowed;
                    room.Session = newRoom.Session;
                }
            }

            // Extend array with new room
            if (!roomExists)
            {
                rooms.Add(newRoom);
                Debug.Log("Room found: " + newRoom.SessionId);
            }
        }

    }
}


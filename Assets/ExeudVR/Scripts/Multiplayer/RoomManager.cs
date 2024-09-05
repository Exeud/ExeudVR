/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v.2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ExeudVR 
{
    /// <summary>
    /// This class is responsible for managing rooms in a multiplayer game. It handles room creation, joining, and updating room information.
    /// <para /><see href="https://github.com/willguest/ExeudVR/tree/develop/Documentation/Multiplayer/RoomManager.md"/>
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        private RoomObject[] rooms;
        private string currentRoom;

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
            currentRoom = string.Empty;
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
            for (int r = 0; r < rooms.Length; r++)
            {
                if ((int)rooms[r].Participants.Length < (int)rooms[r].MaxParticipantsAllowed)
                {
                    availableRoom = rooms[r].Sessionid;
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

        private void RoomCreated(string message)
        {
            currentRoom = message;
            roomString = currentRoom;

            Debug.Log("You made a room of " + roomString);
            NetworkIO.Instance.MakeReady();
        }

        private void RoomJoined(string message)
        {
            currentRoom = message;
            roomString = currentRoom;

            Debug.Log("You joined a room of " + roomString);
            NetworkIO.Instance.MakeReady();
        }

        private void RoomIsFull(string roomId)
        {
            Debug.Log("Room is full");
        }

        private void RoomFound(string message)
        {
            RoomObject newRoom = JsonConvert.DeserializeObject<RoomObject>(message);
            bool roomExists = false;
            if (rooms == null)
            {
                rooms = new RoomObject[0];
            }

            // If room already exists, update it
            for (int r = 0; r < rooms.Length; r++)
            {
                RoomObject room = rooms[r];

                if (room.Sessionid == newRoom.Sessionid)
                {
                    // room already exists
                    roomExists = true;
                    room.Owner = newRoom.Owner;
                    room.Participants = newRoom.Participants;
                    room.MaxParticipantsAllowed = newRoom.MaxParticipantsAllowed;
                    room.Session = newRoom.Session;
                }
            }

            // Extend array with new room
            if (!roomExists)
            {
                Array.Resize(ref rooms, rooms.Length + 1);
                rooms[rooms.Length - 1] = newRoom;
            }
        }

    }
}


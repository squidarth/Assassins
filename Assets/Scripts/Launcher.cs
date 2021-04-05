using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System;

namespace Com.Assassins
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";
        private static System.Random random = new System.Random();

        [Tooltip("Max Players Per Room")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        string roomToJoin;


        bool isConnecting;

        private string generateRoomName()
        {
            int length = 6;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            
        }

        public void SetRoomToJoin(string roomCode)
        {
            roomToJoin = roomCode;
        }

        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(roomToJoin);
            }

        }

        public void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(generateRoomName());
            }
        }

        public void Start()
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                Debug.Log("OnConnectedToMaster called");
             }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("On Disconnected called with reason {0}", cause);
            isConnecting = false;
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom , PublishUserId = true});
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("MainScene");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;


namespace Com.Assassins
{

    
    public class GameManager : MonoBehaviourPunCallbacks
    {

        private System.Random random = new System.Random();
        public GameObject playerPrefab;

        void Start()
        {

            int startX = random.Next(-4, 4);
            int startY = random.Next(-4, 4);
            if (playerPrefab == null)
            {
                Debug.LogError("No prefab found");
            } else
            {
                Debug.Log("Instantiating the player");
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(startX, startY, 0), Quaternion.identity, 0);
                } else
                {

                }
            }
        }
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Not the master client");
            }
            Debug.LogFormat("PhotonNetwork, loading level with {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            if (SceneManager.GetActiveScene().name != "MainScene")
            {
                PhotonNetwork.LoadLevel("MainScene");
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom {0}", other.NickName);

            if (PhotonNetwork.IsMasterClient)
            {

                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }

        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }
    }
}


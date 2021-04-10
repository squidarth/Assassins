
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;


namespace Com.Assassins
{

    
    public class GameManager : MonoBehaviourPunCallbacks
    {

        private System.Random random = new System.Random();
        public GameObject playerPrefab;
        public GameObject roundSystemPrefab;
        public List<Player> currentPlayers;
        public RoundSystem roundSystem;

        public delegate void GamePlayersChanged(List<Player> players);
        public static event GamePlayersChanged OnGamePlayersChanged;

        public delegate void GameStateEnded(string winnerId);
        public static event GameStateEnded OnGameStateEnded;

        public delegate void GameStateStarted();
        public static event GameStateStarted OnGameStateStarted;

        public delegate void LivePlayersUpdated();
        public static event LivePlayersUpdated OnLivePlayersUpdated;


        public void handleDeath(string attackerId, string attackedId)
        {
            roundSystem.photonView.RPC("Death", RpcTarget.All, attackerId, attackedId);

        }

        public void UpdateLivePlayers()
        {
            OnLivePlayersUpdated();
        }

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
                    var playerObject = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(startX, startY, 0), Quaternion.identity, 0);
                }
            }


            if(PhotonNetwork.IsMasterClient)
            {
                GameObject roundSystemObject = PhotonNetwork.Instantiate(this.roundSystemPrefab.name, new Vector3(0,0,0), Quaternion.identity, 0);
                roundSystem = roundSystemObject.GetComponent<RoundSystem>();
                roundSystem.gameManager = this;
            } else
            {
                roundSystem = GameObject.FindObjectOfType<RoundSystem>();
                //roundSystem.gameManager = this;
            }



            this.currentPlayers = new List<Player>(PhotonNetwork.PlayerList);
            OnGamePlayersChanged(currentPlayers);
        }

        public void StartRound()
        {
            var userIds = from player in currentPlayers select player.ActorNumber.ToString();
            string[] shuffledUserIds = userIds.OrderBy(a => Guid.NewGuid()).ToArray();
            Debug.Log(shuffledUserIds);
            foreach (var id in shuffledUserIds)
            {
                Debug.Log(id);
            }

            roundSystem.photonView.RPC("StartRound", RpcTarget.All, shuffledUserIds, 0);

        }

        public void RoundStarted()
        {
            OnGameStateStarted();
        }

        public void GameComplete(string winnerUserId)
        {
            OnGameStateEnded(winnerUserId);
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Not the master client");
            }

            Debug.Log("Called LoadArena");
            if (SceneManager.GetActiveScene().name != "MainScene")
            {
                Debug.Log("Creating round system");
                PhotonNetwork.LoadLevel("MainScene");
                Debug.Log(roundSystem);
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

            currentPlayers.Add(other);

            OnGamePlayersChanged(currentPlayers);
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }

            currentPlayers.Remove(other);
            OnGamePlayersChanged(currentPlayers);
        }
    }
}


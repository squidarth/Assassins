using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

namespace Com.Assassins
{
    public class RoomInfoManager : MonoBehaviourPunCallbacks
    {
        public GameManager gameManager;
        public Text roomCodeText;
        public Transform GameEventPanel;
        public TextMeshProUGUI gameEventText;
        public GameObject startButton;

        void Start()
        {
            GameManager.OnGamePlayersChanged += OnGamePlayersChanged;
            GameManager.OnGameStateEnded += OnGameStateChanged;
            GameManager.OnGameStateStarted += Render;
            Render();
            GameEventPanel.gameObject.SetActive(false);
        }

        private void OnGameStateChanged(string winnerId)
        {
            Player winner = gameManager.currentPlayers.Find((player) => player.ActorNumber.ToString() == winnerId);
            gameEventText.SetText("The winner was  " + winner.NickName);
            GameEventPanel.gameObject.SetActive(true);
            Render();
        }

        private void OnGamePlayersChanged (List<Player> players)
        {
            Render();
        }
        private void Render()
        {
            roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
            if (!PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(false);
            } else if (gameManager.roundSystem && gameManager.roundSystem.gameInProgress)
            {
                startButton.SetActive(false);
            } else
            {
                startButton.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


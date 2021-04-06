using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using TMPro;
using Photon.Realtime;

namespace Com.Assassins
{
    public class PlayerListSidebar : MonoBehaviour
    {
        // Start is called before the first frame update

        public TextMeshProUGUI playerListingTextField;
        public TextMeshProUGUI gameInProgressTextField;
        public Transform playerListPanel;
        public GameObject playerNamePrefab;

        public GameManager gameManager;

        private string renderPlayerName(Player player)
        {
            var nameToRender = player.NickName;
            if (player.IsLocal)
            {
                nameToRender += " (You)";
            }

            if(player.IsMasterClient)
            {
                nameToRender += "(Host)";
            }

            return nameToRender;

        }

        public void Render()
        {
            var players = gameManager.currentPlayers;
            var localActorNumber = PlayerManager.LocalPlayerInstance.GetPhotonView().Owner.ActorNumber;


                for (int i = 0;i < playerListPanel.childCount;i++) {
                    GameObject.Destroy(playerListPanel.GetChild(i).gameObject);
                }

                foreach (var player in players)
                {
                    var playerNameObject = Instantiate(playerNamePrefab, playerListPanel);
                    var textMesh = playerNameObject.GetComponent<TextMeshProUGUI>();

                    if (gameManager.roundSystem && gameManager.roundSystem.gameInProgress && gameManager.roundSystem.targets[localActorNumber.ToString()] == player.ActorNumber.ToString())
                    {

                        textMesh.SetText("💀" + renderPlayerName(player));
                        textMesh.color = new Color32(214, 34, 0, 255);
                    } else
                    {
                        textMesh.SetText(renderPlayerName(player));
                    }
                            
                }

            if (gameManager.roundSystem.gameInProgress)
            {
                gameInProgressTextField.SetText("Game In Progress");
            } else
            {
                gameInProgressTextField.SetText("No Game In Progress");
            }
        }

        public void OnGamePlayersChanged(List<Player> players)
        {
            Render();
        }

        public void OnGameStateEnded(string winnerId)
        {

            Render();
        }

        private void OnEnable()
        {
            GameManager.OnGamePlayersChanged += OnGamePlayersChanged;
            GameManager.OnGameStateStarted += Render;
            GameManager.OnGameStateEnded += OnGameStateEnded;
        }
    }
}


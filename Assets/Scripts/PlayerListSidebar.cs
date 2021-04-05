using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            string stringToRender;

            if (players == null)
            {
                stringToRender = "";
            } else
            {
                List<string> playerNames = (
                  from player in players select renderPlayerName(player)
                ).ToList();
                stringToRender = string.Join("\n", playerNames);
            }

            playerListingTextField.SetText(stringToRender);

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


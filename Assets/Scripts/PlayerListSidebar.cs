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

        private TextMeshProUGUI textField;

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

        public void Render(List<Player> players)
        {
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

            textField.SetText(stringToRender);
        }

        private void OnEnable()
        {
            GameManager.OnGamePlayersChanged += Render;
        }

        private void OnDisable()
        {
            GameManager.OnGamePlayersChanged -= Render;
        }
        void Start()
        {
            textField = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}


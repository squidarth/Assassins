using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Assassins
{
    public class CharacterUI : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI nameField;
        private PlayerManager playerManager;
        private PlayerCombat playerCombat;

        // Start is called before the first frame update
        void Start()
        {
            this.playerManager = GetComponentInParent<PlayerManager>();
            this.playerCombat = GetComponentInParent<PlayerCombat>();
            this.playerCombat.OnDie += SetText;
            SetText();
        }

        void SetText()
        {
            var text = playerManager.owner.NickName;
            if (playerCombat.IsDead)
            {
                text += "(Dead)";
            }

            nameField.SetText(text);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


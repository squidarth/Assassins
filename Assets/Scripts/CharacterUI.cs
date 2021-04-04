using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Assassins
{
    public class CharacterUI : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI nameField;
        private PlayerManager playerManager;
        // Start is called before the first frame update
        void Start()
        {
            this.playerManager = GetComponentInParent<PlayerManager>();
            nameField.SetText(playerManager.owner.NickName);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


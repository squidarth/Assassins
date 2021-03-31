using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Assassins
{
    public class LootBox : MonoBehaviourPunCallbacks
    {
        public List<ItemType> items;
        public LootBoxUI lootBoxUI;

        [PunRPC]
        public void RemoveItem(ItemType item)
        {
            items.Remove(item);
        }

        private void Start()
        {
            lootBoxUI = GameObject.Find("Lootbox Panel").GetComponent<LootBoxUI>();
        }

        public void transferItemToPlayer(ItemType item)
        {
            photonView.RPC("RemoveItem", RpcTarget.All, item);
            var playerPhotonView = PlayerManager.LocalPlayerInstance.GetPhotonView();
            playerPhotonView.RPC("AcquireItem", RpcTarget.All, item);
            lootBoxUI.Render();
        }

        public void Open()
        {
            lootBoxUI.lootBox = this;

            lootBoxUI.Render();
            lootBoxUI.toggle();
        }

        public void CloseBox()
        {
            lootBoxUI.Close();
        }
    }
}



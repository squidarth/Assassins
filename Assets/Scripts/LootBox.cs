using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Assassins
{
    public class LootBox : MonoBehaviourPunCallbacks
    {
        public List<ItemObject> originalItems;
        public List<ItemObject> items;
        public LootBoxUI lootBoxUI;

        [PunRPC]
        public void RemoveItem(int itemIndex)
        {
            items.Remove(StaticItemManager.FromIndex(itemIndex));
        }

        private void Start()
        {
            lootBoxUI = GameObject.Find("Lootbox Panel").GetComponent<LootBoxUI>();
            originalItems = new List<ItemObject>(items);
            GameManager.OnGameStateEnded += (string winnerId) =>
            {
                items = new List<ItemObject>(originalItems);
            };
        }

        public void transferItemToPlayer(ItemObject item)
        {
            photonView.RPC("RemoveItem", RpcTarget.All, StaticItemManager.ToIndex(item));
            var playerPhotonView = PlayerManager.LocalPlayerInstance.GetPhotonView();
            playerPhotonView.RPC("AcquireItem", RpcTarget.All, StaticItemManager.ToIndex(item));
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



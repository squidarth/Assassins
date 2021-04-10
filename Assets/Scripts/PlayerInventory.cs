using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

namespace Com.Assassins
{

    public class PlayerInventory : MonoBehaviourPunCallbacks
    {
        public List<ItemObject> items;
        public WeaponObject primaryWeapon;

        public delegate void ItemsUpdated();
        public event ItemsUpdated OnItemsUpdated;

        private void Start()
        {
            GameManager.OnGameStateEnded += (string _) =>
            {
                items = new List<ItemObject>();
                primaryWeapon = null;
                if (photonView.IsMine)
                {
                    OnItemsUpdated();
                }

                RemoveWeaponDisplay(FindWeaponVisual());
            };
        }

        [PunRPC]
        public void AcquireItem(int itemIndex)
        {
            items.Add(StaticItemManager.FromIndex(itemIndex));
            if (photonView.IsMine)
            {
                OnItemsUpdated();
            }
        }

        public void RemoveWeaponDisplay(Transform weaponContainer)
        {
            for (int i = 0; i< weaponContainer.childCount;i++)
            {
                GameObject.Destroy(weaponContainer.GetChild(i).gameObject);
            }
        }

        private Transform FindWeaponVisual()
        {
            return transform.Find("Visual").Find("Weapon");
        }

        [PunRPC]
        public void EquipWeapon(int itemIndex)
        {
            var item = StaticItemManager.FromIndex(itemIndex);
            var weaponContainer = FindWeaponVisual();
            RemoveWeaponDisplay(weaponContainer);
            if ((item.type) == ItemCategory.Weapon)
            {
                items.Remove(item);
                primaryWeapon = (WeaponObject) item;
                var newWeapon = Instantiate(primaryWeapon.displayPrefab, weaponContainer.transform);
                if (photonView.IsMine)
                {
                    OnItemsUpdated();
                }
            }
        }

        public void LocalEquipWeapon(ItemObject itemObject)
        {
            var playerPhotonView = PlayerManager.LocalPlayerInstance.GetPhotonView();
            playerPhotonView.RPC("EquipWeapon", RpcTarget.All, StaticItemManager.ToIndex(itemObject));
        }
    }
}


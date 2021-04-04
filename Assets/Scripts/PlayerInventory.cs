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

        [PunRPC]
        public void AcquireItem(int itemIndex)
        {
            items.Add(StaticItemManager.FromIndex(itemIndex));
            if (photonView.IsMine)
            {
                OnItemsUpdated();
            }
        }

        [PunRPC]
        public void EquipWeapon(int itemIndex)
        {
            var item = StaticItemManager.FromIndex(itemIndex);
            var weaponContainer = transform.Find("Weapon");
            if ((item.type) == ItemCategory.Weapon)
            {
                items.Remove(item);
                primaryWeapon = (WeaponObject) item;
                var newWeapon = Instantiate(primaryWeapon.displayPrefab, weaponContainer.transform);
                //newWeapon.transform.SetParent(weaponContainer);
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


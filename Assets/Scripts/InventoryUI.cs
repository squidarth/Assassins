using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Assassins
{
    public class InventoryUI : MonoBehaviour
    {
        public PlayerInventory inventory;

        private bool isOpen = false;
        public void SetInventory(PlayerInventory inventory)
        {
            this.inventory = inventory;
            inventory.OnItemsUpdated += Render;
        }

        public void OnToggleInventory()
        {
            if (isOpen)
            {
                GetComponent<Canvas>().enabled = false;
                isOpen = false;
            } else
            {
                GetComponent<Canvas>().enabled = true;
                isOpen = true;
            }
        }

        private void Start()
        {
            
            GetComponent<Canvas>().enabled = false;
        }

        void RenderInventory()
        {
            var inventoryTransform = transform.Find("Inventory Panel");
            int children = inventoryTransform.childCount;
            for (int i = children -1;i>=0;i--)
            {
                GameObject.Destroy(inventoryTransform.GetChild(i).gameObject);
            }
            foreach (var item in inventory.items)
            {
                GameObject availableItem = (GameObject)Instantiate(item.inventoryDisplayPrefab);
                availableItem.transform.SetParent(inventoryTransform, false);
                Button tmpButton = availableItem.GetComponent<Button>();
                if (item.type == ItemCategory.Weapon)
                {
                    tmpButton.onClick.AddListener(() => inventory.LocalEquipWeapon(item));
                }

            }
        }

        void RenderEquipmentSection()
        {
            var equipped = transform.Find("Equipped Panel");
            int children = equipped.childCount;
            for (int i = children -1;i>=0;i--)
            {
                GameObject.Destroy(equipped.GetChild(i).gameObject);
            }

            if (inventory.primaryWeapon != null) {
                GameObject availableItem = (GameObject)Instantiate(inventory.primaryWeapon.inventoryDisplayPrefab);
                availableItem.transform.SetParent(equipped, false);
                Button tmpButton = availableItem.GetComponent<Button>();
            }
        }

        void Render()
        {
            RenderInventory();
            RenderEquipmentSection();
        }
    }
}


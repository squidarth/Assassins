using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Assassins
{
    public class InventoryUI : MonoBehaviour
    {
        private PlayerInventory inventory;
        public GameObject itemDisplayPrefab;
        private bool isOpen = false;
        public void SetInventory(PlayerInventory inventory)
        {
            this.inventory = inventory;
        }

        public void OnToggleInventory()
        {
            if (isOpen)
            {
                this.gameObject.GetComponentInParent<Canvas>().enabled = false;
                isOpen = false;
            } else
            {
                Render();
                this.gameObject.GetComponentInParent<Canvas>().enabled = true;
                isOpen = true;
            }
        }

        private void Start()
        {
            
            this.gameObject.GetComponentInParent<Canvas>().enabled = false;
        }

        void Render()
        {
            int children = transform.childCount;
            for (int i = children -1;i>=0;i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            foreach (var item in inventory.items)
            {
                Debug.LogFormat("Processing item {0}", item);
                GameObject availableItem = (GameObject)Instantiate(itemDisplayPrefab);
                availableItem.transform.SetParent(transform, false);
                Button tmpButton = availableItem.GetComponent<Button>();
            }

        }
    }
}


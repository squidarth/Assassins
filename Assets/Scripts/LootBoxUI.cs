using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Assassins
{
    public class LootBoxUI : MonoBehaviour
    {

        public LootBox lootBox;

        public GameObject itemDisplayPrefab = null;
        private bool IsOpen = false;
        // Start is called before the first frame update
        void Start()
        {
            this.gameObject.GetComponentInParent<Canvas>().enabled = false;
        }

        public void Close()
        {
            this.gameObject.GetComponentInParent<Canvas>().enabled = false;
            IsOpen = false;
        }

        public void toggle()
        {
            if (IsOpen)
            {
                this.gameObject.GetComponentInParent<Canvas>().enabled = false;
                IsOpen = false;
            } else
            {
                this.gameObject.GetComponentInParent<Canvas>().enabled = true;
                IsOpen = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Render()
        {
            int children = transform.childCount;
            for (int i = children-1;i>=0;i--) {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }


            foreach (var item in lootBox.items)
            {
                Debug.LogFormat("Processing item {0}", item);
                GameObject availableItem = (GameObject)Instantiate(item.inventoryDisplayPrefab);
                availableItem.transform.SetParent(transform, false);
                Button tmpButton = availableItem.GetComponent<Button>();

                // TODO: Set the icon on these buttons
                tmpButton.onClick.AddListener(() => lootBox.transferItemToPlayer(item));
            }
            
        }


    }
}


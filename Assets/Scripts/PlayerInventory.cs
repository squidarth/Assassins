using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public enum ItemType
{
    Sword,
    Gun,
    Slingshot
}

namespace Com.Assassins
{

    public class PlayerInventory : MonoBehaviourPunCallbacks
    {
        public List<ItemType> items;

        [PunRPC]
        public void AcquireItem(ItemType item)
        {
            items.Add(item);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


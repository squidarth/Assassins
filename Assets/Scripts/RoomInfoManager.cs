using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.Assassins
{
    public class RoomInfoManager : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject roomCodeComponent = GameObject.Find("Room Code");
            var textComponent = roomCodeComponent.GetComponent<Text>();
            textComponent.text = PhotonNetwork.CurrentRoom.Name;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


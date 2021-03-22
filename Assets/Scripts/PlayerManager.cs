using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

namespace Com.Assassins
{

    public class PlayerManager : MonoBehaviourPunCallbacks
    {

        private Vector2 moveVec;
        public float speed = 10;
        private Rigidbody2D controller;
        public static GameObject LocalPlayerInstance;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnMove(InputValue input)
        {
            Vector2 inputVec = input.Get<Vector2>();
            moveVec = new Vector2(inputVec.x, inputVec.y);
            
        }
        // Start is called before the first frame update
        void Start()
        {
            controller = this.gameObject.GetComponent<Rigidbody2D>();
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                // Check network status
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            
        }

        void FixedUpdate()
        {
            if (moveVec != Vector2.zero && (this.gameObject == PlayerManager.LocalPlayerInstance || !PhotonNetwork.IsConnected))
            {
                Vector2 oldPosition = new Vector2(transform.position.x, transform.position.y);
                controller.MovePosition(oldPosition + (moveVec * speed * Time.deltaTime));
            }
        }
    }
}


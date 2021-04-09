using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;

namespace Com.Assassins
{

    public class PlayerManager : MonoBehaviourPunCallbacks
    {

        private Vector2 moveVec;
        public float speed = 10;
        private Rigidbody2D controller;
        public static GameObject LocalPlayerInstance;
        public Player owner;
        private bool facingRight = true;
        private bool isGhost = false;

        /* Loot box in proximity of this player */
        private LootBox lootBoxInRange;

        private void Awake()
        {

            this.owner = this.gameObject.GetPhotonView().Owner;
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                var inventoryUI = GameObject.Find("Inventory Canvas").GetComponent<InventoryUI>();
                var inventory = GetComponent<PlayerInventory>();
                inventoryUI.SetInventory(inventory);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void TurnToGhost()
        {
            isGhost = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CapsuleCollider2D>().isTrigger = true;
        }

        public void ComeBackToLife()
        {

            isGhost = false;
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<CapsuleCollider2D>().isTrigger = false;
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

            PlayerCombat combat = this.gameObject.GetComponent<PlayerCombat>();
            combat.OnDie += TurnToGhost;
            GameManager.OnGameStateEnded += (string winnerId) => ComeBackToLife();

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
                RaycastHit2D[] hits = new RaycastHit2D[10];
                ContactFilter2D filter = new ContactFilter2D()
                {
                    layerMask = 1 << LayerMask.NameToLayer("Player"),
                    useLayerMask = true
                };
                var numCollisions = controller.Cast(moveVec, filter, hits, 0.6f);

                if (numCollisions == 0 || isGhost || hits[0].rigidbody.isKinematic)
                {
                    controller.MovePosition(oldPosition + (moveVec * speed * Time.deltaTime));
                }

                if (moveVec.x > 0 && !facingRight || moveVec.x < 0 && facingRight)
                {
                    transform.Find("Visual").transform.Rotate(new Vector3(0, 180, 0));
                    if (moveVec.x > 0)
                    {
                        facingRight = true;
                    } else if (moveVec.x < 0)
                    {
                        facingRight = false;
                    }
                }
            }
        }
            
        void OnOpenLootBox()
        {
            if (lootBoxInRange)
            {
                lootBoxInRange.Open();
                Debug.Log("Found loot box in range.");
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Entered a trigger range");
            if (collision.gameObject.tag == "LootBox" && (this.gameObject == PlayerManager.LocalPlayerInstance || !PhotonNetwork.IsConnected ))
            {
                lootBoxInRange = collision.gameObject.GetComponent<LootBox>();
                Debug.LogFormat("setting lootBoxInragne to {0}", lootBoxInRange);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "LootBox" && lootBoxInRange != null)
            {
                Debug.Log("Leaving range of loot box");
                lootBoxInRange.CloseBox();
                lootBoxInRange = null;
                
            }
        }
    }
}


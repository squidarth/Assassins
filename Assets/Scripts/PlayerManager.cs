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
            controller.isKinematic = true;
            transform.Find("CharacterCollision").gameObject.SetActive(false);

        }

        public void ComeBackToLife()
        {
            controller.isKinematic = false;
            transform.Find("CharacterCollision").gameObject.SetActive(true);
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

                Debug.LogFormat("Printing numCollisions: {0}", numCollisions);
                if (numCollisions == 0)
                {
                    controller.MovePosition(oldPosition + (moveVec * speed * Time.deltaTime));
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


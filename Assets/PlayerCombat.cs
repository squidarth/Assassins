using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


namespace Com.Assassins
{
    public class PlayerCombat : MonoBehaviourPunCallbacks
    {
        public bool IsDead;
        public float attackRange;

        public GameManager gameManager;

        public delegate void Die();
        public event Die OnDie;

        [PunRPC]
        void PerformAttack(string attackerId, string attackedId)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(players.Length);

            foreach(var player in players)
            {
                Debug.Log(player);
                if (player.GetPhotonView().Owner.ActorNumber.ToString() == attackedId)
                {
                    var playerCombat = player.GetComponent<PlayerCombat>();
                    playerCombat.IsDead = true;
                    playerCombat.OnDie();
                    gameManager.roundSystem.Death(attackerId, attackedId);
                } else if (player.GetPhotonView().Owner.ActorNumber.ToString() == attackerId)
                {
                    var animation = player.transform.Find("Visual").Find("Weapon").GetComponentInChildren<Animation>();
                    player.GetComponent<PlayerInventory>().loseWeapon();
                    if (animation != null)
                    {
                        animation.Play();
                    }

                }
            }
        }

        void OnAttack()
        {
            if (!photonView.IsMine || !gameManager.roundSystem.gameInProgress)
            {
                return;
            }

            var myUserId = photonView.Owner.ActorNumber;
            Debug.Log("Performing attack");
            if (GetComponent<PlayerInventory>().primaryWeapon != null)
            {
                Debug.Log(LayerMask.NameToLayer("Player"));
                Debug.Log("Performing attack with weapon");
                Debug.Log(new Vector2(transform.position.x, transform.position.y));

                RaycastHit2D[] hits = new RaycastHit2D[10];
                ContactFilter2D filter = new ContactFilter2D()
                {
                    layerMask = 1 << LayerMask.NameToLayer("Player"),
                    useLayerMask = true
                };

                Vector2 direction = GetComponent<PlayerManager>().facingRight ? new Vector2(1, 0) : new Vector2(-1, 0);
                var numCollisions = GetComponent<Rigidbody2D>().Cast(direction, filter, hits, 1);

                var colliders = from hit in hits select hit.collider ;
                var collider = colliders.First();

                if (collider != null)
                {
                    var otherUserId = collider.gameObject.GetPhotonView().Owner.ActorNumber;
                    if (AllowedToAttackTarget(myUserId, otherUserId))
                    {
                        Debug.LogFormat("Attacking {0}", otherUserId);
                        photonView.RPC("PerformAttack", RpcTarget.All, myUserId.ToString(), otherUserId.ToString());
                    }
                        
                }
            }
        }

        bool AllowedToAttackTarget(int myUserId, int targetUserId)
        {
            return gameManager.roundSystem.targets[myUserId.ToString()] == targetUserId.ToString();
        }

        // Start is called before the first frame update
        void Start()
        {
            IsDead = false;
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameManager.OnGameStateEnded += (string winnerId) =>
            {
                IsDead = false;
                gameManager.UpdateLivePlayers();
            };
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


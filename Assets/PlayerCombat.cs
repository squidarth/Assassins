using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
                    gameManager.handleDeath(attackerId, attackedId);
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
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), attackRange, 1 << LayerMask.NameToLayer("Player"));
                
                foreach(var collider in colliders)
                {
                    var otherUserId = collider.gameObject.GetPhotonView().Owner.ActorNumber;
                    if (otherUserId != myUserId)
                    {
                        Debug.LogFormat("Attacking {0}", otherUserId);
                        photonView.RPC("PerformAttack", RpcTarget.All, myUserId.ToString(), otherUserId.ToString());
                    }
                        
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            IsDead = false;
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


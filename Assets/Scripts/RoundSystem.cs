using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

namespace Com.Assassins
{
public class RoundSystem : MonoBehaviourPunCallbacks
{

    public bool gameInProgress;
    public GameManager gameManager;
    public Dictionary<string, string> targets;
    public Dictionary<string, string> reverseTargets;

    [PunRPC]
    public void Death(string attackerId, string attackedId)
    {
        Debug.Log("Death RPC hit");
        /* In this case, we've reached the end of the game */
        if (targets[attackedId] == attackerId)
        {
            // end of the game logic
            gameInProgress = false;
            gameManager.GameComplete(attackerId);
        } else
        {
            var newTarget = targets[attackedId];
            targets.Remove(attackedId);
            reverseTargets.Remove(attackedId);
            targets[attackerId] = targets[attackedId];
            reverseTargets[targets[attackedId]] = attackerId;
        }

    }

    [PunRPC]
    public void StartRound(string[] userIds, int pointlessInt)
    {
        targets = new Dictionary<string, string>();
        reverseTargets = new Dictionary<string, string>();

        Debug.Log(userIds);
        for (int i = 0;i<userIds.Length;i++)
        {
            Debug.Log(userIds[i]);
            Debug.Log(userIds[i]);
            Debug.Log(userIds[(i + 1) % userIds.Length]);
            targets[userIds[i]] = userIds[(i + 1) % userIds.Length];
        }

        for (int i = userIds.Length - 1;i >= 0;i--)
        {
            reverseTargets[userIds[i]] = userIds[(i - 1 + userIds.Length) % userIds.Length];
        }
        gameInProgress = true;
        gameManager.RoundStarted();
    }

    [PunRPC]
    public void EndRound()
    {
        gameInProgress = false;
    }
    // Start is called before the first frame update
    void Awake()
    {
        gameInProgress = false;

    }

    private void Start()
    {
            if (gameManager == null)
            {
                gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.roundSystem = this;
            }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}

}

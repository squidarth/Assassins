using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Assassins
{
    public class CharacterCollisionBlocker : MonoBehaviour
    {
        public CapsuleCollider2D characterCollider;
        public CapsuleCollider2D characterBlockerCollider;

        // Start is called before the first frame update
        void Start()
        {
            Physics2D.IgnoreCollision(characterCollider, characterBlockerCollider, true);
            
        }


    }
}


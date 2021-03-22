using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Assassins
{

    public class CameraWork : MonoBehaviour
    {
        Transform target;
        public float smoothing;
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public bool followOnStart;
        bool isFollowing;

        public void OnStartFollowing()
        {
            target = Camera.main.transform;
            isFollowing = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (followOnStart)
            {
                target = Camera.main.transform;
                isFollowing = true;
                
            }
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (target != null && transform.position != target.position && isFollowing) {
                Debug.Log("Attempting to do a camera update");
                Debug.Log(target);
                Vector3 transformPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                transformPosition.x = Mathf.Clamp(transformPosition.x, minPosition.x, maxPosition.x);

                transformPosition.y = Mathf.Clamp(transformPosition.y, minPosition.y, maxPosition.y);
                transformPosition.z = -10;

                target.position = Vector3.Lerp(target.position, transformPosition, smoothing);
            }
            
        }
    }

}

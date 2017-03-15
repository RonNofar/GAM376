using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace R
{
    public class FollowObject : MonoBehaviour
    {

        [SerializeField]
        private GameObject followerObject;
        [SerializeField]
        private GameObject toFollowObject;

        [SerializeField]
        private float maxDistance = 10f;
        [SerializeField]
        private float minSpeed = 0.01f;
        [SerializeField]
        private bool enableMaxSpeed = false;
        [SerializeField]
        private float maxSpeed = 2f;


        private Transform follower;
        private Transform toFollow;

        private void Awake()
        {
            if (followerObject != null) follower = followerObject.GetComponent<Transform>();
            else Debug.Log("followerObject is null");
            if (toFollowObject != null) toFollow = toFollowObject.GetComponent<Transform>();
            else Debug.Log("toFollowObject is null");
        }

        private void FixedUpdate()
        {
            FixPosition();
        }

        private void FixPosition()
        {
            float[,] pos = { { follower.position.x, follower.position.y }, { toFollow.position.x, toFollow.position.y } };
            Vector2 vForce = new Vector2();

            for (int i = 0; i<2; ++i) // 2 for number of dimensions (x, y) || if able to get width of pos matrix this would be applyable in any # of dimensions
            {
                if (pos[0, i] != pos[1, i])
                {
                    float dist = pos[1, i] - pos[0, i];
                    float force = (1 / (maxDistance / Mathf.Abs(dist)) + minSpeed);// * Time.deltaTime; // uses 1/(ratio of how near) + min speed
                    if (enableMaxSpeed) if(force > maxSpeed) force = maxSpeed;
                    if (pos[0, i] > pos[1, i]) force *= -1;
                    vForce = new Vector2(
                        Convert.ToBoolean(i) ? 0f : force, 
                        Convert.ToBoolean(i) ? force : 0f
                    );
                }
                follower.Translate(vForce);
            }                    
        }
    }
}

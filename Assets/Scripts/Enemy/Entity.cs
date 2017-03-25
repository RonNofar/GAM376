using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split
{
    public abstract class Entity : MonoBehaviour
    {
        //temp usage to maintain functionality
        protected Rigidbody2D myRigidbody;

        [SerializeField]
        private float minimumHeight = -100f;

        // Use this for initialization
        protected virtual void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (transform.position.y < minimumHeight)
            {
                Destroy(gameObject);
            }

        }


    }
}

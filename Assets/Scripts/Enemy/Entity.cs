using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split
{
    public abstract class Entity : PixelPerfect
    {
        //temp usage to maintain functionality
        protected Rigidbody2D myRigidbody;

        private Vector2 velocity;

        [SerializeField]
        private float minimumHeight = -100f;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            myRigidbody = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (transform.position.y < minimumHeight)
            {
                Destroy(gameObject);
            }

        }

        protected virtual void FixedUpdate()
        {
            transform.position +=(Vector3)(
            Time.fixedDeltaTime*velocity);
            

        }

        public void addForce(float force)
        {

        }

    }
}

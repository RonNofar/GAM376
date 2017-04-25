using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.UI;

namespace KRaB.Split.Enemy
{
    public class Slime : Enemy, ColoredObject
    {
        [SerializeField]
        private KRaB.Enemy.Colors.EnemyColor colorData;
        public KRaB.Enemy.Colors.EnemyColor ColorData
        {
            get { return colorData; }
            set
            {
                colorData = value;
                mySpriteRenderer.color = colorData.color;
            }
        }
        
        private float damageTime = 0f;

        [Header("Movement")]
        [SerializeField]
        private Util.RTool.FloatRange jumpDelay;
        [SerializeField]
        private Util.RTool.FloatRange jumpForce;
        [SerializeField]
        private float minimumVerticleJumpForce = 10f;
        [SerializeField]
        private float xDampner = 2f;
        [SerializeField]
        private float yDampner = 1f;
        [SerializeField]
        private float maxDistance = 10f;

        [SerializeField]
        private SpriteRenderer mySpriteRenderer;

        private float delayTime = 0f;
        private bool isBounce = false;
        private AudioSource audio;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            ColorData = colorData;
            audio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            //Debug.Log("Slime");

            if (!isBounce)
            {
                Bounce();
                delayTime = Time.time + jumpDelay.RandomInRange;
            }
            else
            {
                if (Time.time > delayTime)
                {
                    isBounce = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Damage(collision);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            Damage(collision);
        }

        protected void Damage(Collision2D collision)
        {
            if (!tossed && Time.time > damageTime)
            {
                if (collision.gameObject.GetComponent<Transform>().tag == "Player")
                {
                    if (Parent != null)
                    {
                        player.DamagePlayer(Parent.Damage);
                        damageTime = Time.time + Parent.DamageDelay;
                    }
                }
            }

        }


        private void Bounce()
        {
            if (tossed)
                return;
            Vector2 move = new Vector2();
            if (Zone)
                move = Zone.movement;


            Vector2 bias=new Vector2();
            if (Vector3.Distance(playerTransform.position, GetComponent<Transform>().position) < (Parent?Parent.SearchDistance:maxDistance))
            {
                bias = new Vector2(
                            (playerTransform.position.x - transform.position.x) / xDampner * jumpForce.RandomInRange,
                            (playerTransform.position.y - transform.position.y) / yDampner * jumpForce.RandomInRange + minimumVerticleJumpForce
                    );
                audio.Play();
            }
            if(Parent)
                move = Parent.clampJump(move+bias);
            myRigidbody.AddForce(move,
                ForceMode2D.Impulse
            );
            //Debug.Log("Bounce");
            isBounce = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class Pusher : MonoBehaviour
    {
        [SerializeField]
        private bool upward = true;
        [SerializeField]
        private float force = 5f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(
                    new Vector2(
                        0f,
                        force * Time.deltaTime * (upward ? 1 : -1) + -Physics2D.gravity.y
                    )
                );
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Shovel
{
    public class ShovelHandler : MonoBehaviour {

        [SerializeField]
        private Player.PlayerControl player;

        public static bool isCollision = false;

        private List<GameObject> objs = new List<GameObject>();

        // Use this for initialization
        void Start() {
            //objs.Add(gameObject);
        }

        // Update is called once per frame
        void Update() {

        }
        /*
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<Transform>().tag == "Enemy")
            {
                if (player.GetIsShovel())
                {
                    player.ApplyShovelCurve(col.gameObject);
                }
            }
            isCollision = true;
        }
        */
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<Transform>().tag == "Enemy")
            {
                if (player.GetIsShovel())
                {
                    if (!objs.Contains(col.gameObject))
                    {
                        player.ApplyShovelCurve(col.gameObject);
                        objs.Add(col.gameObject);
                    }
                }
            }
            isCollision = true;
        }
    }
}

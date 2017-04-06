using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class Checkpoint : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject o = collision.gameObject;
            if (o.tag == "Player")
            {
                o.GetComponent<Player.PlayerControl>().SetSpawn(
                    GetComponent<Transform>().position);
            }
        }
    }
}

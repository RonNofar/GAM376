using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class DeathZone : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                Player.PlayerControl.Instance.Death();
            }
        }
    }
}

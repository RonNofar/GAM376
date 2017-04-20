using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Player
{
    public class HealthDrop : MonoBehaviour
    {
        [SerializeField]
        private float healAmount = 5f;

        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.GetComponent<Transform>().tag == "Player")
            {
                Player.PlayerControl.Instance.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}

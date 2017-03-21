using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.UI;
using R;

namespace KRaB.Split.Enemy
{
    public class enemyScript : MonoBehaviour
    {
        [SerializeField]
        private ColorManager.eColors colorType;
        [SerializeField]
        private float damage = 10f;

        public PlayerControl player;

        private Color color;

        private void Awake()
        {
            InitializeEnemy();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D col) // trigger might not work with this
        {
            //Debug.Log("Collision: "+col.transform.name+" | Tag: "+ col.transform.tag);
            Transform colTransform = col.gameObject.GetComponent<Transform>();
            if (colTransform.gameObject.tag == "slash")
            {
                // put thing here
                Debug.Log("In there fam");
                Destroy(gameObject);
            }
            else if (colTransform.gameObject.tag == "Player")
            {
                player.DamagePlayer(damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Transform colTransform = col.gameObject.GetComponent<Transform>();
            Player.SlashHandler colObj = colTransform.GetComponent<Player.SlashHandler>();
            if (colObj.tag != null)
            {
                if (colorType == colObj.Color)
                {
                    // put thing here
                    Destroy(gameObject);
                }
            }
        }

        private void InitializeEnemy()
        {
            GetComponent<SpriteRenderer>().color = ColorManager.GetColor(colorType);
            
        }
    }
}

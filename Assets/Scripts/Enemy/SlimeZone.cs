using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KRaB.Split.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class SlimeZone : MonoBehaviour
    {

        public Vector2 movement
        {
            get { return new Vector2(Horizontal.RandomInRange,Vertical.RandomInRange); }
        }

        [SerializeField]
        private Util.FloatRange Horizontal;
        [SerializeField]
        private Util.FloatRange Vertical;



        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Enter");
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if(e!=null)
                e.Zone = this;
            //Zone = collision.gameObject.GetComponent<SlimeZone>();

        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("Leave");
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (e == null) return;
            if(e.Zone == this)
                e.Zone = null;
            //Zone = null;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

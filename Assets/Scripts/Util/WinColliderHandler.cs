using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class WinColliderHandler : MonoBehaviour {

        [SerializeField]
        private Manager.GameMaster gameMaster;
    
    // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.transform.tag == "Player")
            {
                gameMaster.Win();
            }
        }
    }
}

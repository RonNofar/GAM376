using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class WinColliderHandler : MonoBehaviour {

        [SerializeField]
        private Manager.GameMaster gameMaster;

        private AudioSource audio;
    
    // Use this for initialization
        void Start() {
            audio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.transform.tag == "Player")
            {
                gameMaster.Win();
                audio.Play();
            }
        }
    }
}

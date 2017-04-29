using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class ButtonHandler : MonoBehaviour
    {

        [SerializeField]
        private ButtonDoorHandler correspondingDoor;
        [SerializeField]
        private GameObject anchor;
        [SerializeField]
        private float triggeredYScale = 0.1f;

        private bool isTriggered = false;
        private new AudioSource audio;

        private void Start()
        {
            GetComponent<SpriteRenderer>().color = correspondingDoor.currColor;
            audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Trigger");
            if (!isTriggered)
            {
                if (other.gameObject.tag == "Player")
                {
                    TriggerButton();
                }
            }
        }

        private void TriggerButton()
        {
            audio.Play();
            correspondingDoor.TriggerDoor();
            isTriggered = true;
            Vector3 scale = new Vector3(
                anchor.GetComponent<Transform>().localScale.x,
                anchor.GetComponent<Transform>().localScale.y * triggeredYScale,
                anchor.GetComponent<Transform>().localScale.z);
            anchor.GetComponent<Transform>().localScale = scale;
        }
    }
}

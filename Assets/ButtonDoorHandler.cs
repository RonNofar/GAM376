using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class ButtonDoorHandler : MonoBehaviour
    {

        [SerializeField]
        private Transform doneTransform;
        [SerializeField]
        private float totalTime = 1f;

        private Transform myTransform;
        private Vector3 originalPosition;
        private Vector3 donePosition;
        public bool isTriggered = false;
        private bool isOpening = false;

        private float startTime = 0f;
        private float timeRatio = 0f;

        public Color currColor
        {
            get { return GetComponent<SpriteRenderer>().color; }
            set { GetComponent<SpriteRenderer>().color = value; }
        }
        // Use this for initialization
        void Start()
        {
            myTransform = GetComponent<Transform>();
            originalPosition = myTransform.position;
            donePosition = doneTransform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTriggered)
            {
                startTime = Time.time;
                isOpening = true;
                isTriggered = false;
            }
            if (isOpening)
            {
                // Open
                Debug.Log("Time.time: " + Time.time + " | startTime: " + startTime + " | timeRatio: " + timeRatio);
                timeRatio = (Time.time - startTime) / totalTime;
                Debug.Log(timeRatio);

                timeRatio = (timeRatio > 1) ? 1f : timeRatio;
                Debug.Log(timeRatio);
                myTransform.position = Vector3.Lerp(originalPosition, donePosition, timeRatio);
                if (timeRatio == 1) isOpening = false;
            }
        }

        public void TriggerDoor()
        {
            isTriggered = true;
        }


    }
}

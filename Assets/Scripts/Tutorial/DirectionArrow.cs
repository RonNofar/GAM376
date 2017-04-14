using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class DirectionArrow : Tutorial
    {
        [SerializeField]
        private float totalTime = 0.5f;

        [SerializeField]
        private Transform doneTransform;

        private Transform myTransform;
        private Vector3 originalPosition;
        private Vector3 donePosition;
        private float distance;

        private bool isTriggered = false;
        private bool isOn = false;

        private float startTime = 0f;
        private float timeRatio = 0f;



        protected override void Awake()
        {
            base.Awake();

            myTransform = GetComponent<Transform>();
            originalPosition = myTransform.position;
            donePosition = doneTransform.position;
            distance = Vector3.Distance(originalPosition, donePosition);
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            if (!isOn) isTriggered = true;
            if (isTriggered)
            {
                startTime = Time.time;

                isTriggered = false;
                isOn = true;
            }
            if (isOn)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                timeRatio = (timeRatio > 1) ? 1 : timeRatio;
                // SinLerp for Vector3 here

                //Debug.Log(startTime + " | " + Time.time + " | " + Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distance / 2));

                myTransform.position = new Vector3(
                    originalPosition.x,
                    originalPosition.y + Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distance / 2),
                    originalPosition.z
                );
                // To make this into a multi axis function, simply caluclate the distance of each axis then use that individually when assigning position

                if (timeRatio == 1)
                {
                    isOn = false;
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            //isOn = false;
        }

        void FixedUpdate()
        {
        
        }

        void ApplyAnimation()
        {
            if (isTriggered)
            {
                startTime = Time.time;

                isTriggered = false;
                isOn = true;
            }
            if (isOn)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                timeRatio = (timeRatio > 1) ? 1 : timeRatio;
                // SinLerp for Vector3 here

                Debug.Log(startTime + " | " + Time.time + " | " + Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distance / 2));

                myTransform.position = new Vector3(
                    originalPosition.x,
                    originalPosition.y + Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distance / 2),
                    originalPosition.z
                );

                if (timeRatio == 1)
                {
                    isOn = false;
                }
            }
        }
    }
}

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
        private Vector3 originalRotation;
        private Vector3 donePosition;
        private Vector3 doneRotation;
        private float[] distanceXYZ = new float[3];
        private float[] angleXYZ = new float[3];

        private bool isTriggered = false;
        private bool isOn = false;

        private float startTime = 0f;
        private float timeRatio = 0f;



        protected override void Awake()
        {
            base.Awake();

            myTransform = GetComponent<Transform>();
            originalPosition = myTransform.position;
            originalRotation = myTransform.localEulerAngles;
            donePosition = doneTransform.position;
            doneRotation = doneTransform.localEulerAngles;
            Debug.Log(doneRotation);
            distanceXYZ[0] = Mathf.Abs(originalPosition.x - donePosition.x);
            distanceXYZ[1] = Mathf.Abs(originalPosition.y - donePosition.y);
            distanceXYZ[2] = Mathf.Abs(originalPosition.z - donePosition.z);
            angleXYZ[0]    = Mathf.Abs(originalRotation.x - doneRotation.x);
            angleXYZ[1]    = Mathf.Abs(originalRotation.y - doneRotation.y);
            angleXYZ[2]    = Mathf.Abs(originalRotation.z - doneRotation.z);
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            if (!isOn) isTriggered = true;
            ApplyAnimation();
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

                Debug.Log(startTime + " | " + Time.time + " | " + Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distanceXYZ[1] / 2));

                myTransform.position = new Vector3(
                    originalPosition.x + ((distanceXYZ[0] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distanceXYZ[0] / 2) : 0f),
                    originalPosition.y + ((distanceXYZ[1] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distanceXYZ[1] / 2) : 0f),
                    originalPosition.z + ((distanceXYZ[2] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (distanceXYZ[2] / 2) : 0f)
                );

                myTransform.localEulerAngles = new Vector3(
                    originalRotation.x + ((angleXYZ[0] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (angleXYZ[0] / 2) : 0f),
                    originalRotation.y + ((angleXYZ[1] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (angleXYZ[1] / 2) : 0f),
                    originalRotation.z + ((angleXYZ[2] != 0) ? Mathf.Sin(timeRatio * (2 * Mathf.PI)) * (angleXYZ[2] / 2) : 0f)
                );

                if (timeRatio == 1)
                {
                    isOn = false;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class DirectionArrow : Tutorial
    {
        [SerializeField]
        private float totalTime = 0.5f;

        private bool isTriggered = false;
        private bool isOn = false;

        private float startTime = 0f;
        private float timeRatio = 0f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            isTriggered = true;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            isOn = false;
        }

        void FixedUpdate()
        {
            if (isTriggered)
            {
                startTime = Time.time;

                isTriggered = false;
            }
            if (isOn)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                // SinLerp for Vector3 here
            }
        }
    }
}

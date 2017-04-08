using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class ShovelTutorial : Tutorial
    {
        [Header("To Unlock")]
        [SerializeField]
        private GameObject unlock;
        [SerializeField]
        private float totalTime = 1f;
        [SerializeField]
        private Transform doneTransform;

        private Vector3 originalPosition;
        private Vector3 donePosition;

        private bool isUnlocked = false;

        protected override void Awake()
        {
            base.Awake();

            originalPosition = unlock.GetComponent<Transform>().position;
            donePosition = doneTransform.GetComponent<Transform>().position;
        }

        protected override void OnTriggerStay2D (Collider2D collision)
        {
            base.OnTriggerStay2D(collision);
            if (GameObject.Find("slime(blue)") == null)
            {
                if (!isUnlocked)
                {
                    StartCoroutine(Unlock());
                    isUnlocked = true;
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
        }

        IEnumerator Unlock()
        {
            Transform transform = unlock.GetComponent<Transform>();
            float startTime = Time.time;
            float timeRatio = 0f;
            while (timeRatio < 1f)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                timeRatio = (timeRatio > 1) ? 1 : timeRatio;

                transform.position = Vector3.Lerp(originalPosition, donePosition, timeRatio);

                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class SwitchTutorial : Tutorial
    {
        [Header("To Unlock")]
        [SerializeField]
        private GameObject unlockOne;
        [SerializeField]
        private GameObject unlockTwo;
        [SerializeField]
        private float totalTime = 1f;
        [SerializeField]
        private Transform doneTransformOne;
        [SerializeField]
        private Transform doneTransformTwo;

        private Vector3 originalPositionOne;
        private Vector3 donePositionOne;

        private Vector3 originalPositionTwo;
        private Vector3 donePositionTwo;

        private bool isUnlockedOne = false;
        private bool isUnlockedTwo = false;

        protected override void Awake()
        {
            base.Awake();

            originalPositionOne = unlockOne.GetComponent<Transform>().position;
            donePositionOne = doneTransformOne.GetComponent<Transform>().position;
            originalPositionTwo = unlockTwo.GetComponent<Transform>().position;
            donePositionTwo = doneTransformTwo.GetComponent<Transform>().position;
        }

        protected override void OnTriggerStay2D (Collider2D collision)
        {
            base.OnTriggerStay2D(collision);
            if (GameObject.Find("slime(red)") == null && !isUnlockedOne)
            {
                StartCoroutine(Unlock(unlockOne, originalPositionOne, donePositionOne));
                isUnlockedOne = true;
            }
            if (GameObject.Find("slime(yellow)") == null && !isUnlockedTwo)
            {
                StartCoroutine(Unlock(unlockTwo, originalPositionTwo, donePositionTwo));
                isUnlockedTwo = true;
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Unlock");
            base.OnTriggerExit2D(collision);
        }

        IEnumerator Unlock(GameObject unlock, Vector3 originalPosition, Vector3 donePosition)
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

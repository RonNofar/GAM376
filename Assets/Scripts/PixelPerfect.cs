using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB
{
    public class PixelPerfect : MonoBehaviour
    {
        [Header("Pixel Perfect", order = -9001)]
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        protected Vector3 position;

        [SerializeField]
        bool self = false;

        new private Transform transform;

        // Use this for initialization
        protected virtual void Start()
        {
            transform = GetComponent<Transform>();
            if (!self)
            {
                position = transform.position;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        }
        protected virtual void LateUpdate()
        {
            if (!self)
            {
                position = transform.position;
            }
            Vector3 offsetPosition = position + offset +PixelPerfecrManager.Instance.GlobalOffset;
            float pixelHeight = PixelPerfecrManager.Instance.PixelHeight;
            float pixelWidth = PixelPerfecrManager.Instance.PixelWidth;
            float x = offsetPosition.x % pixelWidth;
            float y = offsetPosition.y % pixelHeight;
            transform.position = new Vector3(offsetPosition.x - x + (x < pixelWidth / 2 ? 0 : pixelWidth),
                offsetPosition.y - y + (y < pixelHeight / 2 ? 0 : pixelHeight),
                offsetPosition.z);
            StartCoroutine(returnPosition());
        }
        private IEnumerator returnPosition()
        {
            yield return new WaitForEndOfFrame();
            if (!self)
            {
                transform.position = position;
            }

        }
    }
}

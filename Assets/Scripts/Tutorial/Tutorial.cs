using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [Header("CanvasRender Components")]
        [SerializeField]
        private CanvasRenderer[] toRender;

        protected virtual void Awake()
        {
            SetCanvasRenderArrayAlpha(0f, toRender);
            toRender = GetComponentsInChildren<CanvasRenderer>(true);
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                SetCanvasRenderArrayAlpha(1f, toRender);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            SetCanvasRenderArrayAlpha(0f, toRender);
        }

        private void SetCanvasRenderArrayAlpha(float a, CanvasRenderer[] arr)
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                toRender[i].SetAlpha(a);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB
{
    public class PixelPerfectCamera : PixelPerfect
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<Camera>().orthographicSize = Screen.height / 2/PixelPerfecrManager.Instance.PixelsPerUnit/PixelPerfecrManager.Instance.PixelsPerPixel;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB
{
    [CreateAssetMenu]
    public class PixelPerfecrManager : ScriptableObject
    {

        public static PixelPerfecrManager Instance { get; private set; }

        [SerializeField]
        private float pixelWidth = 0.025f;
        [SerializeField]
        private float pixelHeight = 0.025f;
        [SerializeField]
        private Vector3 globalOffset;

        [SerializeField]
        private float pixelsPerUnit;
        [SerializeField]
        private int pixelsPerPixel;

        public float PixelWidth { get { return pixelWidth > 0 ? pixelWidth : 1; } }
        public float PixelHeight { get { return pixelHeight > 0 ? pixelHeight : 1; } }
        public Vector3 GlobalOffset { get { return globalOffset; } }
        public float PixelsPerUnit { get { return pixelsPerUnit > 0 ? pixelsPerUnit : 1; } }
        public int PixelsPerPixel { get { return pixelsPerPixel > 0 ? pixelsPerPixel : 1; } }


        // Use this for initialization
        void Start()
        {
            if (Instance)
                return;
            Instance = this;
            pixelWidth = (pixelWidth > 0) ? pixelWidth : 1;
            pixelHeight = (pixelHeight > 0) ? pixelHeight : 1;

        }
    }
}

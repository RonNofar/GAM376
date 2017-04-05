using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB
{

    public class PixelPerfecrManager : MonoBehaviour
    {

        public static PixelPerfecrManager Instance { get; private set; }

        [SerializeField]
        private float pixelWidth = 0.025f;
        [SerializeField]
        private float pixelHeight = 0.025f;
        [SerializeField]
        private Vector3 globalOffset;

        public float PixelWidth { get { return pixelWidth; } }
        public float PixelHeight { get { return pixelHeight; } }
        public Vector3 GlobalOffset { get { return globalOffset; } }


        // Use this for initialization
        void Start()
        {
            if (Instance)
                return;
            Instance = this;
            pixelWidth = (pixelWidth > 0) ? pixelWidth : 1;
            pixelHeight = (pixelHeight > 0) ? pixelHeight : 1;

        }

        // Update is called once per frame
        void Update()
        {

            pixelWidth = (pixelWidth > 0) ? pixelWidth : 0.0001f;
            pixelHeight = (pixelHeight > 0) ? pixelHeight : 0.0001f;
        }
    }
}

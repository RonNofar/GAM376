using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Environment{
    public class BackgroundParallax : MonoBehaviour
    {

        public Transform camPosition;

        public Vector3 offset;


        public float xRatio;
        public float yRatio;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 cPos = camPosition.position;
            cPos += offset;
            Vector3 oPos = new Vector3();

            oPos.x = cPos.x * xRatio;
            oPos.y = cPos.y * yRatio;
            transform.position = oPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R
{

    public class RTool : MonoBehaviour
    {

        [System.Serializable]
        public struct FloatRange
        {
            public float min, max;

            public float RandomInRange
            {
                get
                {
                    return Random.Range(min, max);
                }
            }
        }
    }

}


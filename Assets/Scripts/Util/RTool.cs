﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KRaB.Split.Util
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

        public static IEnumerator WaitAndRunAction(float secs, UnityAction action)
        { // wait for secs  
            yield return new WaitForSeconds(secs);
            action.Invoke();
        }
    }

}


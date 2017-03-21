using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.Util;

namespace KRaB.Split.UI
{
    public class ParticleGenerator : MonoBehaviour
    {
        [Header("Particles")]
        [SerializeField]
        private GameObject[] particles;

        [Header("Values")]
        [SerializeField]
        private RTool.FloatRange spawnDelayRange;
        [SerializeField]
        private RTool.FloatRange sizeRange;
        [SerializeField]
        private RTool.FloatRange angleRange;
        


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

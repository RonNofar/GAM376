using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KRaB.Split.Enemy
{
    public class AIMaster : MonoBehaviour
    {
        public Util.FloatRange JumpHeight;
        public Util.FloatRange JumpWidth;

        [SerializeField]
        private GameObject SpawnerPrefab;


        // Use this for initialization
        void Start()
        {
            SlimeSpawner[] children = GetComponentsInChildren<SlimeSpawner>();
            foreach(SlimeSpawner c in children)
            {
                c.Parent = this;
            }
            Transform[] child = GetComponentsInChildren<Transform>();
            Debug.Log(child.Length);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

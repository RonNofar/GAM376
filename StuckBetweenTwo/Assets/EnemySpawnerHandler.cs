using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R
{
    public class EnemySpawnerHandler : MonoBehaviour {

        [Header("Variables")]
        [SerializeField]
        private RTool.FloatRange enemySpawnDelay;
        [SerializeField]
        private GameObject[] enemyList;
        [SerializeField]
        private ColorManager.eColors colorType;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}

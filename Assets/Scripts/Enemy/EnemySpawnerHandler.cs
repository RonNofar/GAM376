using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.Util;
using KRaB.Split.UI;

namespace KRaB.Split.Enemy
{
    public class EnemySpawnerHandler : MonoBehaviour {

        [Header("Variables")]
        [SerializeField]
        private RTool.FloatRange enemySpawnDelay;
        [SerializeField]
        private GameObject[] enemyList;
        [SerializeField]
        private ColorManager.eColors colorType;

        private bool isStart = false;
        private float startTime = 0f;

        // Use this for initialization
        void Start() {

        }

        void FixedUpdate() {
            if (!isStart)
            {
                startTime = Time.time;
                
            }
        }

        public void SpawnEnemy()
        {

        }
    }
}

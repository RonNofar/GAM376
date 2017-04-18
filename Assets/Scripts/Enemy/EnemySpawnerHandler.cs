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
        private GameObject enemy;
        [SerializeField]
        private bool isOneColor = false;

        [SerializeField]
        private KRaB.Enemy.Color.EnemyColor[] possibleSpawns;

        private bool isStart = false;
        private float startTime = 0f;

        private bool spawning;

        // Use this for initialization
        void Start() {
            Time.timeScale = 1;
            spawning = true;
            StartCoroutine(spawnEnemies());
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
        private IEnumerator spawnEnemies()
        {
            Debug.Log("Spawn start");
            while (spawning)
            {
                KRaB.Enemy.Color.EnemyColor m = possibleSpawns[Random.Range(0, possibleSpawns.Length)];
                GameObject temp = Instantiate(enemy);
                temp.GetComponent<Slime>().ColorData = m;
                temp.transform.position = transform.position;
                yield return new WaitForSeconds(enemySpawnDelay.RandomInRange);

            }
        }
    }
}

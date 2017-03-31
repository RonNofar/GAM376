using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.Util;
using KRaB.Split.UI;

namespace KRaB.Split.Enemy
{
    public class SlimeSpawner : MonoBehaviour
    {

        [Header("Variables")]
        [SerializeField]
        private RTool.FloatRange enemySpawnDelay;
        [SerializeField]
        private GameObject enemy;
        [SerializeField]
        private ColorManager.eColors colorType;
        [SerializeField]
        private RTool.FloatRange colorRange;

        [Header("Runtime Linking", order = 101)]
        [SerializeField]
        private AIMaster parent;

        public AIMaster Parent
        {
            set
            {
                if (parent == null)
                    parent = value;
                StartCoroutine(spawnEnemies(value));
            }
        }

        private bool isStart = false;
        private float startTime = 0f;

        private bool spawning;

        // Use this for initialization
        void Start()
        {
            Time.timeScale = 1;
            spawning = true;
            if (parent)
                StartCoroutine(spawnEnemies(parent));
        }

        void FixedUpdate()
        {
            if (!isStart)
            {
                startTime = Time.time;

            }
        }

        public void SpawnEnemy()
        {

        }
        private IEnumerator spawnEnemies(AIMaster parent)
        {
            Debug.Log("Spawn start");
            while (spawning && parent == this.parent)
            {
                if (parent.spawn)
                {
                    Debug.Log("Spawn Continues");
                    enemy.GetComponent<Slime>().Color = (UI.ColorManager.eColors)(1 << (Random.Range((int)colorRange.min, (int)colorRange.max)));
                    GameObject temp = Instantiate(enemy);
                    temp.GetComponent<Slime>().Parent = parent;
                    temp.transform.position = transform.position;
                }
                yield return new WaitForSeconds(enemySpawnDelay.RandomInRange);

            }
        }
    }
}

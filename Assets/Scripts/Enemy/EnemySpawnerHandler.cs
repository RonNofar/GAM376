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

        // Use this for initialization
        void Start() {

        }

        void FixedUpdate() {

        }
    }
}

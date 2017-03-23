using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Manager
{
    public class GameMaster : MonoBehaviour {

        public GameObject toSpawn;
        public int numberOfSpawns;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                for (int i = 0; i < numberOfSpawns; ++i)
                {
                    GameObject o = Instantiate(toSpawn);
                    o.GetComponent<Enemy.SlimeHandler>().Color =
                            (Random.Range(0f, 1f) > 0.5f) ? UI.ColorManager.eColors.Blue : UI.ColorManager.eColors.Red;
                }
            }
        }
    }
}

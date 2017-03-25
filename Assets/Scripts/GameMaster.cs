using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KRaB.Split.Manager
{
    public class GameMaster : MonoBehaviour {

        public GameObject toSpawn;
        public int numberOfSpawns;

        [Header("Version")]
        [SerializeField]
        private string versionString = "v0.0.0";
        [SerializeField]
        private Text versionText;

        [Header("Win")]
        [SerializeField]
        private BoxCollider2D winCollider;
        [SerializeField]
        private Text winText;

        private Player.PlayerControl player;

        private string prevVersionString = " "; // used for switching UI if changed in inspector

        // Use this for initialization
        void Start() {
            winText.enabled = false;
            player = GameObject.FindWithTag("Player").GetComponent<Player.PlayerControl>();
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
            if (prevVersionString != versionString)
            {
                prevVersionString = versionString;
                versionText.text = versionString;
            }
        }
        
        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        public void Win()
        {
            winText.enabled = true;
            StartCoroutine(Util.RTool.WaitAndRunAction(3f, () => ReloadScene()));
        }
    }
}

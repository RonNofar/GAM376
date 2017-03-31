using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KRaB.Split.Manager
{
    public class GameMaster : MonoBehaviour {

        static public GameMaster Instance { get { return _instance; } }
        static public GameMaster _instance;

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

        private bool isPaused = false;
        public bool pause
        {
            get { return isPaused; }
            set
            {
                isPaused = value;
                if (value == true) Pause();
                else if (value == false) UnPause();
            }
        }

        public enum GameState
        {
            none,
            MainMenu,
            InGame,
            Paused,
            GameOver
        }

        private GameState currentState;
        private GameState lastState;
        public GameState savedState = GameState.none;
        public GameState gameState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Debug.Log("OnAwake: " + lastState + " | Time: "+Time.realtimeSinceStartup);
            if (_instance != null)
            {
                Debug.LogWarning("Game Master is already in play, deleting new.");
                Destroy(this);
            }
            else
            { _instance = this; }
        }

        // Use this for initialization
        void Start() {
            Debug.Log(savedState);
            if (savedState != GameState.none)
            {
                gameState = savedState; // << TO BE TAKEN OUT <<<
            }
            else gameState = GameState.MainMenu;
            winText.enabled = false;
            player = GameObject.FindWithTag("Player").GetComponent<Player.PlayerControl>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                for (int i = 0; i < numberOfSpawns; ++i)
                {
                    GameObject o = Instantiate(toSpawn);
                    o.GetComponent<Enemy.Slime>().Color =
                            (Random.Range(0f, 1f) > 0.5f) ? UI.ColorManager.eColors.Blue : UI.ColorManager.eColors.Red;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                if (gameState == GameState.InGame)
                {
                    gameState = GameState.Paused;
                }
                else if (gameState == GameState.Paused)
                {
                    gameState = GameState.InGame;
                }
            }
            if (prevVersionString != versionString)
            {
                prevVersionString = versionString;
                versionText.text = versionString;
            }

            // Handle Change in GameState here
            if (lastState != currentState)
            {
                Debug.Log("state change: " + currentState);
                lastState = currentState;
                switch (currentState)
                {
                    case GameState.MainMenu:
                        Pause();
                        break;
                    case GameState.Paused:
                        Pause();
                        break;
                    case GameState.InGame:
                        UnPause();
                        break;
                }
            } // If you see this^ brian, try to combine GUIManager with this cause it is practically the same
        }
        
        public static void ReloadScene()
        {
            Debug.Log("Before reload: " + Instance.lastState);
            SceneManager.LoadScene(0);
        }

        public void Win()
        {
            winText.enabled = true;
            StartCoroutine(Util.RTool.WaitAndRunAction(3f, () => ReloadScene()));
        }

        public static void Pause()
        {
            Time.timeScale = 0.0f;
        }

        public static void UnPause()
        {
            Time.timeScale = 1.0f;
        }

        public static void Exit()
        {
            Application.Quit();
        } 
    }
}

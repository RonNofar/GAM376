using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Manager
{
    public class GUIManager : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField]
        private GameObject mainMenuCanvas;
        [SerializeField]
        private GameObject pauseMenuCanvas;
        [SerializeField]
        private GameObject inGameCanvas;

        private GameMaster.GameState lastState = 
            GameMaster.GameState.none;

        // Use this for initialization
        void Start()
        {
            mainMenuCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (lastState != GameMaster.Instance.gameState)
            {
                lastState = GameMaster.Instance.gameState;
                switch (GameMaster.Instance.gameState)
                {
                    case GameMaster.GameState.none:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                mainMenuCanvas,
                                pauseMenuCanvas,
                                inGameCanvas
                            });
                        break;
                    case GameMaster.GameState.MainMenu:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                pauseMenuCanvas,
                                inGameCanvas
                            });
                        mainMenuCanvas.SetActive(true);
                        break;
                    case GameMaster.GameState.Paused:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                mainMenuCanvas,
                                inGameCanvas
                            });
                        pauseMenuCanvas.SetActive(true);
                        break;
                    case GameMaster.GameState.InGame:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                mainMenuCanvas,
                                pauseMenuCanvas
                            });
                        inGameCanvas.SetActive(true);
                        break;
                }
            }
        }

        public static void SetArrayToActiveState(bool state, GameObject[] objectArr)
        { // Sets a whole game object array to true or false
            for (int i = 0; i < objectArr.Length; i++)
            {
                objectArr[i].SetActive(state);
            }
        }
    }
}

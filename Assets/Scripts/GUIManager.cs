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
        [SerializeField]
        private GameObject creditsCanvas;

        private GameMaster.GameState lastState = 
            GameMaster.GameState.none;

        // Use this for initialization
        void Start()
        {
            mainMenuCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(false);
            inGameCanvas.SetActive(false);
            creditsCanvas.SetActive(false);
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
                                inGameCanvas,
                                creditsCanvas
                            });
                        mainMenuCanvas.SetActive(true);
                        break;
                    case GameMaster.GameState.Paused:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                mainMenuCanvas,
                                inGameCanvas,
                                creditsCanvas
                            });
                        pauseMenuCanvas.SetActive(true);
                        break;
                    case GameMaster.GameState.InGame:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                mainMenuCanvas,
                                pauseMenuCanvas,
                                creditsCanvas
                            });
                        inGameCanvas.SetActive(true);
                        break;
                    case GameMaster.GameState.Credits:
                        SetArrayToActiveState(
                            false,
                            new GameObject[]
                            {
                                mainMenuCanvas,
                                pauseMenuCanvas,
                                inGameCanvas
                            });
                        creditsCanvas.SetActive(true);
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

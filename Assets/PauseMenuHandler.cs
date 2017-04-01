using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KRaB.Split.UI
{
    public class PauseMenuHandler : MonoBehaviour
    {
        #region Data
        [Header("Buttons")]
        [SerializeField]
        private Button resumeButton;
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button mainMenuButton;

        private new AudioSource audio;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            InitializeListeners();
            audio = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            InitializeListeners();
        }
        private void OnDisable()
        {
            RemoveListeners();
        }

        void Update()
        {

        }
        #endregion

        #region Button Functions
        private void ResumeButton()
        {
            Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
            Manager.GameMaster.Instance.gameState = 
                Manager.GameMaster.GameState.InGame;
        }
        private void RestartButton()
        {
            Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
            /*Manager.GameMaster.Instance.savedState =
                Manager.GameMaster.GameState.InGame;
            Manager.GameMaster.ReloadScene();*/
        }
        private void MainMenuButton()
        {
            Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
            Manager.GameMaster.ReloadScene();
        }
        private void InitializeListeners()
        {
            resumeButton.onClick.AddListener(() => ResumeButton());
            restartButton.onClick.AddListener(() => RestartButton());
            mainMenuButton.onClick.AddListener(() => MainMenuButton());
        }
        private void RemoveListeners()
        {
            resumeButton.onClick.RemoveAllListeners();
            restartButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
        }
        #endregion
    }
}

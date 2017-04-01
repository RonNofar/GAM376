using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KRaB.Split.UI
{
    public class MainMenuHandler : MonoBehaviour
    {
        #region Data
        [Header("Buttons")]
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button exitButton;

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
        private void StartButton()
        {
            Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
            Manager.GameMaster.Instance.gameState = 
                Manager.GameMaster.GameState.InGame;
        }
        private void ExitButton()
        {
            Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
            Manager.GameMaster.Exit();
        }
        private void InitializeListeners()
        {
            startButton.onClick.AddListener(() => StartButton());
            exitButton.onClick.AddListener(() => ExitButton());
        }
        private void RemoveListeners()
        {
            startButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        } 
        #endregion
    }
}

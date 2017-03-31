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
        #endregion

        #region Unity Functions
        private void Awake()
        {
            InitializeListeners();
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
            Manager.GameMaster.Instance.gameState = 
                Manager.GameMaster.GameState.InGame;
        }
        private void ExitButton()
        {
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

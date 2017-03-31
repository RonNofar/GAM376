using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.UI
{
    public class GUIManager : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField]
        private GameObject mainMenuCanvas;
        [SerializeField]
        private GameObject pauseMenuCanvas;

        // Use this for initialization
        void Start()
        {
            mainMenuCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour {

    public void onCLick()
    {
        KRaB.Split.Manager.GameMaster.Instance.gameObject.GetComponent<AudioSource>().Play();
        KRaB.Split.Manager.GameMaster.Instance.gameState = KRaB.Split.Manager.GameMaster.GameState.MainMenu;//UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

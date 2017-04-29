using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour {

    public void onCLick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

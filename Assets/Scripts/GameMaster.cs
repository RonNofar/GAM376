using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public GameObject soccerball;
    public int balls;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < balls; ++i)
            {
                Instantiate(soccerball);
            }
        }
	}
}

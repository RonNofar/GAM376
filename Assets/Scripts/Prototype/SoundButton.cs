using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour {

    public KRaB.Split.Environment.SoundGroup sound;
    public AudioSource aud;

	// Update is called once per frame
	void Update () {
        if(Input.anyKeyDown)
        sound.Play(aud);
	}
}

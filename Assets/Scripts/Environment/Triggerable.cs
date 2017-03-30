using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour {

    public bool Activated { get; private set; }

	// Use this for initialization
	protected virtual void Start () {
        Activated = true;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
}

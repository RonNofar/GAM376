using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollider : MonoBehaviour {

    public List<GameObject> occupants;

    private void Start()
    {
        occupants = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        occupants.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        occupants.Add(collision.gameObject);
    }


}

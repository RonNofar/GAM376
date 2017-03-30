using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : Triggerable
{

    protected bool movingUp;

    [Header("Linking for the Lift", order = 1)]
    [SerializeField]
    protected GameObject Top;
    [SerializeField]
    protected GameObject Bottom;
    [SerializeField]
    protected GameObject Platform;
    [SerializeField]
    protected LiftCollider Collider;

    protected Vector3 destination { get { return (movingUp ? Top : Bottom).transform.position; } }
    protected Vector3 travelDirection { get { return (destination- Platform.transform.position).normalized; } }

    [Header("Adjustments", order = 2)]
    [SerializeField]
    protected float speed;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        movingUp = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float distance = speed * Time.deltaTime;
        if (distance * distance > (destination - Platform.transform.position).sqrMagnitude)
        {
            Move(destination - Platform.transform.position);
            movingUp = !movingUp;
        }
        else
        {
            Move( speed * Time.deltaTime * travelDirection);
        }

    }
    private void Move(Vector3 move)
    {
        Platform.transform.position += move;
        foreach (GameObject go in Collider.occupants)
            go.transform.position += move;
    }

}

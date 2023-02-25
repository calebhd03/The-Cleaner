using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endingPositionObject;
    public float duration;

    private Vector3 startingPosition;
    private Vector3 endingPosition;

    public void OnAwake()
    {
        startingPosition = transform.position;
        endingPosition = endingPositionObject.transform.position;
    }

    /// 'Hits' the target for a certain amount of damage
    

    public void Activate()
    {
        Debug.Log(this + "Activated");
        //Move door 
        this.transform.position = Vector3.Lerp(startingPosition, endingPosition, duration);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorManager : MonoBehaviour
{

    public bool doorCanBeToggled;
    public bool Open = false;
    public GameObject door;
    public float duration;
    public GameObject endingPositionObject;

    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public Vector3 endingPosition;

    public void buttonHit()
    {
        StartCoroutine(door.GetComponent<Interactable>().OpenDoor());
    }
}

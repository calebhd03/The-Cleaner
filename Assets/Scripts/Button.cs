using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Interactable doorManager;

    public void Start()
    {
        doorManager = GetComponentInParent<Interactable>();
    }

    public void hit()
    {
        //doorManager.DoorMove();
    }
}

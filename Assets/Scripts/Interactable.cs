using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    public bool DoorStartsOpen;

    private Animator myDoor;
    private bool openTrigger;

    void Start()
    {
        Animator myDoor = GetComponent<Animator>();

        if(DoorStartsOpen)
        {
            openTrigger = false;
        }
        else
        {
            openTrigger = true;
        }
    }

    //Toggles the door
    public void Hit()
    {
        if (openTrigger)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

    }

    public void OpenDoor()
    {
        GetComponent<Animator>().SetTrigger("DoorOpening");

        openTrigger = false;
    }
    public void CloseDoor()
    {
        GetComponent<Animator>().SetTrigger("DoorClosing");

        openTrigger = true;
    }
}

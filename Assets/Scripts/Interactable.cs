using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool DoorStartsOpen;

    public Light doorLight;

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

        doorLight.color = Color.green;

        openTrigger = false;
    }
    public void CloseDoor()
    {
        GetComponent<Animator>().SetTrigger("DoorClosing");

        doorLight.color = Color.red;

        openTrigger = true;
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool DoorStartsOpen;

    public Light doorLight;
    public GameObject Pulser;

    private Animator myDoor;
    private bool openTrigger;

    private AudioManager AudioManager;

    void Start()
    {
        Animator myDoor = GetComponent<Animator>();

        AudioManager = GetComponent<AudioManager>();

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

        AudioManager.Play("DoorOpen");
    }
    public void CloseDoor()
    {
        GetComponent<Animator>().SetTrigger("DoorClosing");

        doorLight.color = Color.red;

        openTrigger = true;

        AudioManager.Play("DoorClose");
    }

    public IEnumerator Pulse()
    {
        GameObject pulse = null;
        for (int i = 0; i < 3; i++)
        {
            Destroy(pulse);
            Debug.Log("Spawned Pulse");
            pulse = Instantiate(Pulser, transform.position, Quaternion.identity, transform);
            pulse.transform.position = new Vector3(pulse.transform.position.x, 2, pulse.transform.position.z);
            yield return new WaitForSeconds(3f);
        }
    }
}

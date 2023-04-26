using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject Enemies;

    public bool WaveSpawningRoom;
    public Interactable NextDoor = null;
    public List<Interactable> Doors = null;


    private bool firstTimeEntering = true;

    // Start is called before the first frame update
    void Start()
    {
        deactivateEnemies();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player enter room
        //if player leave room
        if (other.CompareTag("Player"))
        {
            activateEnemies();

            if (firstTimeEntering && WaveSpawningRoom)
            {
                StartCoroutine(GetComponent<WaveSpawner>().waveSpawnEnemies());
                closeAllDoors();
                firstTimeEntering= false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if player leave room
        if(other.CompareTag("Player"))
        {
            //deactivateEnemies();
        }
    }

    void activateEnemies()
    {
        Transform EChild = Enemies.transform;

        //Set all enemy active
        foreach (Transform child in EChild)
        {
            if(child.GetComponent<BasicEnemy>().isDead != true)
                child.gameObject.SetActive(true);
        }
    }

    void deactivateEnemies()
    {
        Transform EChild = Enemies.transform;

        //Set all enemy inactive
        foreach (Transform child in EChild)
        {
            child.gameObject.SetActive(false);
        }
    }

    void closeAllDoors()
    {
        foreach (Interactable door in Doors) 
        { 
            door.CloseDoor();
        }    
    }

    public void openNextDoor()
    {
        NextDoor.OpenDoor();

        //Spawn Pulser Above door
        StartCoroutine(NextDoor.Pulse());
    }
}

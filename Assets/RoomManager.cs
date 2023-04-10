using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        despawnEnemies();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player enter room
        //if player leave room
        if (other.CompareTag("Player"))
        {
            spawnEnemies();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if player leave room
        if(other.CompareTag("Player"))
        {
            despawnEnemies();
        }
    }

    void spawnEnemies()
    {
        //Set all enemy active
        foreach (Transform child in transform)
        {
            if(child.GetComponent<BasicEnemy>().isDead != true)
                child.gameObject.SetActive(true);
        }
    }

    void despawnEnemies()
    {
        //Set all enemy inactive
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}

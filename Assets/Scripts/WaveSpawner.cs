using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject BasicEnemy;
    public GameObject WaveSpawnerObj;
    public GameObject Enemies;
    public GameManager GameManager;
    public int NumberOfWavesLeft;
    public int RemainingEnemies = 0;
    public List<Vector3> SpawnPoints;

    

    // Start is called before the first frame update
    void Start()
    {
        //Set all enemy active
        foreach (Transform child in WaveSpawnerObj.transform)
        {
            SpawnPoints.Add(child.position);
        }
    }

    public void updateRemainingEnemies()
    {
        RemainingEnemies--;

        if(RemainingEnemies <= 0 )
        {
            waveSpawnEnemies();
        }
    }

    public void waveSpawnEnemies()
    {
        if(NumberOfWavesLeft > 0)
        {
            foreach (Vector3 point in SpawnPoints)
            {
                GameObject newEnemy = Instantiate(BasicEnemy, point, Quaternion.Euler(new Vector3(0,-45,0)), Enemies.transform);

                //makes it so they always persue the player
                newEnemy.GetComponent<BasicEnemy>().isAlwaysAngry = true;
                newEnemy.GetComponent<BasicEnemy>().isAngered = true;
            }

            RemainingEnemies = SpawnPoints.Count;
            NumberOfWavesLeft--;
            GameManager.totalEnemies += SpawnPoints.Count;
        }
        else
        {
            GetComponent<RoomManager>().openNextDoor();
        }
    }
}

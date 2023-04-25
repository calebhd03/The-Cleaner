using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject BasicEnemy;
    public GameObject WaveSpawnerObj;
    public GameObject Enemies;
    public GameManager GameManager;
    public int NumberOfWavesLeft;
    public int RemainingEnemies = 0;
    public float TimeInBetweenEnemySpawns;
    public List<Vector3> SpawnPoints;

    

    // Start is called before the first frame update
    void Start()
    {
        //Set all enemy active
        foreach (Transform child in WaveSpawnerObj.transform)
        {
            SpawnPoints.Add(child.position);
        }

        foreach (Transform child in Enemies.transform)
        {
            RemainingEnemies++;
        }

        Shuffle(SpawnPoints);
    }

    void Shuffle(IList<Vector3> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void updateRemainingEnemies()
    {
        RemainingEnemies--;

        if(RemainingEnemies <= 0 )
        {
            StartCoroutine(waveSpawnEnemies());
        }
    }

    public IEnumerator waveSpawnEnemies()
    {
        if(NumberOfWavesLeft > 0)
        {
            foreach (Vector3 point in SpawnPoints)
            {
                //Debug.Log("Spawned Enemy");

                GameObject newEnemy = Instantiate(BasicEnemy, point, Quaternion.Euler(new Vector3(0,-45,0)), Enemies.transform);

                //makes it so they always persue the player
                newEnemy.GetComponent<BasicEnemy>().isAlwaysAngry = true;
                newEnemy.GetComponent<BasicEnemy>().isAngered = true;

                RemainingEnemies++; 
                GameManager.totalEnemies++;
                yield return new WaitForSeconds(TimeInBetweenEnemySpawns);
            }

            NumberOfWavesLeft--;
        }
        else
        {
            Debug.Log("Room Finished");
            GetComponent<RoomManager>().openNextDoor();
        }
    }
}

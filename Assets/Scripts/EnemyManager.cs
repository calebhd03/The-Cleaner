using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyManager : MonoBehaviour
{
    public GameObject Player;
    public bool BatonAttackSystem;
    public float BatonSafeDistance;

    public List<GameObject> ListOfEnemies;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in ListOfEnemies)
        {
            MoveEnemies(enemy);
        }
    }

    //Removes and Enemy from the List
    public void EnemyKill(GameObject enemy)
    {
        ListOfEnemies.Remove(enemy);
    }

    //Manages the movement for each enemy based on the values stored in its prefab
    void MoveEnemies(GameObject enemy)
    {
        BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
        float Distance = Vector3.Distance(Player.transform.position, enemy.transform.position);

        if (Distance > enemyScript.SpotingDistance)
        {
            enemyScript.Passived();
        }

        if (Distance < enemyScript.SpotingDistance)
        {
            enemyScript.Angered();
        }

        if (enemyScript.isAngered)
        {
            EnemyAttack(enemyAgent, enemyScript, enemy);
        }
        
        if (!enemyScript.isAngered)
        {
            enemyAgent.isStopped = true;
        }
    }
    void EnemyAttack(NavMeshAgent enemyAgent, BasicEnemy enemyScript, GameObject enemy)
    {
        enemyAgent.isStopped = false;

        Vector3 targetPosition = Player.transform.position;

        if (BatonAttackSystem)
        {
            //check to see if enemy is currently attacking player
            if (!enemyScript.IsReadyToAttack)
            {
                //Moves the enemy to the ready to attack position
                Vector3 dir = enemy.transform.position - Player.transform.position;

                Ray r = new Ray(Player.transform.position, dir);

                targetPosition = r.GetPoint(BatonSafeDistance);
            }
        }
        enemyAgent.SetDestination(targetPosition);
    }



}

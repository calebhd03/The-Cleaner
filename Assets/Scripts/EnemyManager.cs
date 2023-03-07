using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyManager : MonoBehaviour
{
    public GameObject Player;
    public bool BatonAttackSystem;
    public float BatonSafeDistance;

    public List<GameObject> ListOfEnemies;

    // Update is called once per frame
    void Update()
    {
        //Handle each enemies brain and targeting
        foreach (GameObject enemy in ListOfEnemies)
        {
            MoveEnemies(enemy);
        }
    }

    //Manages the movement for each enemy based on the values stored in the enemy's prefab
    //Gets passed 1 enemy at a time
    void MoveEnemies(GameObject enemy)
    {
        //Finds the enemy components
        BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();

        //Finds the distance betweent the player and the enemy
        float Distance = Vector3.Distance(Player.transform.position, enemy.transform.position);

        //Check if player is out of enemies PassiveDistance
        if (Distance > enemyScript.PassiveDistance)
        {
            enemyScript.Passived();
        }

        //Check if player is inside of enemies SpotingDistance
        else if (Distance < enemyScript.SpotingDistance)
        {
            enemyScript.Angered();
            EnemyAttack(enemyAgent, enemyScript, enemy);
        }
        
    }
    void EnemyAttack(NavMeshAgent enemyAgent, BasicEnemy enemyScript, GameObject enemy)
    {
        //check to see if enemy is not ready to go for an attack
        //Moves the enemy to the waiting to attack position
        if (!enemyScript.IsReadyToAttack)
        {
            //find the waiting point in the world
            Vector3 dir = enemy.transform.position - Player.transform.position;
            Ray r = new Ray(Player.transform.position, dir);
            Vector3 targetPosition = r.GetPoint(BatonSafeDistance);

            enemyAgent.SetDestination(targetPosition);
        }

        //Enemy is attacking player
        else
        {
            enemyAgent.SetDestination(Player.transform.position);
        }
    }

    //Removes and Enemy from the List of enemies
    public void EnemyKill(GameObject enemy)
    {
        ListOfEnemies.Remove(enemy);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [Header("Enemy Numbers")]
    public float health;
    public int Damage;

    [Header ("Targeting")]
    public float SpotingDistance;
    public float PassiveDistance;
    public float TimeInBetweenAttacks;
    public bool IsReadyToAttack = true;
    public bool isAngered = false;

    private EnemyManager EnemyManager;
    private NavMeshAgent NavAgent;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager = transform.parent.GetComponent<EnemyManager>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    //Enemy is no longer targeting player
    public void Passived()
    {
        //Reassign enemy booleans
        isAngered = false;
        IsReadyToAttack = true;

        //Stop the NavMeshAgent
        NavAgent.isStopped = true;

        //Start idle animation
    }

    //Enemy is no targeting player
    public void Angered()
    {
        //Reassign enemy booleans
        isAngered = true;

        //Start the NavMeshAgent
        NavAgent.isStopped = false;

        //Start walk animation
    }

    //Enemy has made collided with player
    public void HitPlayer()
    {
        //Start attack animation

        //Used for attack delay
        IsReadyToAttack = false;
        StartCoroutine(ReadyToAttackDelay());
    }

    //Enemy has been hit by weapon
    public void Hit(float damage)
    {
        //Enemy Takes Damage from the parameter
        health -= damage;

        //Check if enemy died
        if (health <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
        else
        {
            //Start taken damage animation
        }
    }

    //Used for delaying attacks
    IEnumerator ReadyToAttackDelay()
    {
        yield return new WaitForSeconds(TimeInBetweenAttacks);
        IsReadyToAttack = true;
    }

    //Called when enemy dies
    IEnumerator EnemyDeath()
    {
        //Player death animation
        //Animation.Start();
        yield return new WaitForSeconds(1f/*Animation.length*/);

        //Remove the enemy from the scene
        GetComponentInParent<EnemyManager>().EnemyKill(this.gameObject);
        this.gameObject.SetActive(false);
    }
}

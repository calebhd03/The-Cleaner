using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float AttackWaitingDistance;
    public bool IsReadyToAttack = true;
    public bool isAngered = false;

    private AudioSource SoundSource;
    private EnemyManager EnemyManager;
    private NavMeshAgent NavAgent;
    private GameObject Player;
    private AudioManager AudioManager;
    private GameManager GameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager = transform.parent.GetComponent<EnemyManager>();
        NavAgent = GetComponent<NavMeshAgent>();
        SoundSource = GetComponent<AudioSource>();
        Player = GameObject.FindWithTag("Player");
        GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        AudioManager = GetComponent<AudioManager>();

        SoundSource.enabled = false;
    }
    private void Update()
    {
        //Finds the distance betweent the player and the enemy
        float Distance = Vector3.Distance(Player.transform.position, transform.position);

        //Check if player is out of enemies PassiveDistance
        if (isAngered)
        {
            Passived(Distance);
        }

        //Check if player is inside of enemies SpotingDistance
        else if (!isAngered && Distance < SpotingDistance)
        {
            Angered(Distance);
        }
    }

    //Enemy has made collided with player
    public void HitPlayer()
    {
        //Start attack animation

        //Start light attack sound

        //Used for attack delay
        IsReadyToAttack = false;
        StartCoroutine(ReadyToAttackDelay());
    }

    //Enemy has been hit by weapon
    public void Hit(float damage)
    {
        //Enemy Takes Damage from the parameter
        health -= damage;
        GameManagerScript.bulletsHit++;

        //Check if enemy died
        if (health <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
        else
        {
            //Start taken damage animation

            //Start damage taken sound
            AudioManager.Play("DamageTaken");
        }
    }


    //Enemy is no longer targeting player
    private void Passived(float Distance)
    {
        if(Distance > PassiveDistance)
        {
            //Reassign enemy booleans
            isAngered = false;
            IsReadyToAttack = true;

            //Stop the NavMeshAgent
            NavAgent.isStopped = true;

            //Start idle animation
        }

        //Only do ambient sound 10% of the time
        if(Random.Range(0, 100) > 90)
            //idle Sound
            AudioManager.Play("Ambient");
    }

    //Enemy is no targeting player
    private void Angered(float Distance)
    {
        if (Distance > PassiveDistance)
        {
            //Reassign enemy booleans
            isAngered = true;

            //Start the NavMeshAgent
            NavAgent.isStopped = false;

            //Start walk animation
        }

        //Start walk sound
        SoundSource.enabled = true;

        EnemyAttack();
    }

    void AttackPlayer(Vector3 targetPosition)
    {
        //Play attack animation

        //Target player
        NavAgent.SetDestination(targetPosition);
    }

    void EnemyAttack()
    {
        //check to see if enemy is not ready to go for an attack
        //Moves the enemy to the waiting to attack position
        if (!IsReadyToAttack)
        {
            //find the waiting point in the world
            Vector3 dir = transform.position - Player.transform.position;
            Ray r = new Ray(Player.transform.position, dir);
            Vector3 targetPosition = r.GetPoint(AttackWaitingDistance);

            AttackPlayer(targetPosition);
        }

        //Enemy is attacking player
        else
        {
            NavAgent.SetDestination(Player.transform.position);
        }

        //FootstepSound
        if (NavAgent.velocity.x > 0 || NavAgent.velocity.z > 0)
            SoundSource.enabled = true;
        else
            SoundSource.enabled = false;
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
        //Start enemy death sound
        AudioManager.Play("Death");
        GameManagerScript.totalScuttleKilled++;

        //Enemy death animation
        //Animation.Start();
        yield return new WaitForSeconds(0f/*Animation.length*/);

        //Remove the enemy from the scene
        this.gameObject.SetActive(false);
    }
}

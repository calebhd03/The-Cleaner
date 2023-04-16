using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    public Collider AttackBox;

    [Header("Enemy Numbers")]
    public float health;
    public int Damage;
    public bool isDead = false;
    public bool isAlwaysAngry = false;

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
    private Animator Animator;

    private bool PlayAmbient = true;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager = transform.parent.GetComponent<EnemyManager>();
        NavAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("Player");
        GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        AudioManager = GetComponent<AudioManager>();
        Animator = GetComponent<Animator>();

        //GameManagerScript.totalEnemies++;
    }
    private void Update()
    {
        
        if (isDead != true)
        {
            //Finds the distance betweent the player and the enemy
            float Distance = Vector3.Distance(Player.transform.position, transform.position);


            //Check if player is out of enemies PassiveDistance
            if (isAlwaysAngry)
            {
                Angered(Distance);
            }

            else
            {
                CheckIfPassiveOrAngry(Distance);

                if (isAngered)
                {
                    Angered(Distance);
                }
                else
                {
                    Passived(Distance);
                }
            }

        }

        if(PlayAmbient)
            StartCoroutine(PlayAmbientSound());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Animator.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator.SetBool("Attack", false);
        }

    }

    IEnumerator PlayAmbientSound()
    {
        PlayAmbient = false;
        yield return new WaitForSeconds(Random.Range(5f, 15f));

        if(!isDead)
        {
            AudioManager.Play("Ambient");
            PlayAmbient = true;
        }
    }

    //Enemy has made collided with player
    public void HitPlayer()
    {
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
            EnemyDeath();
        }
        else
        {
            //Start taken damage animation
            Animator.SetBool("Hit", true);

            //Start damage taken sound
            AudioManager.Play("DamageTaken");

            StartCoroutine(WaitAnimationLength("Hit", false));
        }
    }

    IEnumerator WaitAnimationLength(string boolName, bool boolState)
    {
        float time = Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);

        Animator.SetBool(boolName, boolState);
    }

    private void CheckIfPassiveOrAngry(float Distance)
    {
        if (Distance > PassiveDistance)
        {
            //Reassign enemy booleans
            isAngered = false;
            IsReadyToAttack = true;

            //Stop the NavMeshAgent
            NavAgent.isStopped = true;

        }

        else if(Distance < SpotingDistance)
        {
            //Reassign enemy booleans
            isAngered = true;

            //Start the NavMeshAgent
            NavAgent.isStopped = false;

        }
    }

    //Enemy Animation
    //Enemy is no longer targeting player
    private void Passived(float Distance)
    {
        //Stop walk animation
        Animator.SetBool("Moving", false);
    }

    //Enemy is no targeting player
    private void Angered(float Distance)
    {
        EnemyAttack();


        //Set animator direction
        Vector3 direction = Vector3.ClampMagnitude(NavAgent.velocity, 1);
        Animator.SetFloat("MoveX", direction.x);
        Animator.SetFloat("MoveZ", direction.z);
        
        //Checks if enemy is not moving
        if(direction.x == 0 && direction.z == 0) 
        { 
           Animator.SetBool("Moving", false);
        }
        else
        {
            //Start walk animation
            Animator.SetBool("Moving", true);
        }
    }

    void WaitingForPlayer(Vector3 targetPosition)
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

            WaitingForPlayer(targetPosition);
        }

        //Enemy is attacking player
        else
        {
            NavAgent.SetDestination(Player.transform.position);
        }
    }

    //Used for delaying attacks
    IEnumerator ReadyToAttackDelay()
    {
        yield return new WaitForSeconds(TimeInBetweenAttacks);
        IsReadyToAttack = true;
    }

    //Called when enemy dies
    void EnemyDeath()
    {
        //Set death bool
        isDead = true;

        //Start enemy death sound
        AudioManager.Play("Death");
        GameManagerScript.totalScuttleKilled++;

        //Enemy death animation
        Animator.SetBool("Died", true);

        //Stop the NavMeshAgent
        NavAgent.isStopped = true;

        //Remove the enemy from the scene
        Transform parent = transform.parent;
        parent.GetComponentInParent<WaveSpawner>().updateRemainingEnemies();

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<NavMeshAgent>());

    }
}

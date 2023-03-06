using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [Header("Enemy Numbers")]
    public float health;

    [Header ("Targeting")]
    public float SpotingDistance;
    public float PassiveDistance;
    public float TimeInBetweenAttacks;
    public bool IsReadyToAttack = true;
    public bool isAngered = false;

    private EnemyManager EnemyManager;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager = transform.parent.GetComponent<EnemyManager>();
    }
    public void Passived()
    {
        isAngered = false;
        IsReadyToAttack = true;
    }

    public void Angered()
    {
        isAngered = true;
    }

    public void HitPlayer()
    {
        IsReadyToAttack = false;
        StartCoroutine(ReadyToAttackDelay());
    }
    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GetComponentInParent<EnemyManager>().EnemyKill(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator ReadyToAttackDelay()
    {
        yield return new WaitForSeconds(TimeInBetweenAttacks);
        IsReadyToAttack = true;
    }
}

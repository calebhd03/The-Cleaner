using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject Player;
    public float SpotingDistance;
    public float PassiveDistance;
    public float Distance;

    public bool isAngered;

    public NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(Player.transform.position, this.transform.position);

        if(Distance < SpotingDistance )
        {
            isAngered = true;
        }
        if(Distance > SpotingDistance)
        {
            isAngered = false;
        }

        if (isAngered)
        {
            _agent.isStopped = false;
            _agent.SetDestination(Player.transform.position);
        }

        if (!isAngered)
        {
            _agent.isStopped = true;
        }
    }
}

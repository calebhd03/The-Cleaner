using JetBrains.Annotations;
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

    }

}

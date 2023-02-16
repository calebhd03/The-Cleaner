using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter(Collision other) 
    {
        Target target = other.gameObject.GetComponent<Target>();
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        if(target.CompareTag("Enemy")) 
        {
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

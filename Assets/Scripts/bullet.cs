using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage;

    void Awake()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Target target = other.gameObject.GetComponent<Target>();
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        if(other.CompareTag("Enemy")) 
        {
            Debug.Log("HIT");
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

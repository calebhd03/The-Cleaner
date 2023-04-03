using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage; 
    public float bulletSpeed;

    void Awake()
    {
        Destroy(gameObject, 5f);
    }

    public void MoveBullet()
    {
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject); // Deletes the round
        }
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        if (other.CompareTag("Enemy"))
        {
            BasicEnemy target = other.gameObject.GetComponent<BasicEnemy>();
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else if (other.CompareTag("Interactable"))
        {
            Interactable door = other.gameObject.GetComponentInParent<Interactable>();
            door.Hit();
            Destroy(gameObject); // Deletes the round
        }
    }
}

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
            Button button = other.gameObject.GetComponent<Button>();
            button.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

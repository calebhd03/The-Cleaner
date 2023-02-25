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
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("HIT");
            Target target = other.gameObject.GetComponent<Target>();
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else if (other.CompareTag("Interactable"))
        {
            Debug.Log("Interactable");
            Button target = other.gameObject.GetComponent<Button>();
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

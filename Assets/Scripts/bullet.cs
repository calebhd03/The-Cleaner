using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage; 
    public float bulletSpeed;

    void Awake()
    {
        Debug.Log("bulletSpeed = " + bulletSpeed);
        Destroy(gameObject, 5f);
    }

    public void MoveBullet()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletSpeed;
    }

    void OnTriggerEnter(Collider other) 
    {
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        if (other.CompareTag("Enemy"))
        {
            Target target = other.gameObject.GetComponent<Target>();
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
        else if (other.CompareTag("Interactable"))
        {
            Debug.Log("Interactable");
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

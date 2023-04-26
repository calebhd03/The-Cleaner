using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScutlordProjectile : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, 8f);
    }
    private void Update()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Character>().checkIfLosingHealth(null);
            other.GetComponent<Character>().TookDamage();
        }

        else if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

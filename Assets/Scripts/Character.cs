using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Character : MonoBehaviour
{
    public float MovementSpeed;
    public int Health;
    public float InvincibleTime;
    public float invincibilityDeltaTime;
    public GameObject Gun;
    public GameObject Model;
    
    private Vector2 moveInput;
    private bool isInvincible;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Awake()
    {
        isInvincible = false;
        rb2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + moveInput * MovementSpeed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("TRIGGER");
        GameObject target = other.gameObject;
        if(target.CompareTag("Enemy"))
        {
            if(!isInvincible)
            {
                
                Health--;
                TookDamage();
            }
        }
    }
    void OnFire()
    {
        Debug.Log("PlayerFire");
        Gun.GetComponent<HandgunController>().Shoot();
    }

    void TookDamage()
    {
        Debug.Log("Damage Taken");
        //Update UI

        //Cheack if Dead
        if(Health <= 0) {
            Destroy(gameObject);
        }

        //Make invincible
        StartCoroutine(BecomeTemporarilyInvincible());
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        Debug.Log("--------Invincible");
        for (float i = 0; i < InvincibleTime; i += invincibilityDeltaTime)
        {
            // Alternate between 0 and 1 scale to simulate flashing
            if (Model.transform.localScale == Vector3.one)
            {
                Model.transform.localScale = Vector3.zero;
            }
            else
            {
                Model.transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        Model.transform.localScale = Vector3.one; 
        isInvincible = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MovementSpeed;
    public int Health;
    public float InvincibleTime;
    
    [SerializeField]
    [Range(0.1f, 3)]private float invincibilityDeltaTime;

    private GameObject Model;
    private bool isInvincible;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        rb2d = GetComponent<Rigidbody2D> ();
        Model = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
 
        rb2d.velocity = new Vector2 (moveHorizontal*MovementSpeed, moveVertical*MovementSpeed);
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
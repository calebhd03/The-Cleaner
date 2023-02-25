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

    [Header("FogOfWar")]
    //public FogOfWar fogOfWar;
    //public Transform secondaryFogOfWar;
    [Range(0, 5)] public float sightDistance;
    public GameObject FOVMesh;
    public float checkInterval;

    private Vector3 moveInput3D;
    private bool isInvincible;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        isInvincible = false;
        rb = GetComponent<Rigidbody> ();
        FOVMesh.SetActive(true);
        FOVMesh.transform.localScale = new Vector3(sightDistance, sightDistance, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput3D * MovementSpeed * Time.fixedDeltaTime);
        //rb.velocity = moveInput3D;
    }

    void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        moveInput3D = new Vector3(moveInput.x, moveInput.y, 0);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
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
        Gun.GetComponent<HandgunController>().Shoot();
    }

    void TookDamage()
    {
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
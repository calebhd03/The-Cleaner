using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Character : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject Gun;
    public GameObject Model;
    public GameObject GlowCharge;
    public GameObject CameraPivot;

    [Header("Player Numbers")]
    public float MovementSpeed;
    public int Health;
    public float InvincibleTime;
    public float invincibilityDeltaTime;
    public int MaxGlowCharges;
    public float ThrowDistance;
    public float ThrowTime;

    private Vector3 moveInput3D;
    private bool isInvincible;
    private Rigidbody rb;
    private int CurrentGlowCharges;
    private CameraMainScript PivotScript;

    // Start is called before the first frame update
    void Awake()
    {
        isInvincible = false;
        rb = GetComponent<Rigidbody>();
        CurrentGlowCharges = MaxGlowCharges;
        PivotScript = CameraPivot.GetComponent<CameraMainScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput3D * MovementSpeed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        moveInput3D = new Vector3(moveInput.x, 0, moveInput.y);
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
    void OnThrowGlowCharge()
    {
        if(CurrentGlowCharges > 0)
        {
            //CurrentGlowCharges--;

            GameObject spawnedGlowCharge = Instantiate(GlowCharge, transform.position, transform.rotation);
            StartCoroutine(ThrowCharge(spawnedGlowCharge));
        }
    }
    IEnumerator ThrowCharge(GameObject glowCharge)
    {
        Debug.Log("ThrowingCharge!!!!!!!");
        Ray r = new Ray(transform.position, PivotScript.getMousePostition() - transform.position);
        Debug.DrawRay(transform.position, transform.position - PivotScript.getMousePostition(), Color.yellow, 20f);
        RaycastHit hit;
        Vector3 EndingPosition;
        int layerMask =~ LayerMask.GetMask("IgnoreRaycast");
        if (Physics.Raycast(r, out hit, ThrowDistance, layerMask))
        {
            EndingPosition = hit.transform.position;
        }
        else
        {
            EndingPosition = r.GetPoint(ThrowDistance);
        }
        EndingPosition.y = transform.position.y; 
        Debug.Log("EndingPosition" + EndingPosition);
        Debug.Log("MousePosition" + PivotScript.getMousePostition());

        float elapsedTime = 0;
        Vector3 startingPosition = glowCharge.transform.position;
        while (elapsedTime < ThrowTime)
        {
            glowCharge.transform.position = Vector3.Lerp(startingPosition, EndingPosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
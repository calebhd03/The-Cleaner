using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject EnemyManager;

    [Header("Player Numbers")]
    public float MovementSpeed;
    public int Health;
    public float InvincibleTime;
    public float invincibilityDeltaTime;

    [Header("Glow Charges")]
    public int MaxGlowCharges;
    public float ThrowDistance;
    public float ThrowDuration;
    public float GlowChargeRechargeDelay;

    private Vector3 moveInput3D;
    private bool isInvincible;
    private Rigidbody rb;
    private int CurrentGlowCharges;
    private int MaxHealth;
    private CameraMainScript PivotScript;
    private EnemyManager EnemyManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        MaxHealth = Health;
        CurrentGlowCharges = MaxGlowCharges;
        EnemyManagerScript = EnemyManager.GetComponent<EnemyManager>();

        rb = GetComponent<Rigidbody>();
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

    void OnTriggerEnter(Collider other) 
    {
        GameObject target = other.gameObject;
        if(target.CompareTag("Enemy"))
        {
            if(!isInvincible)
            {
                Health--;
                TookDamage();
            }
            target.GetComponent<BasicEnemy>().HitPlayer();
        }
    }

    void OnFire()
    {
        Gun.GetComponent<HandgunController>().Shoot();
    }

    void OnThrowGlowCharge()
    {
        if (CurrentGlowCharges > 0)
        {
            if (CurrentGlowCharges >= MaxGlowCharges)
                StartCoroutine(GlowChargeRecharge());

            CurrentGlowCharges--;


            GameObject spawnedGlowCharge = Instantiate(GlowCharge, transform.position, transform.rotation);

            Ray r = new Ray(transform.position, PivotScript.getMousePostition() - transform.position);
            RaycastHit hit;
            Vector3 EndingPosition;

            int layerMask = ~LayerMask.GetMask("IgnoreRaycast");
            if (Physics.Raycast(r, out hit, ThrowDistance, layerMask))
            {
                EndingPosition = hit.transform.position;
            }
            else
            {
                EndingPosition = r.GetPoint(ThrowDistance);
            }

            EndingPosition.y = transform.position.y;
            Transform startingPosition = spawnedGlowCharge.transform;

            spawnedGlowCharge.GetComponent<GlowCharge>().CreateGlowCharge(EndingPosition, ThrowDuration);
        }
    }

    IEnumerator GlowChargeRecharge()
    {
        yield return new WaitForSeconds(GlowChargeRechargeDelay);
        CurrentGlowCharges++;
        if(CurrentGlowCharges != MaxGlowCharges)
            StartCoroutine(GlowChargeRecharge());
    }

    void TookDamage()
    {
        //Update UI

        //Cheack if Dead
        if(Health <= 0) {
            gameObject.SetActive(false);
        }

        //Make invincible
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
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
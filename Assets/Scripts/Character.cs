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
    public GameObject AudioManagerObj;
    public PauseMenu PauseScript;

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
    private AudioSource SoundSource;
    private AudioManager AudioManager;
    private GameManager GameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManagerScript = EnemyManager.GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody>();
        PivotScript = CameraPivot.GetComponent<CameraMainScript>();
        SoundSource = GetComponent<AudioSource>();
        AudioManager = AudioManagerObj.GetComponent<AudioManager>();
        GameManagerScript = GameObject.FindAnyObjectByType<GameManager>().GetComponent<GameManager>();

        isInvincible = false;
        MaxHealth = Health;
        CurrentGlowCharges = MaxGlowCharges;
        SoundSource.enabled = false;
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

        //Footstep Sound
        if (moveInput.x > 0 || moveInput.y > 0)
            SoundSource.enabled = true;
        else
            SoundSource.enabled = false;
        
    }

    void OnTriggerEnter(Collider other) 
    {
        GameObject target = other.gameObject;
        if(target.CompareTag("Enemy"))
        {
            if(!isInvincible)
            {
                Health-= target.GetComponent<BasicEnemy>().Damage;
                TookDamage();
            }
            target.GetComponent<BasicEnemy>().HitPlayer();
        }
    }

    void OnFire()
    {
        GameManagerScript.bulletsFired++;
        Gun.GetComponent<HandgunController>().Shoot();
    }

    void OnThrowGlowCharge()
    {
        //If has extraGlowCharges
        if (CurrentGlowCharges > 0)
        {
            //If at max glowCharges
            if (CurrentGlowCharges >= MaxGlowCharges)
                StartCoroutine(GlowChargeRecharge());

            //Remove number of glowCharges
            CurrentGlowCharges--;
            GameManagerScript.glowChargesThrown++;

            //Play Audio
            AudioManager.Play("GlowChargeThrow");

            //Calculate throwing direction of glowCharge
            Vector3 EndingPosition;

            EndingPosition = PivotScript.getMousePostition() - transform.position;
            EndingPosition.y = transform.position.y;

            //Create glowcharge with its value
            GameObject spawnedGlowCharge = Instantiate(GlowCharge, transform.position, transform.rotation);
            spawnedGlowCharge.GetComponent<GlowCharge>().throwDirection = EndingPosition;
            spawnedGlowCharge.GetComponent<GlowCharge>().DoneSettingUpCharge();
        }
    }

    IEnumerator GlowChargeRecharge()
    {
        yield return new WaitForSeconds(GlowChargeRechargeDelay);

        //increase glowcharges
        CurrentGlowCharges++;

        //Update UI Showing glowcharges

        //If not at full glowCharges
        if (CurrentGlowCharges != MaxGlowCharges)
            StartCoroutine(GlowChargeRecharge());
    }

    void OnPause()
    {
        PauseScript.toggle();
    }

    void TookDamage()
    {
        //Update UI

        //Cheack if Dead
        if(Health <= 0)
        {
            AudioManager.Play("PlayerDeath"); 
            GameManagerScript.deaths++;
            gameObject.SetActive(false);
        }

        //Make invincible
        else
        {
            AudioManager.Play("PlayerDamageTaken");
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
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
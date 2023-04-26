using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [Header("Game Objects")]
    public HandgunController HandgunController;
    public GameObject Model;
    public GameObject GlowCharge;
    public GameObject CameraPivot;
    public GameObject AudioManagerObj;
    public PauseMenu PauseScript;
    public healthBarScript healthBar;
    public GlowChargeUI GlowChargeUI;
    public Animator BodyAnimator;
    public Animator ThrowAnimator;
    public GameObject ThrowingArm;
    public GameObject bloodParticle;

    [Header("Player Numbers")]
    public float MovementSpeed;
    public int Health;
    public float InvincibleTime;
    public float invincibilityDeltaTime;

    [Header("I Dash")]
    public float DashRechargeTime;
    public float DashInvincibleTime;
    public float DashSpeed;
    public float DashGraceTime;
    public bool IsDashing = false;
    public bool CanDash = true;


    [Header("Glow Charges")]
    public int MaxGlowCharges;
    public float ThrowDistance;
    public float ThrowDuration;
    public float GlowChargeRechargeDelay;

    private Vector3 moveInput3D;
    private bool isInvincible = false;
    private Rigidbody rb;
    private int CurrentGlowCharges;
    private int MaxHealth;
    private CameraMainScript PivotScript;
    private AudioSource SoundSource;
    private AudioManager AudioManager;
    private GameManager GameManagerScript;
    private float playerVolume;
    private float enemyVolume;
    private bool throwRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PivotScript = CameraPivot.GetComponent<CameraMainScript>();
        SoundSource = GetComponent<AudioSource>();
        AudioManager = AudioManagerObj.GetComponent<AudioManager>();
        GameManagerScript = GameObject.FindAnyObjectByType<GameManager>().GetComponent<GameManager>();

        //Set up health
        MaxHealth = Health;
        HealthToMax();

        CurrentGlowCharges = MaxGlowCharges;
        SoundSource.enabled = false;

        SwitchToPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDashing == false)
        {
            //rb.MovePosition(rb.position + moveInput3D * MovementSpeed * Time.fixedDeltaTime);
            //rb.AddForce(moveInput3D * MovementSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

            rb.velocity = moveInput3D * MovementSpeed;
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        moveInput3D = new Vector3(moveInput.x, 0, moveInput.y);

        //Movement Animation
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            BodyAnimator.SetBool("IsMoving", true);

            //throw direction
            if (moveInput.x > 0)
                throwRight = true;
            else
                throwRight = false;
        }
        else
            BodyAnimator.SetBool("IsMoving", false);

        //Movement Direction
        if(moveInput.x != 0)
            BodyAnimator.SetFloat("MoveX", moveInput.x);

    }

    void OnTriggerEnter(Collider other) 
    {
        GameObject target = other.gameObject;
        if(target.CompareTag("Enemy"))
        {
            checkIfLosingHealth(target.GetComponent<BasicEnemy>());
        }
    }

    public void checkIfLosingHealth(BasicEnemy target)
    {
        if (!isInvincible)
        {
            Health--;
            TookDamage();
        }
        target.GetComponent<BasicEnemy>().HitPlayer();

    }

    void OnFire()
    {
        GameManagerScript.bulletsFired++;
        HandgunController.Shoot();
    }

    void OnReload()
    {
        HandgunController.Reload();
    }

    void OnThrowGlowCharge()
    {
        //If has extraGlowCharges
        if (CurrentGlowCharges > 0)
        {
            //Throw GlowCharge Animation

            if (throwRight)
                ThrowAnimator.SetTrigger("ThrowRight");
            else
                ThrowAnimator.SetTrigger("ThrowLeft");
        }
    }

    public void ThrowCharge()
    {
        //If at max glowCharges
        if (CurrentGlowCharges >= MaxGlowCharges)
            StartCoroutine(GlowChargeRecharge());

        //Update UI
        else
            GlowChargeUI.GlowChargeUsedUI(CurrentGlowCharges);

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


    IEnumerator GlowChargeRecharge()
    {
        float time = 0f;
        while(time < GlowChargeRechargeDelay)
        {
            yield return null;
            time += Time.deltaTime; 
            
            GlowChargeUI.SetGlowChargeCooldownUI(time / GlowChargeRechargeDelay, CurrentGlowCharges);


        }
        //yield return new WaitForSeconds(GlowChargeRechargeDelay);


        //increase glowcharges
        CurrentGlowCharges++;
        AudioManager.Play("GlowchargeCD");

        //Update UI Showing glowcharges
        GlowChargeUI.SetGlowChargeRecharged(CurrentGlowCharges);

        //If not at full glowCharges
        if (CurrentGlowCharges != MaxGlowCharges)
            StartCoroutine(GlowChargeRecharge());
    }

    void OnPause()
    {
        SwitchToUI();
        PauseScript.toggle();
    }

    public void SwitchToUI()
    {
        //turn off sounds
        //Load AudioMixer
        AudioMixer audioMixer = null;
        audioMixer = Resources.Load<AudioMixer>("MainMixer");

        if (audioMixer == null)
            Debug.LogWarning("Mixer " + this.gameObject + " not found");

        else
        {
            //Change volume dBs
            audioMixer.GetFloat("PlayerVolume", out playerVolume);
            audioMixer.GetFloat("EnemyVolume", out enemyVolume);
            audioMixer.SetFloat("PlayerVolume", -80);
            audioMixer.SetFloat("EnemyVolume", -80);
        }

        DisablePlayerInput();
    }

    public void SwitchToPlayer()
    {
        //turn on sounds
        //Load AudioMixer
        AudioMixer audioMixer = null;
        audioMixer = Resources.Load<AudioMixer>("MainMixer");

        if (audioMixer == null)
            Debug.LogWarning("Mixer " + this.gameObject + " not found");

        else
        {
            //Change volume dBs
            audioMixer.SetFloat("PlayerVolume", playerVolume);
            audioMixer.SetFloat("EnemyVolume", enemyVolume);
        }

        //Enable character movement
        EnablePlayerInput();

    }

    public void EnablePlayerInput()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindActionMap("Player").Enable();
        input.actions.FindActionMap("UI").Disable();
    }
    public void DisablePlayerInput()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindActionMap("Player").Disable();
        input.actions.FindActionMap("UI").Enable();
    }

    public void HealthToMax()
    {
        Health = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    public void TookDamage()
    {
        //Update UI
        healthBar.SetHealth(Health);

        GameManagerScript.damageTaken++;

        //Blood Particle
        Instantiate(bloodParticle, new Vector3(transform.position.x, .2f, transform.position.z), Quaternion.identity);

        //Cheack if Dead
        if (Health <= 0)
        {
            Died();
        }

        //Make invincible
        else
        {
            AudioManager.Play("PlayerDamageTaken");
            StartCoroutine(BecomeTemporarilyInvincible(InvincibleTime));
        }
    }
    void Died()
    {

        AudioManager.Play("PlayerDeath");
        GameManagerScript.deaths++;
        Time.timeScale = 0f;
        Model.SetActive(false);
        SwitchToUI();
        //play death animation
        //play death particle

        GameManagerScript.levelOver();
    }

    void OnDash()
    {
        if(CanDash)
            StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        //Disable playermovement
        CanDash =false;
        IsDashing= true;
        //rb.isKinematic = false;

        AudioManager.Play("CleanerDash");

        //find and apply the dash direction
        Vector3 DashDirection = new Vector3(moveInput3D.x * DashSpeed, 0f, moveInput3D.z * DashSpeed);
        rb.AddForce(DashDirection, ForceMode.Impulse);


        //Make player invincible
        StartCoroutine(BecomeTemporarilyInvincible(DashInvincibleTime + DashGraceTime));
        yield return new WaitForSeconds(DashInvincibleTime);

        //Reinable player movement
        IsDashing = false;
        //rb.isKinematic = true;

        //Dash Recharge
        yield return new WaitForSeconds(DashRechargeTime - DashInvincibleTime);
        CanDash = true;

    }

    private IEnumerator BecomeTemporarilyInvincible(float timeInvincible)
    {
        isInvincible = true;
        for (float i = 0; i < timeInvincible; i += invincibilityDeltaTime)
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
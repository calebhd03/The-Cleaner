using UnityEngine;

public class HandgunController : MonoBehaviour
{
    public enum ShootState {
        Ready,
        Shooting,
        Reloading
    }

    public GameObject CameraPivot;
    private CameraMainScript PivotScript;
    public AudioManager AudioManager;
    public GameObject ShootParticle;
    public AmmoBarScript ammobar;
    public Animator GunAnimator;

    [Header("Magazine")]
    public GameObject bullet;
    public GameObject Player;
    public GameObject muzzle;
    public int ammunition;
    [Range(0.5f, 10)] public float reloadTime;
    public int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)] public float fireRate;
    public int bulletsPerShot;
    [Range(0.5f, 100)] public float bulletSpeed;
    public int damage;
    [Range(0, 45)] public float maxbulletVariation;

    private ShootState shootState = ShootState.Ready;
    private Vector3 worldPosition;
    private AudioSource soundSource;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    void Start() 
    {
        remainingAmmunition = ammunition;
        PivotScript = CameraPivot.GetComponent<CameraMainScript>();

    }

    void Update() 
    {
        switch(shootState) {
            case ShootState.Shooting:
                // If the gun is ready to shoot again...
                if(Time.time > nextShootTime) {
                    shootState = ShootState.Ready;
                }
                break;
            case ShootState.Reloading:
                // If the gun has finished reloading...
                if(Time.time > nextShootTime) {
                    //Gun is ready to reload
                    remainingAmmunition = ammunition;
                    shootState = ShootState.Ready;
                    ammobar.SetReload();
                }
                break;
        }

        pointGunAtMouse();
    }

    ///Points gun at mouse world position
    public void pointGunAtMouse()
    {
        Vector3 mousePosition = PivotScript.getMousePostition();
        mousePosition.y = transform.position.y;
        transform.LookAt(mousePosition, Vector3.up);
        //GunAnimator.SetFloat("MousePosition", transform.rotation.eulerAngles.y);

        if (transform.rotation.eulerAngles.y < 180)
            GunAnimator.SetTrigger("Right");
        else
            GunAnimator.SetTrigger("Left");
    }

    /// Attempts to fire the gun
    public void Shoot() {
        // Checks that the gun is ready to shoot
        if(shootState == ShootState.Ready) {
            for(int i = 0; i < bulletsPerShot; i++) {
                // Instantiates the bullet at the muzzle position
                GameObject spawnedbullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);

                spawnedbullet.GetComponent<bullet>().bulletSpeed = bulletSpeed;
                spawnedbullet.GetComponent<bullet>().damage = damage; 
                var localDirection = spawnedbullet.transform.rotation * Vector3.right;
                spawnedbullet.GetComponent<Rigidbody>().velocity = localDirection * bulletSpeed;


                //Spawn particle
                Instantiate(ShootParticle, muzzle.transform);

                //Plays animation
                muzzle.GetComponentInChildren<Animator>().SetTrigger("GunShot");

                //Plays audio
                AudioManager.Play("WeaponShoot");
            }

            remainingAmmunition--;

            //Remove UI
            ammobar.SetAmmo(remainingAmmunition);

            //Wait for firerate
            if (remainingAmmunition > 0) {
                nextShootTime = Time.time + (1 / fireRate);
                shootState = ShootState.Shooting;
            } else {
                Reload();
            }
        }
    }

    /// Attempts to reload the gun
    public void Reload() {
        // Checks that the gun is ready to be reloaded
        if(shootState == ShootState.Ready) {

            AudioManager.Play("WeaponReload");
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
        }
    }
}

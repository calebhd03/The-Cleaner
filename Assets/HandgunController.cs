using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandgunController : MonoBehaviour
{
    public enum ShootState {
        Ready,
        Shooting,
        Reloading
    }

    // How far forward the muzzle is from the centre of the gun
    private float muzzleOffset;

    [Header("Magazine")]
    public GameObject bullet;
    public int ammunition;

    [Range(0.5f, 10)] public float reloadTime;

    private int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)] public float fireRate;

    // The number of bullets fired each shot
    public int bulletsPerShot;

    [Range(0.5f, 100)] public float bulletSpeed;

    // The maximum angle that the bullet's direction can vary,
    // in both the horizontal and vertical axes
    [Range(0, 45)] public float maxbulletVariation;

    private ShootState shootState = ShootState.Ready;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    void Start() {
        muzzleOffset = GetComponent<Renderer>().bounds.extents.z;
        remainingAmmunition = ammunition;
    }

    void Update() {

        //if(Input.GetAxis("Fire1"))
            //Shoot();
        //if(Input.GetAxis("Reload"))
            //Reload();

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
                    remainingAmmunition = ammunition;
                    shootState = ShootState.Ready;
                }
                break;
        }
    }

    /// Attempts to fire the gun
    public void Shoot() {
        // Checks that the gun is ready to shoot
        Debug.Log("FIRE");
        if(shootState == ShootState.Ready) {
            for(int i = 0; i < bulletsPerShot; i++) {
                // Instantiates the bullet at the muzzle position
                GameObject spawnedbullet = Instantiate(bullet, transform.position + transform.forward * muzzleOffset, transform.rotation);

                // Add a random variation to the bullet's direction
                spawnedbullet.transform.Rotate(new Vector3(Random.Range(-1f, 1f) * maxbulletVariation, Random.Range(-1f, 1f) * maxbulletVariation, 0));

                Rigidbody2D rb = spawnedbullet.GetComponent<Rigidbody2D>();
                rb.velocity = spawnedbullet.transform.forward * bulletSpeed;
            }

            remainingAmmunition--;
            if(remainingAmmunition > 0) {
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
        Debug.Log("Reload");
        if(shootState == ShootState.Ready) {
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
        }
    }
}

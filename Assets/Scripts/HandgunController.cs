using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
    public GameObject muzzle;
    public int ammunition;

    [Range(0.5f, 10)] public float reloadTime;

    public int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)] public float fireRate;

    // The number of bullets fired each shot
    public int bulletsPerShot;

    [Range(0.5f, 100)] public float bulletSpeed;

    public int damage;
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
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldMousePos.z = 0;
        Quaternion rotation = Quaternion.Euler( 0, 0, Mathf.Atan2 ( worldMousePos.y, worldMousePos.x ) * Mathf.Rad2Deg );
                
        transform.rotation = rotation;
        //transform.rotation = Quaternion.SetLookRotation(rotation);

    }

    /// Attempts to fire the gun
    public void Shoot() {
        // Checks that the gun is ready to shoot
        Debug.Log("FIRE");
        if(shootState == ShootState.Ready) {
            for(int i = 0; i < bulletsPerShot; i++) {
                // Instantiates the bullet at the muzzle position
                
                Vector3 shootDir = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - muzzle.transform.position).normalized;

                GameObject spawnedbullet = Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
                
                spawnedbullet.GetComponent<bullet>().damage = damage;

                Rigidbody2D rb = spawnedbullet.GetComponent<Rigidbody2D>();
                rb.velocity = shootDir * bulletSpeed;
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

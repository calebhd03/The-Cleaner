using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

public class HandgunController : MonoBehaviour
{
    public enum ShootState {
        Ready,
        Shooting,
        Reloading
    }

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
    Plane plane = new Plane(Vector3.up, 0);

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    void Start() 
    {
        remainingAmmunition = ammunition;
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
                    remainingAmmunition = ammunition;
                    shootState = ShootState.Ready;
                }
                break;
        }

        pointGunAtMouse();
    }

    ///Points gun at mouse world position
    public void pointGunAtMouse()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        worldPosition.y = Player.transform.position.y;
        transform.LookAt(worldPosition, Vector3.up);
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
        if(shootState == ShootState.Ready) {
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
        }
    }
}

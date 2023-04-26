using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scutlord : MonoBehaviour
{
    public ScutlordHealthBar healthBar;
    public Interactable Door;

    public void Hit(float health)
    {
        healthBar.SetHealth((int) health);
    }

    public void SetMaxHealth(float health)
    {
        healthBar.SetMaxHealth((int) health);
    }

    public void Death()
    {
        Destroy(healthBar.gameObject);
        Door.OpenDoor();
    }
}

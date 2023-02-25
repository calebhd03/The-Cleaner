using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float health;
    public GameObject door;

    private Interactable interactable;

    public void OnAwake()
    {
        Interactable interactable = door.GetComponent<Interactable>();
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            interactable.Activate();
        }
    }
}

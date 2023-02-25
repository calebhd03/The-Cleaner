using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private doorManager doorManager;

    public void Start()
    {
        doorManager = GetComponentInParent<doorManager>();
    }

    public void Hit(float damage)
    {
        if (!doorManager.doorCanBeToggled)
            if (!doorManager.Open)
                doorManager.buttonHit();
            else { }
        else
            doorManager.buttonHit();
    }
}

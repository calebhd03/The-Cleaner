using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Button : MonoBehaviour
{
    public GameObject particle;

    private Interactable doorManager;
    private bool broken = true;


    public void Start()
    {
        doorManager = GetComponentInParent<Interactable>();
    }

    public void hit()
    {
        if(broken)
        {
            particle.SetActive(false);
            broken= false;
        }
        else
        {
            particle.SetActive(true);
            broken= true;
        }

        doorManager.Hit();
    }
}

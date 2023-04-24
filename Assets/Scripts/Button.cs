using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Button : MonoBehaviour
{
    public GameObject particle;

    private Interactable doorInteractable;
    private bool broken = true;


    public void Start()
    {
        doorInteractable = GetComponentInParent<Interactable>();
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

        Debug.Log("Button hit " + transform);
        doorInteractable.Hit();
    }
}

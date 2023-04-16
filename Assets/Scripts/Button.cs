using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject particle;

    public float lerpTime;

    private Interactable doorManager;


    public void Start()
    {
        doorManager = GetComponentInParent<Interactable>();
    }

    public void hit()
    {
        Destroy(particle);
        doorManager.Hit();
    }
}

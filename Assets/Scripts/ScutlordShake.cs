using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScutlordShake : MonoBehaviour
{
    public float intesity;
    public float time;

    Shake shake = null;

    // Start is called before the first frame update
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Shake>();
    }

    public void ShakeCamera()
    {
        shake.shakeM(intesity, time);
    }
}

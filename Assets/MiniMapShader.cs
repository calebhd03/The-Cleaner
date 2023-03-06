using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapShader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().SetReplacementShader(Shader.Find("SelfIlluminated/Color"), "RenderType");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

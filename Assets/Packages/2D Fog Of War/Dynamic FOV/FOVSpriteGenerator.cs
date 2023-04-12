using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FOVSpriteGenerator : MonoBehaviour
{

    public int sightDistance;
    public int precision;
    public GameObject triangle;

    private List<GameObject> triangleList;

    void Start()
    {
        AddFOVMesh();
    }

    // Edit the vertices obtained from the sprite.  Use OverrideGeometry to
    // submit the changes.
    void AddFOVMesh()
    {
        //Fetch the Sprite and vertices from the SpriteRenderer
        float angleper = 360f / precision;
        float angleDegree = 0;
        float angle = 0;
        for (int i = 0; i < precision; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / precision;

            Vector3 dir = new Vector3(x * sightDistance, 0, z * sightDistance);
            Ray r = new Ray(transform.position, dir);
            //Debug.Log(r + " Ray " + i);
            Debug.DrawRay(transform.position, dir, Color.green, 20f);

            GameObject newObj = Instantiate<GameObject>(triangle, transform.position, Quaternion.Euler(90f, 0f, angleDegree), transform);
            
            //triangleList.Add(newObj);

            angleDegree += 360f / precision;


            newObj.transform.localScale = new Vector3(3.14f*sightDistance*2/precision *1.1f, sightDistance, 1);
            newObj.GetComponent<FOVSprite>().sightDistance = sightDistance;
        }
    }

}
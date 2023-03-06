using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMeshDynamic1 : MonoBehaviour
{
    public int sightDistance;
    public int precision;
    //public Material material;
    public GameObject cube;
    public Texture2D Knob;

    private GameObject FOVMesh;
    private Sprite mySprite;
    private SpriteRenderer sr;

    void Start()
    {

        // Set up game object with mesh;
        mySprite = Sprite.Create(Knob, new Rect(0.0f, 0.0f, Knob.width, Knob.height), new Vector2(0.5f, 0.5f), 100.0f);

        this.GetComponent<SpriteRenderer>().sprite = mySprite;
        this.GetComponent<SpriteMask>().sprite = mySprite;


        UpdateFOVMeshValues();
    }

    void Update()
    {
        //UpdateFOVMeshValues();
    }

    void UpdateFOVMeshValues()
    {
        //Create the Vector3 vertices
        Vector3[] vertices3D = new Vector3[precision+1];
        int[] triangles = new int[precision*3];

        float angle = 0;
        for (int i = 0; i < precision; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / precision;

            Vector3 dir = new Vector3(transform.position.x * x, 0, transform.position.z * z);
            Ray r = new Ray(transform.position, dir);
            //Debug.Log(r + " Ray " + i);
            Debug.DrawRay(transform.position, dir, Color.green, 20f);

            RaycastHit hit;

            if(Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                vertices3D[i] = hit.transform.position;
                //GameObject Cube = Instantiate(cube);
                //Cube.transform.position = hit.transform.position; 
            }
            else
            {
                vertices3D[i] = r.GetPoint(sightDistance);
                //GameObject Cube = Instantiate(cube);
                //Cube.transform.position = vertices3D[i - 1];
            }



            
            //Debug.Log(vertices3D[i] + " Vertice " + i);
        }
        vertices3D[precision] = transform.position;
        //Debug.Log(vertices3D[precision] + " Vertice " + precision);

        //Create UV
        Vector2[] uv = new Vector2[vertices3D.Length];
        for(int i = 0; i < vertices3D.Length; i++)
        {
            uv[i].x = vertices3D[i].x;
            uv[i].y = vertices3D[i].z;
            GameObject Cube = Instantiate(cube);
            Cube.transform.position = uv[i];
        }


        // Use the triangulator to get indices for creating triangles
        int nextVertice = 0;
        for(int i=0; i < triangles.Length; i+=3)
        {
            if (i <= triangles.Length - 3)
            {
                triangles[i + 2] = nextVertice;
                triangles[i + 1] = nextVertice + 1;
                triangles[i] = vertices3D.Length - 1;
            }
            else
            {
                triangles[i + 2] = 0;
                triangles[i + 1] = nextVertice + 1;
                triangles[i] = vertices3D.Length - 1;
            }
                //Debug.Log(triangles[i] + " triangles0 " + i);
                //Debug.Log(triangles[i + 1] + " triangles1 " + i + 1);
                //Debug.Log(triangles[i + 2] + " triangles2 " + i + 2);
                nextVertice += 1;
        }

        // Create the mesh
        mySprite.OverrideGeometry(uv, mySprite.triangles);
        //FOVMesh.transform.position = transform.position;

    }
}

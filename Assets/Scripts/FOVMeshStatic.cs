using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMeshStatic : MonoBehaviour
{
    public float sightDistance;
    public int precision;

    private GameObject FOVMesh;

    public void CreateStaticFOVMeshValues()
    {
        //Variables that will hold the points that make up a mesh
        Vector3[] vertices3D = new Vector3[precision+1];
        int[] triangles = new int[precision*3];
        Vector2[] uv = new Vector2[vertices3D.Length];

        //Fill the vertices3D[] with the world points that will make up the mesh
        findVertices(vertices3D);

        //Create UVs from the vertices
        findUv(uv, vertices3D);


        //Fill triangles[] with the triangles needed for creating the mesh
        findTriangles(vertices3D, triangles);

        //Create the FOVmesh
        //Sets the FOVmesh as a child of this object
        createFOVMesh(vertices3D, uv, triangles);
    }

    //Cast rays in a circle from the player to find the world verticies
    void findVertices(Vector3[] vertices3D)
    {
        float angle = 0;
        for (int i = 0; i < precision; i++)
        {
            //calculate the angle needed to cast the ray
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / precision;

            //creates the array at the angle above
            Vector3 dir = new Vector3(transform.position.x * x, 0, transform.position.z * z);
            Ray r = new Ray(transform.position, dir);

            //if the ray hits a wall then the transform of the wall gets set as the vertice
            //otherwise it saves a world point at sightDistance away from the player
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                vertices3D[i] = hit.transform.position;
            }
            else
            {
                vertices3D[i] = r.GetPoint(sightDistance);
            }
        }

        //holds the vertice for the center of the circle
        vertices3D[precision] = transform.position;
    }

    //takes the vertices3D and curverts them into vector2 so they can be used in UVs
    void findUv(Vector2[] uv, Vector3[] vertices3D)
    {
        for (int i = 0; i < vertices3D.Length; i++)
        {
            uv[i] = vertices3D[i];
        }
    }

    //Creates the triangles needed for the circle based off of left hand
    void findTriangles(Vector3[] vertices3D, int[] triangles)
    {
        //loops through all of the vertices and assigns their index to triangles[]
        int nextVertice = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //used for all of the triangles except the last
            if (i < triangles.Length - 3)
            {
                triangles[i + 2] = nextVertice;
                triangles[i + 1] = nextVertice + 1;
                triangles[i] = vertices3D.Length - 1;
            }
            //used for the last triangle
            else
            {
                triangles[i + 2] = nextVertice;
                triangles[i + 1] = 0;
                triangles[i] = vertices3D.Length - 1;
            }
            nextVertice += 1;
        }
    }

    //Create the FOVmesh
    //Sets the FOVmesh as a child of this object
    void createFOVMesh(Vector3[] vertices3D, Vector2[] uv, int[] triangles)
    {
        //Creats the empty GameObject that holds the FOVMesh
        FOVMesh = new GameObject("StaticFOVMesh", typeof(MeshFilter), typeof(MeshRenderer));
        
        //Assigns the layer to the FOV layer
        FOVMesh.layer = 7;

        //Assigns values to the mesh 
        //Defines where and how big the mesh is
        FOVMesh.GetComponent<MeshFilter>().mesh.vertices = vertices3D;
        FOVMesh.GetComponent<MeshFilter>().mesh.triangles = triangles;
        FOVMesh.GetComponent<MeshFilter>().mesh.uv = uv;
        
        //Sets the FOVMesh GameObject as a child
        FOVMesh.transform.parent = transform;
    }
}

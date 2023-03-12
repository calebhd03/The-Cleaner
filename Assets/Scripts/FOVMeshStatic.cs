using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMeshStatic : MonoBehaviour
{
    public float sightDistance;
    public int precision;

    private GameObject FOVMesh;

    public void UpdateFOVMeshValues()
    {
        //Create the Vector3 vertices
        Vector3[] vertices3D = new Vector3[precision + 1];
        Vector2[] uv = new Vector2[vertices3D.Length];
        int[] triangles = new int[precision * 3];

        //Gets the vertices
        GetVertices(vertices3D);

        //Finds the UVs from the vertices
        FindUVs(vertices3D, uv);

        //Finds the triangles needed to fill the mesh from the vertices
        FindTriangles(triangles, vertices3D);

        //Create the mesh
        //Set up game object with mesh
        FOVMesh = new GameObject("StaticFOVMesh", typeof(MeshFilter), typeof(MeshRenderer));
        FOVMesh.layer = 7;

        //Updates the mesh with the new vertices
        FOVMesh.GetComponent<MeshFilter>().mesh.vertices = vertices3D;
        FOVMesh.GetComponent<MeshFilter>().mesh.triangles = triangles;
        FOVMesh.GetComponent<MeshFilter>().mesh.uv = uv;

        //Sets the object as a child as this object
        FOVMesh.transform.parent = transform;
    }

    private void GetVertices(Vector3[] vertices3D)
    {
        float angle = 0;
        for (int i = 0; i < precision; i++)
        {
            //Finds the next angle for checking the distance
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            //Increments the next angle based off precision
            angle += 2 * Mathf.PI / precision;

            //Creates the ray based off the angle
            Vector3 dir = new Vector3(transform.position.x * x, 0, transform.position.z * z);
            Ray r = new Ray(transform.position, dir);

            //Checks if the ray hit a wall
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                //If hit wall then save the transform as the vertice
                vertices3D[i] = hit.transform.position;
            }
            else
            {

                //If didnt hit wall then save the sightdistance aways as the vertice
                vertices3D[i] = r.GetPoint(sightDistance);
            }
        }
        //Sets the center vertice of the circle
        vertices3D[precision] = transform.position;
    }

    //Creates the UVs off of the the vertices
    private void FindUVs(Vector3[] vertices3D, Vector2[] uv)
    {
        //Converts the vertices into Vector2
        for (int i = 0; i < vertices3D.Length; i++)
        {
            uv[i] = vertices3D[i];
        }
    }

    //Finds the triangles needed to fullfill the rendering of the mesh
    //The triangles are from left handed
    private void FindTriangles(int[] triangles, Vector3[] vertices3D)
    {
        //Use the triangulator to get indices for creating triangles
        int nextVertice = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //All triangles except last
            if (i < triangles.Length - 3)
            {
                triangles[i + 2] = nextVertice;
                triangles[i + 1] = nextVertice + 1;
                triangles[i] = vertices3D.Length - 1;
            }
            //Last triangle
            else
            {
                triangles[i + 2] = nextVertice;
                triangles[i + 1] = 0;
                triangles[i] = vertices3D.Length - 1;
            }
            nextVertice++;
        }
    }
}
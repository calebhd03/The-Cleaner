using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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
            
            triangleList.Add(newObj);

            angleDegree += 360f / precision;


            newObj.transform.localScale = new Vector3(3.14f*sightDistance*2/precision, sightDistance, 1);
            newObj.GetComponent<FOVSprite>().sightDistance = sightDistance;
        }
    }

    private void a()
    {
        float angleper = 360f / precision;
        float angleDegree = 0;
        float angle = angleper / 2;
        for (int i = 0; i < precision; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / precision;

            Vector3 dir = new Vector3(x * sightDistance, 0, z * sightDistance);
            Ray r = new Ray(transform.position, dir);
            //Debug.Log(r + " Ray " + i);
            Debug.DrawRay(transform.position, dir, Color.green, 20f);


            SpriteRenderer sr = triangleList[i].GetComponent<SpriteRenderer>();

            Sprite o = sr.sprite;
            Vector2[] sv = o.vertices;

            sv[2].x = o.bounds.extents.x;

            RaycastHit hit;
            if (Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                Vector3 wall = r.GetPoint(sightDistance);
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                float percentOfSight = distance / sightDistance;
                sv[2].x *= percentOfSight;
                Debug.Log("wall = " + wall + ": postition = " + transform.position);
                Debug.Log("sv " + i + " = " + sv[i] + ": percent = " + percentOfSight + ": distance = " + distance);
                //GameObject Cube = Instantiate(cube);
                //Cube.transform.position = hit.transform.position; 
            }
            else
            {
                //sv[2].x = o.bounds.extents.x;

                //GameObject Cube = Instantiate(cube);
                //Cube.transform.position = vertices3D[i - 1];
            }

            sv[i].x = Mathf.Clamp(
                (o.vertices[i].x - o.bounds.center.x -
                    (o.textureRectOffset.x / o.texture.width) + o.bounds.extents.x) /
                (2.0f * o.bounds.extents.x) * o.rect.width,
                0.0f, o.rect.width);

            sv[i].y = Mathf.Clamp(
                (o.vertices[i].y - o.bounds.center.y -
                    (o.textureRectOffset.y / o.texture.height) + o.bounds.extents.y) /
                (2.0f * o.bounds.extents.y) * o.rect.height,
                0.0f, o.rect.height);




            angleDegree += 360f / precision;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeformSprite : MonoBehaviour
{
    private SpriteRenderer spriteR;
    private SpriteMask spriteM;
    private Rect buttonPos1;
    private Rect buttonPos2;

    private Texture2D texture2D;
    private Vector2[] startingSV;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteM = gameObject.GetComponent<SpriteMask>();
        // Create a blank Texture and Sprite to override later on.
        texture2D = new Texture2D(500, 500);
        spriteR.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);
        spriteM.sprite = spriteR.sprite;


        Sprite o = spriteR.sprite;
        startingSV = o.vertices;
    }

    // Show the sprite triangles
    void DrawDebug()
    {
        Debug.Log("Drawing debug");
        Sprite sprite = spriteR.sprite;

        ushort[] t = sprite.triangles;
        Vector2[] v = sprite.vertices;
        int a, b, c;

        // draw the triangles using grabbed vertices
        for (int i = 0; i < t.Length; i = i + 3)
        {
            a = t[i];
            b = t[i + 1];
            c = t[i + 2];
            Debug.DrawLine(v[a], v[b], Color.white, 100.0f);
            Debug.DrawLine(v[b], v[c], Color.white, 100.0f);
            Debug.DrawLine(v[c], v[a], Color.white, 100.0f);
        }
    }

    // Edit the vertices obtained from the sprite.  Use OverrideGeometry to
    // submit the changes.
    void Update()
    {
        float sightDistance = 3f;
        Sprite o = spriteR.sprite;
        Vector2[] sv = startingSV;

        //Debug.Log("Triangles " + o.triangles.Length);

        float angle = 0;
        for (int i = 0; i < sv.Length; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / sv.Length;

            Vector3 dir = new Vector3(x * sightDistance, 0, z * sightDistance);
            Ray r = new Ray(transform.position, dir);
            
            Debug.DrawRay(transform.position, dir, Color.green, 1f);

            RaycastHit hit;
            if (Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                Vector3 wall = r.GetPoint(sightDistance);
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                float percentOfSight = distance / sightDistance;
                sv[i] *= percentOfSight;
                Debug.Log("wall = " + wall + ": postition = " + transform.position);
                Debug.Log("sv " + i + " = " + sv[i] + ": percent = " + percentOfSight + ": distance = " + distance);
                //GameObject Cube = Instantiate(cube);
                //Cube.transform.position = hit.transform.position; 
            }
            else
            {
                sv[i].x = texture2D.width;
                sv[i].y = texture2D.height;
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



        }

        o.OverrideGeometry(sv, o.triangles);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FOVSprite : MonoBehaviour
{
    private PolygonCollider2D col;
    [HideInInspector] public float sightDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //Vector3 dir = new Vector3(x * sightDistance, 0, z * sightDistance);
        Ray r = new Ray(transform.position, -transform.up * sightDistance);
        //Debug.Log(r + " Ray " + i);
        Debug.DrawRay(transform.position, -transform.up * sightDistance, Color.green, 2f);

        RaycastHit hit;

        if (Physics.Raycast(r, out hit, sightDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            float size = Vector3.Distance(transform.position, hit.transform.position);
            transform.localScale = new Vector3(transform.localScale.x, size, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, sightDistance, transform.localScale.z);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            float size = Vector3.Distance(transform.position, collision.gameObject.transform.position);
            transform.localScale = new Vector3(transform.localScale.x, size, transform.localScale.z);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.localScale = new Vector3(transform.localScale.x, sightDistance, transform.localScale.z);
        }
    }
}

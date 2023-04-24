using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPulser : MonoBehaviour
{
    public float doorScaleSize;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x + doorScaleSize, 1, transform.localScale.z + doorScaleSize);
        if(transform.localScale.x == 500f)
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForKinematicBouncing : MonoBehaviour
{
    public float speed;
    public float timeMoving;

    public Vector3 StartingForce;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        applyForce();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void applyForce()
    {
        rb.AddForce(StartingForce, ForceMode.Impulse);

        StartCoroutine(WhileMoving());
    }

    IEnumerator WhileMoving()
    {
        yield return new WaitForSeconds(timeMoving);

        rb.velocity = Vector3.zero;
        rb.isKinematic= false;
    }
}

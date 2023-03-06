using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMainScript : MonoBehaviour
{
    public GameObject Player;

    Plane plane = new Plane(Vector3.up, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position;
    }

    public Vector3 getMousePostition()
    {
        float distance;
        Vector3 worldPosition = new Vector3(0,0,0);

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        worldPosition.y = Player.transform.position.y;

        return worldPosition;
    }
}

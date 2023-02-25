using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update

    private doorManager doorManager;
    private Vector3 endingPosition;
    private Vector3 startingPosition;
    
    void Start()
    {
        doorManager = GetComponentInParent<doorManager>();
        startingPosition = transform.position;
        endingPosition = doorManager.endingPositionObject.transform.position;
    }

    //Toggles the door
    public IEnumerator OpenDoor()
    {
        doorManager.Open = !doorManager.Open;
        Vector3 targetPosition;
        Vector3 currentPosition = transform.position;

        if (doorManager.Open)
            targetPosition = endingPosition;
        else 
            targetPosition = startingPosition;

        float elapsedTime = 0;
        while (elapsedTime < doorManager.duration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GlowCharge : MonoBehaviour
{
    [Header("Required GameObjects")]
    public GameObject SpotLight;
    public GameObject Sprite;

    [Header("Numbers")]
    public float TimeAtFullCharge;
    public float TimeTillMinimal;
    public float MinimalIntensity;
    public float ThrowHeight;

    private float MaximumIntensity;
    private Light lightComp;
    private Vector3 EndingPosition;
    private float ThrowDuration;

    // Start is called before the first frame update
    void Start()
    {
        //Setting components 
        lightComp = SpotLight.GetComponent<Light>();
        MaximumIntensity = lightComp.intensity;

        StartCoroutine(FullCharge());
        StartCoroutine(ThrowCharge());
    }

    //Gets called by Player
    public void CreateGlowCharge(Vector3 EP, float TD)
    {
        EndingPosition = EP;
        //ThrowDuration= TD;
    }

    //Once the TimeAtFullCharge has been reached reduce the intesity of the SpotLight
    IEnumerator FullCharge()
    {
        yield return new WaitForSeconds(TimeAtFullCharge);

        float elapsedTime = 0;
        while (elapsedTime < TimeTillMinimal)
        {
            lightComp.intensity = Mathf.Lerp(MaximumIntensity, MinimalIntensity, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    //Moves the charge to the target position
    //Moves the scale of the sprite to mimic being thrown
    IEnumerator ThrowCharge()
    {
        Vector3 startingPosition = transform.position;
        Vector3 startingScale = Sprite.transform.localScale;
        Animation throwing = Sprite.GetComponent<Animation>();
        ThrowDuration = throwing.clip.length;

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            //Moves towards target position
            transform.position = Vector3.Lerp(startingPosition, EndingPosition, elapsedTime);
            
            /*

            //Changes the Scale based on a parabolic scale
            float target_Y = startingScale.y * elapsedTime + ThrowHeight * (1 - Mathf.Pow((Mathf.Abs(0.5f - elapsedTime) / 0.5f), 2));
            Sprite.transform.localScale = new Vector3(target_Y, target_Y, 1);
            */

            //Deals with time
            yield return null;
            elapsedTime += Time.deltaTime * (1f / ThrowDuration);
        }

        Sprite.transform.localScale = startingScale;

        //Creates the FOV object that deals with the FogOfWar
        GetComponent<FOVMeshStatic>().UpdateFOVMeshValues();
    }
}

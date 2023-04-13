using System.Collections;
using UnityEngine;

public class GlowCharge : MonoBehaviour
{
    [Header("Required GameObjects")]
    public GameObject SpotLight;
    public GameObject Sprite;

    [Header("Light Decay")]
    public bool LightDecaysAfterTime;
    public float TimeAtFullCharge;
    public float TimeTillMinimal;
    public float MinimalIntensity;

    [Header("Throwing")]
    public float ThrowHeight;
    public float ThrowPower;
    public float MaxThrowDistance;
    public Vector3 throwDirection; //Set by player

    private Light lightComp;
    private Rigidbody rb;

    private float MaximumIntensity;
    private bool HasNotStartedMoving = true;

    private void Update()
    {
        //Check to make sure rb is set up
        if(rb != null)
        {
            //Debug.Log("Velo: " + rb.velocity);
        }    
    }

    // Start is called before the first frame update
    public void DoneSettingUpCharge()
    {
        //Setting components 
        lightComp = SpotLight.GetComponent<Light>();
        MaximumIntensity = lightComp.intensity;
        rb = GetComponent<Rigidbody>();

        //Start Light Decay
        if(LightDecaysAfterTime)
            StartCoroutine(FullCharge());

        //Apply Physics throw
        ThrowGlowCharge();
    }

    //Once the TimeAtFullCharge has been reached reduce the intesity of the SpotLight
    IEnumerator FullCharge()
    {
        yield return new WaitForSeconds(TimeAtFullCharge);

        float elapsedTime = 0;
        while (elapsedTime < TimeTillMinimal)
        {
            lightComp.intensity = Mathf.Lerp(MaximumIntensity, MinimalIntensity, elapsedTime/TimeTillMinimal);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    void ThrowGlowCharge()
    {
        //Applys throw power
        throwDirection *= ThrowPower;

        //Clamps the throw power
        throwDirection = Vector3.ClampMagnitude(throwDirection, MaxThrowDistance);

        //Applies throw height
        throwDirection.y = ThrowHeight;

        //Throws the ball
        rb.AddForce(throwDirection, ForceMode.Impulse);

        StartCoroutine(WhileMoving());

    }

    IEnumerator WhileMoving()
    {
        //Wait for object to start moving
        while(HasNotStartedMoving)
        {
            yield return null;
        }

        //Check if still moving
        while (rb.velocity != Vector3.zero)
        {
            Debug.Log("still moving: " + rb.velocity);
            yield return null;
        }

        //Removes the ability to move
        rb.isKinematic = true;
        rb.detectCollisions = false;
        GetComponent<Collider>().enabled = false;
    }
}

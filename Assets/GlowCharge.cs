using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GlowCharge : MonoBehaviour
{
    public float TimeAtFullCharge;
    public float TimeTillMinimal;
    public float MinimalIntensity;
    public float ThrowHeight;
    public GameObject SpotLight;
    public GameObject Sprite;

    private float MaximumIntensity;
    private Light lightComp;
    private Vector3 EndingPosition;
    private float ThrowDuration;

    // Start is called before the first frame update
    void Start()
    {
        lightComp = SpotLight.GetComponent<Light>();
        MaximumIntensity = lightComp.intensity;
        StartCoroutine(FullCharge());
        StartCoroutine(ThrowCharge());
    }

    public void CreateGlowCharge(Vector3 EP, float TD)
    {
        EndingPosition = EP;
        ThrowDuration= TD;
    }
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

    IEnumerator ThrowCharge()
    {
        Vector3 startingPosition = transform.position;
        Vector3 startingScale = Sprite.transform.localScale;

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            transform.position = Vector3.Lerp(startingPosition, EndingPosition, elapsedTime);
            
            float maxHeigh = startingScale.y + ThrowHeight;
            float target_Y = startingScale.y * elapsedTime + ThrowHeight * (1 - Mathf.Pow((Mathf.Abs(0.5f - elapsedTime) / 0.5f), 2));
            Sprite.transform.localScale = new Vector3(target_Y, target_Y, 1);


            yield return null;
            elapsedTime += Time.deltaTime * (1 / ThrowDuration);
        }

        Sprite.transform.localScale = startingScale;
        GetComponent<FOVMeshStatic>().UpdateFOVMeshValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

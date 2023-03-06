using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlowCharge : MonoBehaviour
{
    public float TimeAtFullCharge;
    public float TimeTillMinimal;
    public float MinimalIntensity;
    public GameObject spotLight;

    private float MaximumIntensity;
    private Light lightComp;

    // Start is called before the first frame update
    void Start()
    {
        lightComp = spotLight.GetComponent<Light>();
        MaximumIntensity = lightComp.intensity;
        StartCoroutine(FullCharge());
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

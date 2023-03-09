using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFogOfWar : MonoBehaviour
{
    public FogOfWar fogOfWar;
    public Transform secondaryFogOfWar;

    public float sightDistance;
    public float checkInterval;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(CheckFogOfWar(checkInterval));
        secondaryFogOfWar.localScale = new Vector2(sightDistance, sightDistance);
    }

    // Update is called once per frame
    void Update()
    {
        fogOfWar.MakeHole(transform.position, sightDistance);
    }

    private IEnumerator CheckFogOfWar(float checkInterval)
    {
        while (true)
        {
            
            yield return new WaitForSeconds(checkInterval);
        }
    }
}

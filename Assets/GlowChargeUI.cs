using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowChargeUI : MonoBehaviour
{
    public List<Image> images;

    //Does the circluar recharge
    public void SetGlowChargeCooldownUI(float percentRecharged, int glowChargesRemaining)
    {
        images[glowChargesRemaining].fillAmount = percentRecharged;
    }

    //Sets a image to 0 when its corresponding charge is used
    public void GlowChargeUsedUI(int glowChargesRemaining)
    {
        images[glowChargesRemaining].fillAmount= 0f;
    }

    //Sets a image to 1 when its corresponding charge is used
    public void SetGlowChargeRecharged(int glowChargesRemaining)
    {
        images[glowChargesRemaining-1].fillAmount = 1f;
    }
}

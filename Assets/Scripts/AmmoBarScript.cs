using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarScript : MonoBehaviour
{
    public List<Image> ammoImages = new List<Image>();

    public void SetReload()
    {
        foreach (Image image in ammoImages)
        {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, 1f);
        }
    }

    public void SetAmmo(int ammo)
    {
        if (ammo < 0)
            return;

        for (int i = ammoImages.Count - 1; i >= ammo; i--)
        {
            Color color = ammoImages[i].color;
            ammoImages[i].color = new Color(color.r, color.g, color.b, 0f);
        }
    }
}

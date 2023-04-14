using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour
{
    public List<Image> healthImages = new List<Image>();


    public void SetMaxHealth(float health)
    {
        foreach (Image image in healthImages)
        {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, 1f);
        }
    }

    public void SetHealth(float health)
    {
        if (health < 0)
            return;

        for(int i = healthImages.Count-1; i>=health; i--)
        {
            Color color = healthImages[i].color;
            healthImages[i].color = new Color(color.r, color.g, color.b, 0f);
        }
    }
}

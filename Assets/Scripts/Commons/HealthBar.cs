using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //public SpriteRenderer healthBarImage;
    //public void UpdateHealthBar(float fillAmount)
    //{
    //    Vector2 size = new Vector2(fillAmount, 1);
    //    healthBarImage.size = size;
    //}

    public Image healthBarImage;
    public void UpdateHealthBar(float fillAmount)
    {
        healthBarImage.fillAmount = fillAmount;
    }
}

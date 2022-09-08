using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
    public SpriteRenderer healthBarImage;
    private void Start()
    {
        healthBarImage = GetComponent<SpriteRenderer>();
    }
    public void UpdateHealthBar(float fillAmount)
    {
        Vector2 size = new Vector2(fillAmount, 1);
        healthBarImage.size = size;
    }
}

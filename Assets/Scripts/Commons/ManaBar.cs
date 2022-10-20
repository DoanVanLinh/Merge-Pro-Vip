using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image manaBarImage;
    public void UpdateManaBar(float fillAmount)
    {
        manaBarImage.fillAmount = fillAmount;
    }
}

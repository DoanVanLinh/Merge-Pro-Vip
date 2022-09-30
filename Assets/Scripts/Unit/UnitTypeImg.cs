using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTypeImg : MonoBehaviour
{
    public Image imgType;
    public void LoadTypeUnit(Sprite typeSprite)
    {
        imgType.sprite = typeSprite;
    }
}

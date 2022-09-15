using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTier : MonoBehaviour
{
    public GameObject star;
    private Unit unit;
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Helper.mainCam;
        unit = GetComponentInParent<Unit>();
        LoadTier();
    }
    void LoadTier()
    {
        int unitTier = unit.Data.tier % 5;

        unitTier = unitTier == 0 ? 5 : unitTier;

        for (int i = 0; i < unitTier; i++)
        {
            Color color = new Color32();
            ColorUtility.TryParseHtmlString("#" + unit.Data.colorTier, out color);
            Instantiate(star, Vector3.zero, Quaternion.identity, transform.GetChild(0).transform).GetComponent<Image>().color = color;
        }
    }
}

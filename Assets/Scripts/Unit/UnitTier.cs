using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WE.Unit;

public class UnitTier : MonoBehaviour
{
    public GameObject star;
    public Transform starParents;
    private BaseUnit unit;
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Helper.mainCam;
        unit = GetComponentInParent<BaseUnit>();
        LoadTier();
    }
    void LoadTier()
    {
        int unitTier = unit.unitStats.tier % 5;

        unitTier = unitTier == 0 ? 5 : unitTier;

        for (int i = 0; i < unitTier; i++)
        {
            Color color = new Color32();
            ColorUtility.TryParseHtmlString("#" + unit.unitStats.colorTier, out color);
            Instantiate(star, Vector3.zero, Quaternion.identity, starParents).GetComponent<Image>().color = color;
        }
    }
    public void SetHealthBarLoc()
    {
        GetComponent<RectTransform>().localPosition = new Vector2(0, GetComponentInParent<BoxCollider2D>().size.y + 0.1f);
    }
}

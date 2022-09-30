using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using TMPro;


public class LibaryElement : MonoBehaviour
{
    public UnitData unitData;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtHP;
    public TextMeshProUGUI txtATK;
    public SkeletonGraphic skeletonGraphic;

    private void Start()
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = unitData.skeletonData;
        skeletonGraphic.Initialize(true);
        LoadData();
    }

    public void LoadData()
    {
        if (CPlayerPrefs.HasKey(unitData.unitName))
        {
            skeletonGraphic.color = Color.white;
            txtName.text = unitData.unitName;
            txtHP.text = unitData.hp.ToString();
            txtATK.text = unitData.dame.ToString();
        }
    }
}

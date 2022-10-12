using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.UI;

public class NewUnitPanel : MonoBehaviour
{
    public UnitData unitData;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtHP;
    public TextMeshProUGUI txtATK;
    public Button buttonContinue; 
    public SkeletonGraphic skeletonGraphic;
    public Animator panelAni;

    private void OnEnable()
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = unitData.skeletonData;
        skeletonGraphic.Initialize(true);
        buttonContinue.onClick.AddListener(ContinueButton);
        LoadData(unitData);
    }
    public void ContinueButton()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        panelAni.Play("Off");
    }
    public void SetPanelOff()
    {
        gameObject.SetActive(false);
    }
    public void LoadData(UnitData unitData)
    {
        this.unitData = unitData;
        txtName.text = unitData.unitName;
        txtHP.text = unitData.hp.ToString();
        txtATK.text = unitData.damage.ToString();
    }
}

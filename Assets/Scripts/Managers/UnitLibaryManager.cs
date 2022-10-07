using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Spine.Unity;

public class UnitLibaryManager : MonoBehaviour
{
    public Transform content;
    public GameObject libaryElement;
    private List<LibaryElement> listElements = new List<LibaryElement>();
    public Button btnClose;
    public Animator panelAni;

    private void OnEnable()
    {
        btnClose.onClick.AddListener(ClosePanel);
        LoadLibary();
    }

    private void LoadLibary()
    {
        if (content.childCount == 0)
        {
            List<UnitData> listUnitdata = new List<UnitData>(GameManager.Instance.GetDataMerge().listUnits);
            foreach (UnitData unitData in listUnitdata)
            {
                LibaryElement element = Instantiate(libaryElement, Vector2.zero, Quaternion.identity, content).GetComponent<LibaryElement>();
                element.unitData = unitData;
                listElements.Add(element);
            }
        }
        else
        {
            foreach (LibaryElement element in listElements)
            {
                element.LoadData();
            }
        }
    }
    public void SetActivePanelOff()
    {
        gameObject.SetActive(false);
        UIManager.Instance.mainMenuPanel.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        panelAni.Play("Off");
    }
    private void OnDisable()
    {
        btnClose.onClick.RemoveAllListeners();
    }

}

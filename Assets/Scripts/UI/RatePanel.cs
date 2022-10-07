using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatePanel : MonoBehaviour
{
    public Button buttonClose;
    public Button buttonRate;
    public Animator panelAni;

    private void OnEnable()
    {
        buttonClose.onClick.AddListener(ButtonClose);
        buttonRate.onClick.AddListener(ButtonRate);
    }
    private void ButtonRate()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        Application.OpenURL(Helper.OPEN_LINK_RATE);
    }

    private void ButtonClose()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        panelAni.Play("Off");
        UIManager.Instance.settingPanel.gameObject.SetActive(true);
    }
    public void SetPanelOff()
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Button buttonClose;
    public Button buttonRate;
    public Button buttonContinue;
    public Button buttonSound;
    public Animator panelAni;

    private void OnEnable()
    {
        buttonClose.onClick.AddListener(ButtonClose);
        buttonRate.onClick.AddListener(ButtonRate);
        buttonContinue.onClick.AddListener(ButtonContinue);
        buttonSound.onClick.AddListener(ButtonSound);
    }

    private void ButtonSound()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
    }

    private void ButtonContinue()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        panelAni.Play("Off");
    }

    private void ButtonRate()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        panelAni.Play("Off");
        UIManager.Instance.ratePanel.gameObject.SetActive(true);
    }

    private void ButtonClose()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        panelAni.Play("Off");
    }
    public void SetPanelOff()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonRate.onClick.RemoveAllListeners();
        buttonContinue.onClick.RemoveAllListeners();
        buttonSound.onClick.RemoveAllListeners();
    }
}

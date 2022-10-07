using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button buttonSetting;
    [SerializeField] private Button buttonLibary;
    [SerializeField] private Button buttonSpawn;
    [SerializeField] private Button buttonStart;
    private void Start()
    {
    }
    private void OnEnable()
    {
        buttonSetting.onClick.AddListener(OpenSettingPanel);
        buttonLibary.onClick.AddListener(OpenLibaryPanel);
        buttonSpawn.onClick.AddListener(SpawnUnitButton);
        buttonStart.onClick.AddListener(SpawnUnitButton);
    }

    private void SpawnUnitButton()
    {
        
    }

    private void OpenSettingPanel()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        UIManager.Instance.settingPanel.gameObject.SetActive(true);
    }
    private void OpenLibaryPanel()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        UIManager.Instance.libaryPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        buttonSetting.onClick.RemoveAllListeners();
        buttonLibary.onClick.RemoveAllListeners();
        buttonSpawn.onClick.RemoveAllListeners();
        buttonStart.onClick.RemoveAllListeners();
    }
}

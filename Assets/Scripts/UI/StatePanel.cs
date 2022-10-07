using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StatePanel : MonoBehaviour
{
    public Image currentEnvironment;
    public Image nextEnvironment;
    public Transform levelsParentPanel;
    public Color completeColor;
    public GameObject levelImgPrefab;
    public SpriteRenderer environmentBackground;

    private int currentState;
    private int currentLevel;

    private void Start()
    {
        currentState = -1;
        currentLevel = -1;
    }
    public void UpdateStatePanel()
    {
        int newState = DataManager.Instance.GetDataGame().currentState;
        int newLevel = DataManager.Instance.GetDataGame().currentLevel;

        if (currentState != newState)//update State
        {
            UpdateState(newState);
            UpdateLevel(newLevel);
        }
        else
        {
            if (currentLevel != newLevel)
                UpdateLevel(newLevel);
        }
    }

    private void UpdateLevel(int newLevel)
    {
        Image[] levelImg = levelsParentPanel.GetComponentsInChildren<Image>();
        int length = levelImg.Length;
        for (int i = 0; i < length; i++)
        {
            if (i <= newLevel)
                levelImg[i].color = completeColor;
        }
        this.currentLevel = newLevel;
    }

    private void UpdateState(int newState)
    {
        State[] currentState = Resources.LoadAll<State>("States/").Where(s => s.id == newState.ToString() || s.id == (newState + 1).ToString()).ToArray();

        if (currentState == null || currentState.Length == 0)
            return;

        if (currentState.Length == 2)
        {
            currentEnvironment.sprite = currentState[0].environmentSprite;
            nextEnvironment.sprite = currentState[1].environmentSprite;
        }
        else
            currentEnvironment.sprite = nextEnvironment.sprite = currentState[0].environmentSprite;

        foreach (Transform item in levelsParentPanel)
        {
            Destroy(item.gameObject);
        }

        int length = currentState[0].listLevel.Count;

        for (int i = 0; i < length; i++)
        {
            Image image = Instantiate(levelImgPrefab, Vector2.zero, Quaternion.identity, levelsParentPanel.transform).GetComponent<Image>();
            if (i == 0)
                image.color = completeColor;
        }
        environmentBackground.sprite = currentState[0].environmentBackground;
        Helper.mainCam.backgroundColor = currentState[0].environmentColor;
        this.currentState = newState;
    }
}

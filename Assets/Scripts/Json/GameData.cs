using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public bool sound;
    public bool vibrate;
    public int currentState;
    public int currentLevel;
    public List<UnitDataJson> listUnits;
    public GameData()
    {
        this.sound = this.vibrate = true;
        this.currentState = this.currentLevel = 0;
        this.listUnits = new List<UnitDataJson>();
    }

    public GameData(bool sound, bool vibrate, int currentState, int currentLevel, List<UnitDataJson> listUnits)
    {
        this.sound = sound;
        this.vibrate = vibrate;
        this.currentState = currentState;
        this.currentLevel = currentLevel;
        this.listUnits = listUnits;
    }

    public void SetLevel(int level)
    {
        this.currentLevel = level;
        DataManager.Instance.CreateData();
    }
    public void SetState(int state)
    {
        this.currentState = state;
        this.currentLevel = 0;
        DataManager.Instance.CreateData();
    }
    public void NextLevel()
    {
        this.currentLevel += 1;
        DataManager.Instance.CreateData();
    }
    public void NextState()
    {
        this.currentState += 1;
        this.currentLevel = 0;
        DataManager.Instance.CreateData();
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public int GetCurrentState()
    {
        return currentState;
    }
}

[Serializable]
public class UnitDataJson
{
    public string nameUnit;
    public float px, py;
    public string unitTag;

    public UnitDataJson()
    {
    }

    public UnitDataJson(string nameUnit, float px, float py, string unitTag)
    {
        this.nameUnit = nameUnit;
        this.px = px;
        this.py = py;
        this.unitTag = unitTag;
    }
}
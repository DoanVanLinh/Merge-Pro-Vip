using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class GameData
{
    public int currentCoins;
    public int currentCostBuyUnit;
    public bool sound;
    public bool vibrate;
    public int currentState;
    public int currentLevel;
    public List<UnitDataJson> listUnits;
    public GameData()
    {
        this.sound = this.vibrate = true;
        this.currentState = this.currentLevel = 0;
        currentCoins = 100;
        currentCostBuyUnit = 100;

        this.listUnits = new List<UnitDataJson>();
    }

    public GameData(bool sound, bool vibrate, int currentState, int currentLevel, int currentCoins,int currentCostBuy, List<UnitDataJson> listUnits)
    {
        this.sound = sound;
        this.vibrate = vibrate;
        this.currentState = currentState;
        this.currentLevel = currentLevel;
        this.currentCostBuyUnit = currentCostBuy;
        this.currentCoins = currentCoins;
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
        State currentState = Resources.LoadAll<State>("States/").Where(s => s.id == this.currentState.ToString()).FirstOrDefault();
        Level currentLevel = currentState.listLevel.Where(l => l.id == this.currentLevel.ToString()).FirstOrDefault();

        if (currentLevel == null)
            NextState();
        else
        {
            DataManager.Instance.CreateData();
        }
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

    public void AddCoins(int coins)
    {
        this.currentCoins += coins;
        DataManager.Instance.CreateData();
    }
    public void SetCurrentCostBuyUnit(int coins)
    {
        this.currentCostBuyUnit = coins;
        DataManager.Instance.CreateData();
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
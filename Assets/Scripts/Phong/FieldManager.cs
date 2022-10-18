using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using WE.Unit;
using System;
using System.Linq;

[System.Serializable]
public static class FieldManager
{
    public static Action OnUnitRemove;

    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldPlayer = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldFirePlayer = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldGrassPlayer = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldWaterPlayer = new Dictionary<Vector2, BaseUnit>();

    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldEnemy = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldFireEnemy = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldGrassEnemy = new Dictionary<Vector2, BaseUnit>();
    [ShowInInspector]
    public static Dictionary<Vector2, BaseUnit> fieldWaterEnemy = new Dictionary<Vector2, BaseUnit>();
    public static void AddToField(Vector2 loc, BaseUnit unit)
    {
        if (unit == null)
        {
            fieldPlayer[loc] = unit;
            return;
        }

        switch (unit.tag)
        {
            case Helper.PLAYER_UNIT_TAG:
                fieldPlayer[loc] = unit;
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFirePlayer[loc] = unit;
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterPlayer[loc] = unit;
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassPlayer[loc] = unit;
                        break;
                    default:
                        break;
                }
                break;
            case Helper.ENEMY_UNIT_TAG:
                fieldEnemy[loc] = unit;
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFireEnemy[loc] = unit;
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterEnemy[loc] = unit;
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassEnemy[loc] = unit;
                        break;
                    default:
                        break;
                }
                break;
        }
    }
    public static void RemoveFromField(Vector2 loc, BaseUnit unit)
    {
        if (unit == null)
            return;

        switch (unit.tag)
        {
            case Helper.PLAYER_UNIT_TAG:
                fieldPlayer[fieldPlayer.Where(u => u.Value == unit).FirstOrDefault().Key] = null;
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFirePlayer.Remove(fieldFirePlayer.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterPlayer.Remove(fieldWaterPlayer.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassPlayer.Remove(fieldGrassPlayer.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    default:
                        break;
                }
                break;
            case Helper.ENEMY_UNIT_TAG:
                fieldEnemy[fieldEnemy.Where(u => u.Value == unit).FirstOrDefault().Key] = null;
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFireEnemy.Remove(fieldFireEnemy.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterEnemy.Remove(fieldWaterEnemy.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassEnemy.Remove(fieldGrassEnemy.Where(u => u.Value == unit).FirstOrDefault().Key);
                        break;
                    default:
                        break;
                }
                break;
        }

        OnUnitRemove?.Invoke();
    }

    public static FightStatus EndFight()
    {
        if (fieldPlayer.Where(u => u.Value != null).Count() == 0)
            return FightStatus.Lose;
        if (fieldEnemy.Where(u => u.Value != null).Count() == 0)
            return FightStatus.Win;

        return FightStatus.Null;
    }

    public static void StartFight()
    {
        foreach (var item in fieldPlayer)
        {
            if (item.Value == null)
                continue;
            GridManager.Instance.UpdateGridNode(item.Value.transform.position, false);
        }
        foreach (var item in fieldEnemy)
        {
            if (item.Value == null)
                continue; 
            GridManager.Instance.UpdateGridNode(item.Value.transform.position, false);
        }

        foreach (var item in fieldPlayer)
        {
            if (item.Value == null)
                continue;

            item.Value.StartAction();
        }

        foreach (var item in fieldEnemy)
        {
            if (item.Value == null)
                continue;

            item.Value.StartAction();
        }
    }
}
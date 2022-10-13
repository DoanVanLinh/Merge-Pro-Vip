using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using WE.Unit;
using System;
using System.Linq;


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
        switch (unit.tag)
        {
            case Helper.PLAYER_UNIT_TAG:
                fieldPlayer.Add(loc, unit);
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFirePlayer.Add(loc, unit);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterPlayer.Add(loc, unit);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassPlayer.Add(loc, unit);
                        break;
                    default:
                        break;
                }
                break;
            case Helper.ENEMY_UNIT_TAG:
                fieldEnemy.Add(loc, unit);
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFireEnemy.Add(loc, unit);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterEnemy.Add(loc, unit);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassEnemy.Add(loc, unit);
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
                fieldPlayer.Remove(fieldPlayer.Where(u=>u.Value == unit).FirstOrDefault().Key);
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
                fieldEnemy.Remove(fieldEnemy.Where(u => u.Value == unit).FirstOrDefault().Key);
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
}
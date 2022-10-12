using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using WE.Unit;
public static class FieldManager
{

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
                fieldPlayer.Remove(loc);
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFirePlayer.Remove(loc);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterPlayer.Remove(loc);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassPlayer.Remove(loc);
                        break;
                    default:
                        break;
                }
                break;
            case Helper.ENEMY_UNIT_TAG:
                fieldEnemy.Remove(loc);
                switch (unit.UnitType)
                {
                    case NameTypeUnit.Fire:
                        fieldFireEnemy.Remove(loc);
                        break;
                    case NameTypeUnit.Water:
                        fieldWaterEnemy.Remove(loc);
                        break;
                    case NameTypeUnit.Grass:
                        fieldGrassEnemy.Remove(loc);
                        break;
                    default:
                        break;
                }
                break;
        }

    }
}
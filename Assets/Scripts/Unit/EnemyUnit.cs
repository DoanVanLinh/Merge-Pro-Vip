using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Dragable))]
public class EnemyUnit : Unit
{
    public EnemyUnit(UnitData data, Vector2 loc)
    {
        GameObject clone = Instantiate(data.unitPrefab, loc, Quaternion.identity);
        Unit enemyUnit = clone.AddComponent<EnemyUnit>();
        enemyUnit.Data = data;
        enemyUnit.healthColor = Color.red;

        clone.tag = Helper.ENEMY_UNIT_TAG;
        clone.layer = Helper.ENEMY_TEAM_LAYER;
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Dragable))]
public class PlayerUnit : Unit
{
    public PlayerUnit(UnitData data,Vector2 loc)
    {
        GameObject clone = Instantiate(data.unitPrefab, loc, Quaternion.identity);

        Unit playerUnit = clone.AddComponent<PlayerUnit>();
        playerUnit.Data = data;
        playerUnit.healthColor = Color.green;

        clone.AddComponent<MergeController>();
        clone.tag = Helper.PLAYER_UNIT_TAG;
        clone.layer = Helper.PLAYER_TEAM_LAYER;
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }
}


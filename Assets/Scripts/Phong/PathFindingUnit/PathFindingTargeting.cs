using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WE.Unit.Target;
using WE.Unit;
public class PathFindingTargeting : BaseTargeting
{
    public override void GetTarget()
    {
        switch (Owner.tag)
        {
            case Helper.PLAYER_UNIT_TAG:
                Target = FieldManager.fieldEnemy.Where(u => u.Value != null && ((PathFindingUnit)u.Value).unitNode.Neighbors.Any(n => n.Walkable)).OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
                break;
            case Helper.ENEMY_UNIT_TAG:
                Target = FieldManager.fieldPlayer.Where(u => u.Value != null && ((PathFindingUnit)u.Value).unitNode.Neighbors.Any(n => n.Walkable)).OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
                break;
        }

        if (Target == null)
        {
            switch (Owner.tag)
            {
                case Helper.PLAYER_UNIT_TAG:
                    if (FieldManager.fieldEnemy.Where(u => u.Value != null).Count() == 0)
                        GameManager.Instance.SetFightStatus(FightStatus.Win);
                    return;
                case Helper.ENEMY_UNIT_TAG:
                    if (FieldManager.fieldPlayer.Where(u => u.Value != null).Count() == 0)
                        GameManager.Instance.SetFightStatus(FightStatus.Lose);
                    return;
            }
        }
        else
        {
            Target.OnUnitDie += OnUnitDie;
        }
        OnNewTarget?.Invoke();
    }
}

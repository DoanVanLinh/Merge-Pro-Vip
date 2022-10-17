using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WE.Unit.Target;
using WE.Unit;
public class PathFindingTargeting : BaseTargeting
{
    private BaseUnit cacheTarget;
    public override void GetTarget()
    {
        ((PathFindingMover)Owner.mover).OnStepDone -= OnStepDone;

        Owner.skeletonAnimation.SetUnitAni(Helper.IDLE_STATE_ANI, true);

        switch (Owner.tag)
        {
            case Helper.PLAYER_UNIT_TAG:
                Target = FieldManager.fieldEnemy.Where(u => u.Value != null && ((PathFindingUnit)u.Value).unitNode.Neighbors.Any(n => n == ((PathFindingUnit)Owner).unitNode || (n.Walkable && HasPath(n))))
                                                .OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position))
                                                .Select(u => u.Value)
                                                .FirstOrDefault();
                break;
            case Helper.ENEMY_UNIT_TAG:
                Target = FieldManager.fieldPlayer.Where(u => u.Value != null && ((PathFindingUnit)u.Value).unitNode.Neighbors.Any(n => n == ((PathFindingUnit)Owner).unitNode || (n.Walkable && HasPath(n))))
                                                 .OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position))
                                                 .Select(u => u.Value)
                                                 .FirstOrDefault();
                break;
        }
        if (Target != null)
        {
            ((PathFindingMover)Owner.mover).OnStepDone += OnStepDone;
            if (cacheTarget != Target)
                Target.OnUnitDie += OnTargetUnitDie;
            cacheTarget = Target;
            OnNewTarget?.Invoke();
        }
        else
        {
            GridManager.Instance.OnGridUpdate += OnGridUpdate;
        }
    }

    private void OnStepDone()
    {
        ((PathFindingMover)Owner.mover).OnStepDone -= OnStepDone;
        GetTarget();
    }
    private void OnGridUpdate()
    {
        GridManager.Instance.OnGridUpdate -= OnGridUpdate;
        if (Owner.Target == null)
        {
            Debug.Log("Grid Update" + Time.deltaTime, gameObject);
            GetTarget();
        }

    }
    public override void OnTargetUnitDie(BaseUnit unit)
    {
        unit.OnUnitDie -= OnTargetUnitDie;

        GameManager.Instance.SetFightStatus(FieldManager.EndFight());
        Owner.OnTargetDie();
        if (((PathFindingMover)Owner.mover).IsStepDone())
            GetTarget();
    }
    private bool HasPath(NodeBase targetNeighbor)
    {
        var path = Pathfinding.FindPath(((PathFindingUnit)Owner).unitNode, targetNeighbor);
        if (path == null || path.Count == 0)
            return false;
        return true;
    }
}

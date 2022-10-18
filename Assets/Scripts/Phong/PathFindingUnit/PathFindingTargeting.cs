using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WE.Unit.Target;
using WE.Unit;
using System.Collections;

public class PathFindingTargeting : BaseTargeting
{
    private BaseUnit cacheTarget;
    public override void GetTarget()
    {
        if (Owner == null)
            return;

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
            //GridManager.Instance.OnGridUpdate += OnGridUpdate;
            StartCoroutine(IEStandBy(1f));
        }
    }
    private bool isStandBy = false;

    IEnumerator IEStandBy(float timeDelay)
    {

        if (!isStandBy)
            isStandBy = true;
        else yield break;


        while (true)
        {
            if (Owner.Target == null)
            {
                yield return null;
                if (((PathFindingMover)Owner.mover).isStepDone)
                {
                    yield return new WaitForSeconds(timeDelay);
                    
                    GetTarget();
                }
            }
            else
            {
                isStandBy = false;
                yield break;
            }
        }
    }
    private void OnStepDone()
    {
        ((PathFindingMover)Owner.mover).OnStepDone -= OnStepDone;
        GetTarget();
    }
    public override void OnOwnerDie(BaseUnit o)
    {
        o.OnUnitDie -= OnTargetUnitDie;
        base.OnOwnerDie(o);
    }
    public override void OnTargetUnitDie(BaseUnit unit)
    {
        unit.OnUnitDie -= OnTargetUnitDie;

        GameManager.Instance.SetFightStatus(FieldManager.EndFight());
        Owner.OnTargetDie();
        if (((PathFindingMover)Owner.mover).isStepDone)
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

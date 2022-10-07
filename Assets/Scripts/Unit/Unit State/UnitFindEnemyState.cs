using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFindEnemyState : UnitBaseState
{
    public Unit Target { get; set; }

    public override void StartState(UnitStateManager unitStateManager)
    {
        FindEnemyNearly(unitStateManager);

        if (Target == null)
        {
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
            return;
        }
        else
        {
            unitStateManager.unitController.MoveToTarget(Target);
            if (unitStateManager.unitController.movement.isWait)
            {
                unitStateManager.SwitchState(unitStateManager.unitIdleState);
                return;
            }
            unitStateManager.SetUnitAni(Helper.MOVE_STATE_ANI, true);
        }
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        unitStateManager.unitController.sortingLayerUnit.Sorting();

        if (unitStateManager.unitController.movement.isWait)
        {
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
            return;
        }

        if (Target.movement.unitNode.Neighbors.Any(n => n.Walkable) && !Target.movement.unitNode.Neighbors.Contains(unitStateManager.unitController.movement.unitNode))
            FindEnemyNearly(unitStateManager);

        if (Target == null)
        {
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
            return;
        }

        if (unitStateManager.unitController.MoveToTarget(Target))
        {
            unitStateManager.SwitchState(unitStateManager.unitAttackState);
            return;
        }
    }
    void FindEnemyNearly(UnitStateManager unitStateManager)
    {
        if (Target == null || !Target.movement.unitNode.Neighbors.Contains(unitStateManager.unitController.movement.unitNode))
            //Target = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag) && u.Value.standPoints.Where(s => s.Value == null).Count() != 0).OrderBy(o => Vector2.Distance(unitStateManager.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
            Target = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag) && u.Value.movement.unitNode.Neighbors.Any(n => n.Walkable)).OrderBy(o => Vector2.Distance(unitStateManager.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
    }
}

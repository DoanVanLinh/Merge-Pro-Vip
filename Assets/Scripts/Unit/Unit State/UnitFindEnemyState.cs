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
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        if (unitStateManager.transform.position == new Vector3(3, 3, 0))
            Debug.Log("test");

        if (!Target.standPoints.ContainsValue(unitStateManager.unitController) && Target.standPoints.Where(s => s.Value == null).Count() == 0)
            FindEnemyNearly(unitStateManager);

        if (unitStateManager.unitController.MoveToTarget(Target))
            unitStateManager.SwitchState(unitStateManager.unitAttackState);
    }
    void FindEnemyNearly(UnitStateManager unitStateManager)
    {
        if (Target == null || !Target.standPoints.ContainsValue(unitStateManager.unitController))
            Target = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag) && u.Value.standPoints.Where(s => s.Value == null).Count() != 0).OrderBy(o => Vector2.Distance(unitStateManager.transform.position, o.Key)).Select(u => u.Value).FirstOrDefault();

        if (Target == null)
        {
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
        }
        else
            unitStateManager.SetUnitAni(Helper.MOVE_STATE_ANI, true);
    }
}

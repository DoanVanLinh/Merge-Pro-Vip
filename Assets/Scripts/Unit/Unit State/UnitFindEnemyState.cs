using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFindEnemyState : UnitBaseState
{
    public Unit Target { get; set; }

    public override void StartState(UnitStateManager unitStateManager)
    {
        Target = FindEnemyNearly(unitStateManager);
        unitStateManager.SetUnitAni(Helper.MOVE_STATE_ANI, true);
        if (Target == null)
        {
            GameManager.Instance.SetFightStatus(unitStateManager.CompareTag(Helper.ENEMY_UNIT_TAG)?FightStatus.Lose:FightStatus.Win);
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
        }
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        if (unitStateManager.unitController.MoveToTarget(Target))
            unitStateManager.SwitchState(unitStateManager.unitAttackState);
    }
    Unit FindEnemyNearly(UnitStateManager unitStateManager)
    {
        return GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag)).OrderBy(o => Vector2.Distance(unitStateManager.transform.position, o.Key)).Select(u => u.Value).FirstOrDefault();
    }
}

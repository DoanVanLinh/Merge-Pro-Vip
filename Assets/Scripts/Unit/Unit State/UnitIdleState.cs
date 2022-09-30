using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UnitIdleState : UnitBaseState
{
    public override void StartState(UnitStateManager unitStateManager)
    {
        unitStateManager.SetUnitAni(Helper.IDLE_STATE_ANI, true);
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        if (GameManager.Instance.isStart)//start Fight
        {
            Unit anyOtherUnit = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag)).Select(u => u.Value).FirstOrDefault();

            Unit anyOtherTarget = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag) && u.Value.standPoints.Where(s => s.Value == null).Count() != 0).Select(u => u.Value).FirstOrDefault();

            if (anyOtherUnit == null)
                GameManager.Instance.SetFightStatus(unitStateManager.CompareTag(Helper.ENEMY_UNIT_TAG) ? FightStatus.Lose : FightStatus.Win);

            if (anyOtherTarget != null)
            {
                unitStateManager.unitController.SetDefaulLoc();
                unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
            }
        }
    }
}

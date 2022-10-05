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
        if (GameManager.Instance.isStart && GameManager.Instance.GetFightStatus() == FightStatus.Null)//start Fight
        {
            Unit anyOtherUnit = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag)).Select(u => u.Value).FirstOrDefault();

            Unit anyOtherTarget = GameManager.fields.Where(u => u.Value != null && !u.Value.gameObject.CompareTag(unitStateManager.tag) && u.Value.movement.unitNode.Neighbors.Any(n => n.Walkable)).Select(u => u.Value).FirstOrDefault();

            if (anyOtherUnit == null)//No enemy 
            {
                GameManager.Instance.SetFightStatus(unitStateManager.CompareTag(Helper.ENEMY_UNIT_TAG) ? FightStatus.Lose : FightStatus.Win);
            }
            else
            {

                if (anyOtherTarget != null)//Start
                {
                    unitStateManager.unitController.SetDefaulLoc();
                    unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
                }
            }
        }
    }
}

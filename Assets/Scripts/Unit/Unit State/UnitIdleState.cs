using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            unitStateManager.unitController.SetDefaulLoc();
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private Unit target;
    public override void StartState(UnitStateManager unitStateManager)
    {
        target = unitStateManager.unitFindEnemyState.Target;
        unitStateManager.SetUnitAni(Helper.ATTACK_STATE_ANI, true, unitStateManager.unitController.CurrentAttackRate);
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
       
        Attack(unitStateManager);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            unitStateManager.SwitchState(unitStateManager.unitDieState);
    }

    void Attack(UnitStateManager unitStateManager)
    {
        if (target == null)
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);

        unitStateManager.unitController.Attack(target, unitStateManager.unitAni.Skeleton.Data.FindAnimation("Attack").Duration);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private Unit target;
    UnitStateManager unitStateManager;
    public override void StartState(UnitStateManager unitStateManager)
    {
        target = unitStateManager.unitFindEnemyState.Target;
        unitStateManager.SetUnitAni(Helper.ATTACK_STATE_ANI, true, unitStateManager.unitController.CurrentAttackRate);
        unitStateManager.unitAni.state.Event += State_Event;
        this.unitStateManager = unitStateManager;
    }

    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.ToLower() == "attack")
        {
            Attack(this.unitStateManager);
            if (unitStateManager.unitController.CurrentAttackDame == 4)
                Debug.LogError("Attack");
        }
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        if (target == null)
        {
            unitStateManager.unitAni.state.Event -= State_Event;
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
            unitStateManager.SwitchState(unitStateManager.unitDieState);
    }

    void Attack(UnitStateManager unitStateManager)
    {
        if (target == null)
        {
            unitStateManager.unitAni.state.Event -= State_Event;
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
        }
        unitStateManager.unitController.Attack(target, unitStateManager.unitAni.Skeleton.Data.FindAnimation("Attack").Duration);
    }
}

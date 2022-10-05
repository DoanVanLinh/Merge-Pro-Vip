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
        //if (!unitStateManager.unitController.movement.IsNeighborOfTarget())
        //{
        //    unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
        //    return;
        //}

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
        }
    }
    public void UnSubcribeEvent()
    {
        unitStateManager.unitAni.state.Event -= State_Event;
    }
    public override void UpdateState(UnitStateManager unitStateManager)
    {
        if (target == null)
        {
            unitStateManager.unitAni.state.Event -= State_Event;
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
            return;
        }

        if (!unitStateManager.unitController.movement.IsNeighborOfTarget())
        {
            unitStateManager.unitAni.state.Event -= State_Event;
            unitStateManager.SwitchState(unitStateManager.unitIdleState);
            return;
        }
    }

    void Attack(UnitStateManager unitStateManager)
    {
        if (target == null)
        {
            unitStateManager.unitAni.state.Event -= State_Event;
            unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
            return;
        }
        unitStateManager.unitController.Attack(target, unitStateManager.unitAni.Skeleton.Data.FindAnimation("Attack").Duration);
    }
}

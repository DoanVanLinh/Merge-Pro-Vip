using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDieState : UnitBaseState
{
    public override void StartState(UnitStateManager unitStateManager)
    {
        Debug.Log("Start Die State", unitStateManager.gameObject);
        unitStateManager.SetUnitAni(Helper.DEAD_STATE_ANI, false);
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {

    }
}

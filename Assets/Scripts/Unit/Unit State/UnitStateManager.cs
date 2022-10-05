using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitStateManager : MonoBehaviour
{
    public UnitBaseState currentState;

    public UnitIdleState unitIdleState = new UnitIdleState();
    public UnitFindEnemyState unitFindEnemyState = new UnitFindEnemyState();
    public UnitAttackState unitAttackState = new UnitAttackState();
    public UnitDieState unitDieState = new UnitDieState();

    public Unit unitController;
    public SkeletonAnimation unitAni;

    void Start()
    {
        unitAni = GetComponent<SkeletonAnimation>();
        currentState = unitIdleState;

        currentState.StartState(this);
    }
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(UnitBaseState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        currentState.StartState(this);
    }
    public void SetUnitAni(string name, bool loop, float timeScale = 1f)
    {
        if (unitAni.AnimationName == name)
            return;

        unitAni.state.SetAnimation(0, name, loop).TimeScale = timeScale;

    }

    public void DestroyUnit(float afterTime = 0f)
    { Destroy(gameObject, afterTime); }
}


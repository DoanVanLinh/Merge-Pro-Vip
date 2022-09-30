using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitStateManager : MonoBehaviour
{
    [SerializeField] private UnitBaseState currentState;

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
        currentState = newState;
        currentState.StartState(this);
    }
    public void SetUnitAni(string name, bool loop, float timeScale = 1f)
    {
        unitAni.state.SetAnimation(0, name, loop).TimeScale = timeScale;

    }

    public void DestroyUnit(float afterTime = 0f)
    { Destroy(gameObject, afterTime); }
}


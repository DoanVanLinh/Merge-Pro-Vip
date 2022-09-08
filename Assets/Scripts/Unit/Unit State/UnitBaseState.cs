using UnityEngine;
public abstract class UnitBaseState
{
    public abstract void StartState(UnitStateManager unitStateManager);
    public abstract void UpdateState(UnitStateManager unitStateManager);
}

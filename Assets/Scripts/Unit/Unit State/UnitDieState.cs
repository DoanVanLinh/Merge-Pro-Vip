using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class UnitDieState : UnitBaseState
{
    UnitStateManager unitStateManager;
    float fade = 1f;
    Material material;
    public override void StartState(UnitStateManager unitStateManager)
    {
        this.unitStateManager = unitStateManager;
        //unitStateManager.SetUnitAni(Helper.DEAD_STATE_ANI, false);
        //unitStateManager.unitAni.state.Complete += State_Complete;
        unitStateManager.GetComponent<SkeletonAnimation>().enabled = false;
        material = unitStateManager.GetComponent<MeshRenderer>().material;

        DOTween.To(() => fade, x => fade = x, 0f, 1f);
        unitStateManager.DestroyUnit(1f);
    }
    //private void State_Complete(Spine.TrackEntry trackEntry)
    //{
    //    unitStateManager.unitAni.state.Complete -= State_Complete;
    //    if (unitStateManager.unitAni.state.ToString().Equals("Dead"))
    //    {
    //        unitStateManager.DestroyUnit();
    //    }
    //}
    public override void UpdateState(UnitStateManager unitStateManager)
    {
        material.SetFloat("_Fade", fade);
    }
}

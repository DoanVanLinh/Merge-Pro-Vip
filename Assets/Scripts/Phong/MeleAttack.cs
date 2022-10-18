using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace WE.Unit.Attack
{
    public class MeleAttack : BaseAttack
    {
        public override void ExcuteAttack()
        {
            if (Owner.skeletonAnimation.OnStateEvent == null)
                Owner.skeletonAnimation.OnStateEvent += OnAttackEvent;
            Owner.attacker.OnAttackDone += CheckUnitAfterAttack;
        }

        private void CheckUnitAfterAttack()
        {
            Owner.attacker.OnAttackDone -= CheckUnitAfterAttack;
            if (Owner.Target == null || !Owner.IsOnAttackRange())
            {
                Owner.targeter.GetTarget();
                Stop();
            }
        }

        private void OnAttackEvent()
        {
            Owner.skeletonAnimation.OnStateEvent -= OnAttackEvent;
            if (Owner.Target != null)
                Owner.Target.TakeDamage(Owner.CurrentDamage, Owner);
        }
        public override void Stop()
        {
            Owner.skeletonAnimation.OnStateEvent -= OnAttackEvent;
            Owner.attacker.OnAttackDone -= CheckUnitAfterAttack;
            base.Stop();
        }
    }
}

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
                Owner.skeletonAnimation.OnStateEvent += OnAttack;
            Owner.Target.OnUnitDie += OnTargetDie;
        }

        private void OnTargetDie(BaseUnit obj)
        {
            Stop();
        }

        private void OnAttack()
        {
            Owner.skeletonAnimation.OnStateEvent -= OnAttack;
            if (Owner.Target != null)
                Owner.Target.TakeDamage(Owner.CurrentDamage, Owner);
            else
                Stop();
        }
        public override void Stop()
        {
            Owner.Target.OnUnitDie -= OnTargetDie;
            Owner.skeletonAnimation.OnStateEvent -= OnAttack;
            base.Stop();
        }
    }
}

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
            Owner.skeletonAnimation.OnStateEvent += OnAttack;
        }

        private void OnAttack()
        {
            Owner.skeletonAnimation.OnStateEvent -= OnAttack;
            Owner.Target.TakeDamage(Owner.CurrentDamage, Owner);
        }
    }
}

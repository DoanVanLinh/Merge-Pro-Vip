using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WE.Unit.Skill
{
    public class SkillBasher : BaseSkill
    {
        public override void Init(BaseUnit _owner)
        {
            base.Init(_owner);
            onManaFull += OnManaFull;
        }

        private void OnManaFull()
        {
            StartCoroutine(WaitToExcute());
        }

        IEnumerator WaitToExcute()
        {
            yield return new WaitUntil(() => ((PathFindingMover)Owner.mover).isMoveDone);
            ExcuteSkill();
            isExcutedSkill = true;
        }

        public override void ExcuteSkill()
        {
            if (Owner.Target == null || Owner == null)
                return;

            StunEffect effect = Owner.Target.AddEffect<StunEffect>();
            effect.ExcuteEffect(Owner, Owner.Target, 0, 2f);
            Owner.Target.TakeDamage(0, Owner);
        }
    }

}

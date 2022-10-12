using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WE.Unit.Skill
{
    public class SkillBasher : BaseSkill
    {
        public override void ExcuteSkill()
        {
            Owner.Target.TakeDamage(0, Owner);
            StunEffect effect = Owner.Target.AddEffect<StunEffect>();
            effect.ExcuteEffect(Owner, Owner.Target);
        }
    }

}

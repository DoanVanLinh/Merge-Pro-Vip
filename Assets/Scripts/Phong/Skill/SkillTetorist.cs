using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public class SkillTetorist : BaseSkill
    {
        public override void ExcuteSkill()
        {
            Owner.mover.OnMoveDone += OnMoveDone;
            Owner.mover.MoveToPosition(Owner.Target.transform.position);
        }
        public void OnMoveDone()
        {
            Owner.mover.OnMoveDone -= OnMoveDone;
            Explose();
        }
        public void Explose()
        {
            Owner.Die();
        }
    }
}


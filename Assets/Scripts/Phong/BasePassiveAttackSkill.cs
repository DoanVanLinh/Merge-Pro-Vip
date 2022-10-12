using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public abstract class BasePassiveAttackSkill : BaseSkill
    {
        public int AttackCounter;
        public override void ExcuteSkill()
        {
            Owner.attacker.OnAttackDone += OnAttack;
        }
        public abstract void OnAttack();
    }
}


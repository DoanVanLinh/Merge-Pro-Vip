using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public class SkillCritAfterCounterAttack : BaseSkill
    {
        public int conditionAmount;

        private int counterAttack;
        public override void ExcuteSkill()
        {
            Owner.attacker.OnAttackDone += CounterAttack;
        }

        private void CounterAttack()
        {
            counterAttack++;
            if (counterAttack == conditionAmount - 1)
            {
                Owner.currentDamage.AddValuePercent(500);
            }
            if (counterAttack == conditionAmount)
            {
                Owner.currentDamage.AddValuePercent(-500);
                counterAttack = 0;
            }
        }
    }
}


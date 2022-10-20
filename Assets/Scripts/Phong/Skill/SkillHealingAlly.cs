using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public class SkillHealingAlly : BaseSkill
    {
        public float healingAmount;
        public override void ExcuteSkill()
        {
            switch (Owner.tag)
            {
                case Helper.PLAYER_UNIT_TAG:
                    foreach (var enemy in FieldManager.fieldPlayer)
                    {
                        if (enemy.Value == null)
                            continue;

                        enemy.Value.CurrentHp += healingAmount;
                        HealingEffect healingEffect = enemy.Value.AddEffect<HealingEffect>();
                        healingEffect.ExcuteEffect(Owner, enemy.Value, 0, 1f);
                    }
                    break;
                case Helper.ENEMY_UNIT_TAG:
                    foreach (var player in FieldManager.fieldEnemy)
                    {
                        if (player.Value == null)
                            continue;

                        player.Value.CurrentHp += healingAmount;
                        HealingEffect healingEffect = player.Value.AddEffect<HealingEffect>();
                        healingEffect.ExcuteEffect(Owner, player.Value, 0, 1f);
                    }
                    break;
            }
        }
        public override void Init(BaseUnit _owner)
        {
            base.Init(_owner);
            onManaFull += OnManaFull;
        }

        private void OnManaFull()
        {
            ExcuteSkill();
            isExcutedSkill = true;
        }
    }
}


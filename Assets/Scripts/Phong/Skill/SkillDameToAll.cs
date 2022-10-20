using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public class SkillDameToAll : BaseSkill
    {
        public override void ExcuteSkill()
        {
            switch (Owner.tag)
            {
                case Helper.PLAYER_UNIT_TAG:
                    foreach (var enemy in FieldManager.fieldEnemy)
                    {
                        if (enemy.Value == null)
                            continue;

                        enemy.Value.TakeDamage(Owner.CurrentDamage, Owner);
                        LightingEffect lightingEffect = enemy.Value.AddEffect<LightingEffect>();
                        lightingEffect.ExcuteEffect(Owner, enemy.Value, 0, 1f);
                    }
                    break;
                case Helper.ENEMY_UNIT_TAG:
                    foreach (var player in FieldManager.fieldPlayer)
                    {
                        if (player.Value == null)
                            continue;

                        player.Value.TakeDamage(Owner.CurrentDamage, Owner);
                        LightingEffect lightingEffect = player.Value.AddEffect<LightingEffect>();
                        lightingEffect.ExcuteEffect(Owner, player.Value, 0, 1f);
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


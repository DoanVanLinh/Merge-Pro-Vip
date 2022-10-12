using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public class SkillAtributes
    {
        public float DamageMultiple;
        public float RangeSkill;
    }
    public abstract class BaseSkill : MonoBehaviour
    {
        public BaseUnit Owner;
        public SkillAtributes skillAtribute;

        protected float maxMana => Owner.MaxMp;
        protected float manaRegen => Owner.CurrentManaRegen;
        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
            if (maxMana > 0)
            {
                StartCoroutine(IERegen());
            }
            else
            {
                ExcuteSkill();
            }
        }
        protected virtual IEnumerator IERegen()
        {
            while (Owner.IsAlive)
            {
                yield return new WaitForSeconds(1);
                Owner.CurrentMp += manaRegen;
                if (Owner.CurrentMp >= maxMana)
                {
                    Owner.CurrentMp -= maxMana;
                    ExcuteSkill();
                }
            }
        }
        public abstract void ExcuteSkill();
    }
}


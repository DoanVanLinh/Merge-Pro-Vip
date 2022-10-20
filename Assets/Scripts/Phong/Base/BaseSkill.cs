using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public Action onManaFull;
        public bool isExcutedSkill;
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
            yield return new WaitUntil(() => GameManager.Instance.isStart);
            while (Owner.IsAlive)
            {
                yield return new WaitForSeconds(1);
                Owner.CurrentMp += manaRegen;
                Owner.UpdateManaBar();

                if (Owner.CurrentMp >= maxMana)
                {
                    while(true)
                    {
                        yield return null;
                        onManaFull?.Invoke();
                        yield return new WaitUntil(()=>Owner.Target!=null);
                        yield return new WaitUntil(() => isExcutedSkill);
                        isExcutedSkill = false;
                        Owner.CurrentMp -= maxMana;
                        Owner.UpdateManaBar();
                        break;
                    }
                }
            }
        }
        public abstract void ExcuteSkill();
    }
}


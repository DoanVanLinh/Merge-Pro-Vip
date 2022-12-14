using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WE.Unit.Skill;
using WE.Unit.Target;

namespace WE.Unit.Attack
{
    public abstract class BaseAttack : MonoBehaviour
    {
        public BaseTargeting targeter;
        public BaseUnit Owner;
        public SkillAtributes skillAtribute;
        public System.Action OnAttackDone;
        float attackSpeed => Owner.CurrentAttackSpeed;
        private bool isAttackDone;
        public virtual void SetTargeter(BaseTargeting _targeter)
        {
            targeter = _targeter;
        }
        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
            isAttackDone = true;

        }
        public virtual void StartAttack()
        {
            StartCoroutine(IEAttack());
        }

        protected virtual IEnumerator IEAttack()
        {
            while (Owner.IsAlive)
            {
                yield return null;
                Owner.skeletonAnimation.SetUnitAni(Helper.ATTACK_STATE_ANI, false, attackSpeed);
                CallAttack();
                yield return new WaitForSeconds(Owner.skeletonAnimation.DurationAniState(Helper.ATTACK_STATE_ANI));
                OnAttackDone?.Invoke();
            }
        }

        protected virtual void CallAttack()
        {
            ExcuteAttack();
        }
        public abstract void ExcuteAttack();
        public virtual void Stop()
        {
            StopAllCoroutines();
        }
    }
}


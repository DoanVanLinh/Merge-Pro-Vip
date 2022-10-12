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
        public virtual void SetTargeter(BaseTargeting _targeter)
        {
            targeter = _targeter;
        }
        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
        }
        public virtual void StartAttack()
        {
            StartCoroutine(IEAttack());
        }
        protected virtual IEnumerator IEAttack()
        {
            while (Owner.IsAlive && targeter.Target != null)
            {
                CallAttack();
                yield return new WaitForSeconds(attackSpeed);

            }
        }
        protected virtual void CallAttack()
        {
            ExcuteAttack();
            OnAttackDone?.Invoke();
        }
        public abstract void ExcuteAttack();
    }
}


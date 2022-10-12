using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WE.Unit.Skill
{
    public enum EffectType
    {
        Buff,
        Debuff
    }
    public abstract class BaseEffect : MonoBehaviour
    {
        public BaseUnit Target;
        public BaseUnit Owner;
        public EffectType effectType;
        public float valueEffect;
        public float durationEffect;
        //public AnimationEffect fxEffect;
        protected virtual void OnEnable()
        {
            PlayFx();
        }
        public virtual void PlayFx()
        {

        }
        public virtual void ExcuteEffect(BaseUnit owner, BaseUnit target,float _valueEffect = 0, float _duration = 0)
        {
            Owner = owner;
            Target = target;
            valueEffect = _valueEffect;
            durationEffect = _duration;
            if (durationEffect > 0 )
            {
                StartCoroutine(IEExcuteEffect());
            }
            else
            {
                StartEffect();
            }
        }
        public virtual IEnumerator IEExcuteEffect()
        {
            StartEffect();
            yield return new WaitForSeconds(durationEffect);
            StopEffect();
        }
        public abstract void StartEffect();
        public virtual void StopEffect()
        {
            Destroy(this);
        }
    }
}


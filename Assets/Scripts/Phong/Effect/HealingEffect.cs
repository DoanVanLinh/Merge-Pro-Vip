using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WE.Unit.Skill
{
    public class HealingEffect : BaseEffect
    {
        private GameObject effectPrefab;
        public override void PlayFx()
        {
            effectPrefab = Instantiate(Resources.Load<GameObject>("Effect Skill/Healing Effect"), Vector2.zero, Quaternion.identity, transform);
            effectPrefab.transform.localPosition = Vector2.zero;
        }
        public override void StartEffect()
        {
        }
        public override void StopEffect()
        {
            Destroy(effectPrefab);
        }
    }
}


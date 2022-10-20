using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WE.Unit.Skill
{
    public class StunEffect : BaseEffect
    {
        private GameObject effectPrefab;
        public override void PlayFx()
        {
            effectPrefab = Instantiate(Resources.Load<GameObject>("Effect Skill/Stun Effect"), Vector2.zero, Quaternion.identity, transform);
            effectPrefab.transform.localPosition = Vector2.zero;
        }
        public override void StartEffect()
        {
            Target.Stop();
        }

        public override void StopEffect()
        {
            Target.Resume();
            Destroy(effectPrefab);
        }
    }
}


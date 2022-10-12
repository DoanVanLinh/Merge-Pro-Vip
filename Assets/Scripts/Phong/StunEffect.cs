using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WE.Unit.Skill
{
    public class StunEffect : BaseEffect
    {
        public override void StartEffect()
        {
            Target.Stop();
        }

        public override void StopEffect()
        {
            Target.Resume();
        }
    }
}


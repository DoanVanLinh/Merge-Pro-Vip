using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

namespace WE.Unit.Animation
{
    public class BaseSkeletonAnimation : MonoBehaviour
    {
        public SkeletonAnimation unitSkeletonAnimation;
        public Action OnStateEvent;
        public Action OnStateStart;
        public Action<string> OnStateEnd;
        public Action<string> OnStateComplete;
        public BaseUnit Owner;

        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
            unitSkeletonAnimation.ClearState();
            unitSkeletonAnimation.skeletonDataAsset = Owner.unitStats.skeletonData;
            unitSkeletonAnimation.Initialize(true);
        }

        public void SetUnitAni(string name, bool loop, float timeScale = 1f)
        {
            if (name != Helper.ATTACK_STATE_ANI)
                if (unitSkeletonAnimation.AnimationName == name)
                    return;

            unitSkeletonAnimation.state.SetAnimation(0, name, loop).TimeScale = timeScale;
            unitSkeletonAnimation.state.Event += State_Event;
            unitSkeletonAnimation.state.Start += State_Start;
            unitSkeletonAnimation.state.End += State_End;
            unitSkeletonAnimation.state.Complete += State_Complete;
        }

        private void State_Complete(Spine.TrackEntry trackEntry)
        {
            unitSkeletonAnimation.state.Complete -= State_Complete;
            OnStateComplete?.Invoke(unitSkeletonAnimation.AnimationName);
        }

        protected virtual void State_End(Spine.TrackEntry trackEntry)
        {
            unitSkeletonAnimation.state.End -= State_End;
            OnStateEnd?.Invoke(unitSkeletonAnimation.AnimationName);
        }

        protected void State_Start(Spine.TrackEntry trackEntry)
        {
            unitSkeletonAnimation.state.Start -= State_Start;
            OnStateStart?.Invoke();
        }

        protected void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name.ToLower() == Helper.ATTACK_STATE_ANI.ToLower())
            {
                OnStateEvent?.Invoke();
            }
        }
        public float DurationAniState(string nameAni)
        {
            return unitSkeletonAnimation.Skeleton.Data.FindAnimation(nameAni).Duration;
        }
    }
}

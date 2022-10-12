using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WE.Unit.Target
{
    public class BaseTargeting : MonoBehaviour
    {
        public BaseUnit Owner;

        public BaseUnit Target;
        public System.Action OnNewTarget;
        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
        }
        public virtual void GetTarget()
        {
            switch (Owner.tag)
            {
                case Helper.PLAYER_UNIT_TAG:
                    Target = FieldManager.fieldEnemy.Where(u => u.Value != null).OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
                    break;
                case Helper.ENEMY_UNIT_TAG:
                    Target = FieldManager.fieldPlayer.Where(u => u.Value != null).OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position)).Select(u => u.Value).FirstOrDefault();
                    break;
            }
            if (Target == null)
            {
                Owner.skeletonAnimation.SetUnitAni(Helper.IDLE_STATE_ANI, true);
                switch (Owner.tag)
                {
                    case Helper.PLAYER_UNIT_TAG:
                        if (FieldManager.fieldEnemy.Count == 0)
                            GameManager.Instance.SetFightStatus(FightStatus.Win);
                        return;
                    case Helper.ENEMY_UNIT_TAG:
                        if (FieldManager.fieldPlayer.Count == 0)
                            GameManager.Instance.SetFightStatus(FightStatus.Lose);
                        return;
                }
            }
            else
            {
                Target.OnUnitDie += OnUnitDie;
            }
            OnNewTarget?.Invoke();

        }
        public virtual void OnUnitDie(BaseUnit unit)
        {
            unit.OnUnitDie -= OnUnitDie;
            if (Owner != null)
            {
                Owner.skeletonAnimation.SetUnitAni(Helper.IDLE_STATE_ANI, true);
                GetTarget();
            }
        }
    }
}


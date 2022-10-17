using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
            Owner.OnUnitDie += OnOwnerDie;
        }
        public virtual void GetTarget()
        {
            Owner.skeletonAnimation.SetUnitAni(Helper.IDLE_STATE_ANI, true);
            switch (Owner.tag)
            {
                case Helper.PLAYER_UNIT_TAG:
                    Target = FieldManager.fieldEnemy.Where(u => u.Value != null)
                                                    .OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position))
                                                    .Select(u => u.Value)
                                                    .FirstOrDefault();
                    break;
                case Helper.ENEMY_UNIT_TAG:
                    Target = FieldManager.fieldPlayer.Where(u => u.Value != null)
                                                     .OrderBy(o => Vector2.Distance(Owner.transform.position, o.Value.transform.position))
                                                     .Select(u => u.Value)
                                                     .FirstOrDefault();
                    break;
            }
            if (Target != null)
            {
                Target.OnUnitDie += OnTargetUnitDie;
                OnNewTarget?.Invoke();
            }
            else
                GridManager.Instance.OnGridUpdate += FindNewTarget;
        }

        private void FindNewTarget()
        {
            GridManager.Instance.OnGridUpdate -= FindNewTarget;
            GetTarget();
        }

        public virtual void OnOwnerDie(BaseUnit o)
        {
            o.OnUnitDie -= OnOwnerDie;
            Target.OnUnitDie -= OnTargetUnitDie;
        }
        public virtual void OnTargetUnitDie(BaseUnit unit)
        {
            unit.OnUnitDie -= OnTargetUnitDie;

            GameManager.Instance.SetFightStatus(FieldManager.EndFight());
            Owner.OnTargetDie();
            if (gameObject.name == "GameObject (1)")
                Debug.Log("Unit Die" + Time.deltaTime);
            GetTarget();
        }
    }
}


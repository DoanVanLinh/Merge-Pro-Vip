using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WE.Unit.Move
{
    public class BaseMover : MonoBehaviour
    {
        public BaseUnit Owner;
        protected float moveSpeed => Owner.CurrentMoveSpeed;
        BaseUnit target => Owner.Target;
        public System.Action OnMoveDone;
        public virtual void Init(BaseUnit _owner)
        {
            Owner = _owner;
        }
        public virtual void MoveToAttackPosition()
        {
            if (moveSpeed == 0)
                return;

            if (GameManager.Instance.GetFightStatus() != FightStatus.Null)
                return;
            Stop();

            OnMoveDone?.Invoke();
        }
        public virtual void MoveToPosition(Vector3 pos)
        {
            if (Owner.transform.position == pos)
                OnMoveDone?.Invoke();
        }
        public virtual void Stop()
        {
            StopAllCoroutines();
            transform.DOKill();
        }
    }
}


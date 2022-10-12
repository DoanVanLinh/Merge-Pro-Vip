using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace WE.Unit.Attack
{
    public class DashAttack : BaseAttack
    {
        Vector2 cacheLoc;

        public override void ExcuteAttack()
        {
            cacheLoc = Owner.transform.position;
            StartCoroutine(IEDashAction());
        }
        IEnumerator IEDashAction()
        {
            if (Owner == null|| targeter.Target==null)
            {
                StopAllCoroutines();
                yield return null;
            }

            Owner.transform.DOMove(Owner.Target.transform.position, Owner.CurrentAttackSpeed * 1 / 2);
            yield return new WaitForSeconds(Owner.CurrentAttackSpeed * 1 / 2);
            targeter.Target?.TakeDamage(Owner.CurrentDamage, Owner);
            Owner.transform.DOMove(cacheLoc, Owner.CurrentAttackSpeed * 1 / 2);
            yield return new WaitForSeconds(Owner.CurrentAttackSpeed * 1 / 2);
        }
    }
}

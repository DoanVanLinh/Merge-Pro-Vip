using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill03 : SkillTest
{
    public float radius;
    public override void ExcuteSkill()
    {
        if (unitInfo.currentHp == 0)
        {
            Collider2D[] allCollider2Ds = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (var item in allCollider2Ds)
            {
                if (!item.CompareTag(unitInfo.tag))
                    item.GetComponent<UnitTest>().TakeDame(unitInfo.currentAttackDame);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
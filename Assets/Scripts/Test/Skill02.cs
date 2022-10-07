using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skill02 : SkillTest
{
    public override void ExcuteSkill()
    {
        StartCoroutine(IEDameToAllEnemy());
    }
    IEnumerator IEDameToAllEnemy()
    {
        yield return new WaitForSeconds(timeExcute);
        List<UnitTest> allEnemy = UnitManager.Instance.listUnit.Where(u => !u.CompareTag(unitInfo.tag)).ToList();

        foreach (UnitTest enemy in allEnemy)
        {
            enemy.TakeDame(unitInfo.currentAttackDame);
        }
    }
}
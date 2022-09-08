using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MergeController : MonoBehaviour
{
    private Unit unitInfo;
    private void OnEnable()
    {
        unitInfo = GetComponent<PlayerUnit>();
    }
    public bool Merge(Unit otherUnit)
    {
        if (otherUnit.NameUnit == unitInfo.NameUnit)//same type same tier
            return false;

        List<UnitData> allChild = unitInfo.Childs.Concat(otherUnit.Childs).ToList();

        var child = allChild.GroupBy(c => c).Where(g => g.Count() > 1).Select(g => g.Key).FirstOrDefault();

        if (child == null)
            return false;

        //GameObject unitClone = Instantiate(child.unitPrefab, otherUnit.transform.position, Quaternion.identity, GameManager.Instance.playerZone.transform);

        //unitClone.GetComponent<UnitInfo>().data = child;
        Unit newPlayerUnit = new PlayerUnit(child, otherUnit.transform.position);

        Destroy(gameObject);
        Destroy(otherUnit.gameObject);

        return true;
    }

}

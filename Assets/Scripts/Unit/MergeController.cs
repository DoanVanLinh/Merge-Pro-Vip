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

    public bool Split()
    {
        if (GameManager.fields.Where(u => u.Value == null).Count()<1)
            return false;

        UnitData[] parentUnit = GameManager.Instance.GetDataMerge().listUnits.Where(u => u.childs.Contains(unitInfo.Data)).ToArray();
        if (parentUnit.Length == 0)
            return false;

        GameManager.Instance.DeleteUnitInField(unitInfo.GetDefaulLoc());
        Destroy(gameObject);

        Vector2 loc = GameManager.fields.Where(u => u.Value == null).First().Key;

        Unit newPlayerUnit = new PlayerUnit(parentUnit[0], loc);
        loc = GameManager.fields.Where(u => u.Value == null).First().Key;
        Unit newPlayerUnit2 = new PlayerUnit(parentUnit[1], loc);

        return true;
    }

}

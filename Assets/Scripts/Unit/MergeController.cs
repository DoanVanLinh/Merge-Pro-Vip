using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using WE.Unit;

public class MergeController : MonoBehaviour
{
    private BaseUnit unitInfo;
    private void OnEnable()
    {
        unitInfo = GetComponent<BaseUnit>();
    }
    public bool Merge(BaseUnit otherUnit)
    {
        if (otherUnit.unitStats.unitName == unitInfo.unitStats.unitName)//same type same tier
            return false;

        List<UnitData> allChild = unitInfo.Childs.Concat(otherUnit.Childs).ToList();

        var child = allChild.GroupBy(c => c).Where(g => g.Count() > 1).Select(g => g.Key).FirstOrDefault();

        if (child == null)
            return false;

        GameManager.Instance.SpawnBaseUnit(otherUnit.transform.position, child, Helper.PLAYER_UNIT_TAG);

        if (!CPlayerPrefs.HasKey(child.unitName))
        {
            UIManager.Instance.newUnitPanel.LoadData(child);
            UIManager.Instance.newUnitPanel.gameObject.SetActive(true);
            CPlayerPrefs.SetBool(child.unitName, true);
        }

        Destroy(gameObject);
        Destroy(otherUnit.gameObject);

        return true;
    }

    public bool Split()
    {
        if (FieldManager.fieldPlayer.Where(u => u.Value == null).Count() < 1)
            return false;

        UnitData[] parentUnit = GameManager.Instance.GetDataMerge().listUnits.Where(u => u.childs.Contains(unitInfo.unitStats)).ToArray();
        if (parentUnit.Length == 0)
            return false;

        Destroy(gameObject);

        FieldManager.RemoveFromField(unitInfo.GetDefaulLoc(), unitInfo);

        Vector2 loc = FieldManager.fieldPlayer.Where(u => u.Value == null).First().Key;
        GameManager.Instance.SpawnBaseUnit(loc,parentUnit[0], Helper.PLAYER_UNIT_TAG);

        loc = FieldManager.fieldPlayer.Where(u => u.Value == null).First().Key;
        GameManager.Instance.SpawnBaseUnit(loc, parentUnit[1], Helper.PLAYER_UNIT_TAG);

        return true;
    }

}

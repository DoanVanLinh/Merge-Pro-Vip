using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/State", order = 1)]
public class State : ScriptableObject
{
    public string nameState;
    public string id;
    public Sprite environmentSprite;
    public Sprite environmentBackground;
    public Color environmentColor;
    public List<Level> listLevel;

    [Button("Get Data Level")]
    public void GetDataUnit()
    {
        listLevel = new List<Level>();
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRlVymujZxj9_USq05ZbHlbFbiALvFsJLDOwcMT9MbPvwQkVw5I7VFYKEBAsYWHhVd6sA3ZYNYgLbLv/pub?gid=114127507&single=true&output=csv";

        System.Action<string> actionComplete = new System.Action<string>((string str) =>
        {
            var data = CSVReader.ReadCSV(str);
            for (int i = 1; i < data.Count; i++)
            {
                var _data = data[i];

            //    UnitData unit = ScriptableObject.CreateInstance<UnitData>();

            //    unit.unitName = _data[0];
            //    unit.unitType = Helper.StringToEnum<NameTypeUnit>(_data[8]);
            //    unit.damage = int.Parse(_data[1]);
            //    unit.hp = int.Parse(_data[2]);
            //    unit.attackRange = Helper.ParseFloat(_data[3]);
            //    unit.attackSpeed = Helper.ParseFloat(_data[4]);
            //    unit.tier = int.Parse(_data[5]);
            //    unit.id = int.Parse(_data[6]);
            //    unit.colorTier = _data[7];
            //    unit.isJump = int.Parse(_data[10]) == 1;
            //    unit.moveSpeed = int.Parse(_data[11]);
            //    unit.unitPrefab = unitPrefab;
            //    unit.bullet = bulletPrefab;
            //    unit.skeletonData = UnitSkeletonDataManager.Instance.GetSkeletonData(unit.unitName);

            //    if (unit.attackRange != 1.5f)
            //    {
            //        string shotLoc = _data[9];
            //        unit.shotLoc = new Vector2(Helper.ParseFloat(shotLoc.Split(' ')[0]), Helper.ParseFloat(shotLoc.Split(' ')[1]));
            //    }
            //    else
            //    {
            //        unit.shotLoc = Vector2.zero;
            //    }


            //    unit.childs = new List<UnitData>();
            //    string[] childNames = dataMerges.Where(d => d.tier == unit.tier + 1).Where(d => d.id == unit.id || d.id == (unit.id - 1 == 0 ? dataMerges.Where(d => d.tier == unit.tier + 1).FirstOrDefault().id : unit.id - 1)).Select(d => d.unitName).ToArray();
            //    UnitData[] childs = listUnits.Where(u => childNames.Contains(u.unitName)).ToArray();
            //    unit.childs.AddRange(childs);

            //    AssetDatabase.CreateAsset(unit, "Assets/Prefabs/ScriptableObject/" + _data[0] + ".asset");

            //    AssetDatabase.SaveAssets();

            //    EditorUtility.FocusProjectWindow();

            //    Selection.activeObject = unit;

            //    listUnits.Add(unit);

            //    dataMerges.Add(new DataMerge(_data[0], int.Parse(_data[5]), int.Parse(_data[6])));
            }

            //listUnits.Reverse();

            UnityEditor.EditorUtility.SetDirty(this);
        });
        EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
    }


}

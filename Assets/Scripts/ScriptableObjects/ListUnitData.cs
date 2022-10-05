using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "List Unit Data", menuName = "ScriptableObjects/List Unit Data", order = 1)]
public class ListUnitData : ScriptableObject
{
    public GameObject unitPrefab;
    public GameObject bulletPrefab;
    public List<UnitData> listUnits = new List<UnitData>();
    List<DataMerge> dataMerges = new List<DataMerge>();

#if UNITY_EDITOR
    [Button("Get Data Unit")]
    public void GetDataUnit()
    {
        listUnits = new List<UnitData>();
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRlVymujZxj9_USq05ZbHlbFbiALvFsJLDOwcMT9MbPvwQkVw5I7VFYKEBAsYWHhVd6sA3ZYNYgLbLv/pub?gid=846980900&single=true&output=csv";

        System.Action<string> actionComplete = new System.Action<string>((string str) =>
        {
            var data = CSVReader.ReadCSV(str);
            for (int i = data.Count - 1; i > 0; i--)
            {
                var _data = data[i];

                UnitData unit = ScriptableObject.CreateInstance<UnitData>();

                unit.unitName = _data[0];
                unit.unitType = Helper.StringToEnum<NameTypeUnit>(_data[8]);
                unit.dame = int.Parse(_data[1]);
                unit.hp = int.Parse(_data[2]);
                unit.attackRange = Helper.ParseFloat(_data[3]);
                unit.attackRate = Helper.ParseFloat(_data[4]);
                unit.tier = int.Parse(_data[5]);
                unit.id = int.Parse(_data[6]);
                unit.colorTier = _data[7];
                unit.unitPrefab = unitPrefab;
                unit.bullet = bulletPrefab;
                unit.skeletonData = UnitSkeletonDataManager.Instance.GetSkeletonData(unit.unitName);

                if (unit.attackRange != 0.8f)
                {
                    string shotLoc = _data[9];
                    unit.shotLoc = new Vector2(Helper.ParseFloat(shotLoc.Split(' ')[0]), Helper.ParseFloat(shotLoc.Split(' ')[1]));
                }
                else
                {
                    unit.shotLoc = Vector2.zero;
                }


                unit.childs = new List<UnitData>();
                string[] childNames = dataMerges.Where(d => d.tier == unit.tier + 1).Where(d => d.id == unit.id || d.id == (unit.id - 1 == 0 ? dataMerges.Where(d => d.tier == unit.tier + 1).FirstOrDefault().id : unit.id - 1)).Select(d => d.unitName).ToArray();
                UnitData[] childs = listUnits.Where(u => childNames.Contains(u.unitName)).ToArray();
                unit.childs.AddRange(childs);

                AssetDatabase.CreateAsset(unit, "Assets/Prefabs/ScriptableObject/" + _data[0] + ".asset");

                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = unit;

                listUnits.Add(unit);

                dataMerges.Add(new DataMerge(_data[0], int.Parse(_data[5]), int.Parse(_data[6])));
            }

            //Merge Data
            //foreach (DataMerge dataMerge in dataMerges)
            //{
            //    UnitData unitData = listUnits.Where(u => u.unitName == dataMerge.unitName).FirstOrDefault();

            //    unitData.childs = new List<UnitData>();

            //    string[] childNames = dataMerges.Where(d => d.tier == dataMerge.tier + 1).Where(d => d.id == dataMerge.id || d.id == (dataMerge.id - 1 == 0 ? dataMerges.Where(d => d.tier == dataMerge.tier + 1).LastOrDefault().id : dataMerge.id - 1)).Select(d => d.unitName).ToArray();

            //    UnitData[] childs = listUnits.Where(u => childNames.Contains(u.unitName)).ToArray();

            //    unitData.childs.AddRange(childs);

            //    AssetDatabase.SaveAssets();

            //    //UnityEditor.EditorUtility.SetDirty(unitData);
            //}

            UnityEditor.EditorUtility.SetDirty(this);
        });
        EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
    }


    [Button("Get Data Merge")]
    public void GetDataMerge()
    {
        dataMerges = new List<DataMerge>();
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRlVymujZxj9_USq05ZbHlbFbiALvFsJLDOwcMT9MbPvwQkVw5I7VFYKEBAsYWHhVd6sA3ZYNYgLbLv/pub?gid=0&single=true&output=csv";

        System.Action<string> actionComplete = new System.Action<string>((string str) =>
        {
            var data = CSVReader.ReadCSV(str);
            for (int i = 1; i < data.Count; i++)
            {
                var _data = data[i];

                dataMerges.Add(new DataMerge(_data[0], int.Parse(_data[1]), int.Parse(_data[2])));
            }

            foreach (DataMerge dataMerge in dataMerges)
            {
                UnitData unitData = listUnits.Where(u => u.unitName == dataMerge.unitName).FirstOrDefault();

                unitData.childs = new List<UnitData>();

                string[] childNames = dataMerges.Where(d => d.tier == dataMerge.tier + 1).Where(d => d.id == dataMerge.id || d.id == (dataMerge.id - 1 == 0 ? dataMerges.Where(d => d.tier == dataMerge.tier + 1).LastOrDefault().id : dataMerge.id - 1)).Select(d => d.unitName).ToArray();

                UnitData[] childs = listUnits.Where(u => childNames.Contains(u.unitName)).ToArray();

                unitData.childs.AddRange(childs);

                //UnityEditor.EditorUtility.SetDirty(unitData);
            }


        });
        EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
    }
#endif
}
public class DataMerge
{
    public DataMerge(string unitName, int tier, int id)
    {
        this.unitName = unitName;
        this.tier = tier;
        this.id = id;
    }

    public string unitName;
    public int tier;
    public int id;
}

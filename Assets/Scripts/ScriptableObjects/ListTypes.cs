using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "ListTypes", menuName = "ScriptableObjects/ListTypes", order = 1)]
public class ListTypes : SerializedScriptableObject
{
    public Dictionary<NameTypeUnit, Dictionary<NameTypeUnit, float>> listTypes;

#if UNITY_EDITOR
    [Button("Get Unit Type Data")]
    public void GetUnitTypeData()
    {
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRlVymujZxj9_USq05ZbHlbFbiALvFsJLDOwcMT9MbPvwQkVw5I7VFYKEBAsYWHhVd6sA3ZYNYgLbLv/pub?gid=525898463&single=true&output=csv";
        listTypes = new Dictionary<NameTypeUnit, Dictionary<NameTypeUnit, float>>();

        System.Action<string> actionComplete = new System.Action<string>((string str) =>
        {
            var data = CSVReader.ReadCSV(str);
            for (int i = 1; i < data.Count; i++)
            {
                var _data = data[i];

                NameTypeUnit typeUnit = Helper.StringToEnum<NameTypeUnit>(_data[0]);

                Dictionary<NameTypeUnit, float> child = new Dictionary<NameTypeUnit, float>();
                for (int j = 1; j < data.Count; j++)
                {
                    NameTypeUnit nameTypeChild = Helper.StringToEnum<NameTypeUnit>(data[0][j]);
                    child.Add(nameTypeChild, Helper.ParseFloat(_data[j]));
                }
                listTypes.Add(typeUnit, child);
            }

            AssetDatabase.SaveAssets();
            UnityEditor.EditorUtility.SetDirty(this);
        });
        EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
    }

#endif
}

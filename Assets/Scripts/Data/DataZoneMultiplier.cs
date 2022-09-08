using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace WE.Data
{
    [CreateAssetMenu(fileName = "Data Zone Multiplier", menuName = "WE/Data/Data Zone Multiplier")]
    public class DataZoneMultiplier : SerializedScriptableObject
    {
        public Dictionary<int, ZoneMultiplier> ZoneMultiplierData;
#if UNITY_EDITOR
        [Button("Get Data")]
        public void GetData()
        {
            ZoneMultiplierData = new Dictionary<int, ZoneMultiplier>();
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTBO_e7NjDFNq1K56OO5JB19OFqI3VyA8vIW345_YL1QnanxaV0MNvCKJBcpkkWF9DziZOWzuQmXqTG/pub?gid=478127169&single=true&output=csv";
            System.Action<string> actionComplete = new System.Action<string>((string str) =>
            {
                var data = CSVReader.ReadCSV(str);
                for (int i = 1; i < data.Count; i++)
                {
                    var _data = data[i];
                    ZoneMultiplier config = new ZoneMultiplier();
                    config.DamageMultiplier = Helper.ParseFloat(_data[1]);
                    config.HpMultiplier = Helper.ParseFloat(_data[2]);
                    config.InZoneMultiplier = Helper.ParseFloat(_data[3]);
                    config.CoinMultiplier = Helper.ParseFloat(_data[4]);
                    ZoneMultiplierData.Add(i, config);
                }
                UnityEditor.EditorUtility.SetDirty(this);
            });
            EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
        }
#endif
    }
    [Serializable]
    public class ZoneMultiplier
    {
        public float DamageMultiplier;
        public float HpMultiplier;
        public float InZoneMultiplier;
        public float CoinMultiplier;
    }
}


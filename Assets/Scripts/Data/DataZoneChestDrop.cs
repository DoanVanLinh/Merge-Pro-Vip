using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace WE.Data
{
    [CreateAssetMenu(fileName = "Data Zone Chest Drop", menuName = "WE/Data/Data Zone Chest Drop")]
    public class DataZoneChestDrop : SerializedScriptableObject
    {
        public Dictionary<EZoneChestDrop, ChestDropItemConfig> ZoneChestDropData;
        public Dictionary<EZoneChestDrop, float> ZoneChestDropRate;
#if UNITY_EDITOR
        [Button("Get Data")]
        public void GetData()
        {
            ZoneChestDropData = new Dictionary<EZoneChestDrop, ChestDropItemConfig>();
            string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTBO_e7NjDFNq1K56OO5JB19OFqI3VyA8vIW345_YL1QnanxaV0MNvCKJBcpkkWF9DziZOWzuQmXqTG/pub?gid=1604362137&single=true&output=csv";
            System.Action<string> actionComplete = new System.Action<string>((string str) =>
            {
                var data = CSVReader.ReadCSV(str);
                for (int i = 1; i < data.Count; i++)
                {
                    var _data = data[i];
                    EZoneChestDrop eTpye = EZoneChestDrop.Small_Coin;
                    //Helper.TryToEnum<EZoneChestDrop>(_data[1], out eTpye);
                    ChestDropItemConfig config = new ChestDropItemConfig();
                    config.Quantity = Helper.ParseFloat(_data[2]);
                    config.Priority = Helper.ParseFloat(_data[3]);
                    ZoneChestDropData.Add(eTpye, config);
                    ZoneChestDropRate.Add(eTpye, config.Priority);
                }
                UnityEditor.EditorUtility.SetDirty(this);
            });
            EditorCoroutine.start(Helper.IELoadData(url, actionComplete));
        }
#endif
    }
    [Serializable]
    public class ChestDropItemConfig
    {
        public float Quantity;
        public float Priority;
    }
}


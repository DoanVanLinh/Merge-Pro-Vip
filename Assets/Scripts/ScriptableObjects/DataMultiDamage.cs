using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//[CreateAssetMenu(fileName = "DataMultiDamage", menuName = "ScriptableObjects/DataMultiDamage", order = 1)]
[System.Serializable]
public class DataMultiDamage
{
    [ShowInInspector]
    public Dictionary<NameTypeUnit, Dictionary<NameTypeUnit, float>> listTypeInteractDame;
}


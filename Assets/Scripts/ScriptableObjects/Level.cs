using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public string nameLevel;
    public string id;
    public List<UnitDataJson> listEnemy;
}

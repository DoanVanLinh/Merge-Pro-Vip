using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit", order = 1)]
public class UnitData : ScriptableObject
{
    public string nameUnit;
    public GameObject unitPrefab;
    public GameObject bullet;
    public int dame;
    public int hp;
    public float attackRange;
    public float attackRate;
    public float delayDame;
    public int tier;
    public string colorTier;
    public int id;
    public int level;
    public SkeletonDataAsset skeletonData;
    public List<UnitData> childs = new List<UnitData>();
}
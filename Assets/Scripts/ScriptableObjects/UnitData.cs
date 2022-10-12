using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit", order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName;
    public NameTypeUnit unitType;
    public GameObject unitPrefab;
    public GameObject bullet;
    public int hp;
    public int mp;
    public int damage;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public float manaRegen;
    public string colorTier;
    public int tier;
    public int id;
    public int level;
    public bool isJump;
    public Vector2 shotLoc;
    public SkeletonDataAsset skeletonData;
    public List<UnitData> childs = new List<UnitData>();
}
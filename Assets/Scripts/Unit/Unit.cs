using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Linq;
using System.Collections;
using Pathfinding;

public class Unit : MonoBehaviour
{
    public string test;
    public string NameUnit { get; set; }
    public int AttackDame { get; set; }
    public int CurrentAttackDame { get; set; }
    public int Hp { get; set; }
    public int CurrentHp { get; set; }
    public float AttackRange { get; set; }
    public float AttackRate { get; set; }
    public float CurrentAttackRate { get; set; }
    public float DelayDame { get; set; }
    public Vector2 pointShot;
    public List<UnitData> Childs { get; set; }
    public UnitData Data { get; set; }
    public Color healthColor { get; set; }

    private Vector2 defaultLoc;
    private float safeArea;
    public SkeletonAnimation skeletonAni;
    private UnitStateManager unitStateManager;
    private HealthBar healthBar;
    private UnitTypeImg unitTypeImg;
    public Dictionary<Vector2, Unit> standPoints;
    private SortingLayer sortingLayerUnit;
    private Sprite spriteBullet;
    private void Start()
    {
        standPoints = new Dictionary<Vector2, Unit>() { {  new Vector2(0.75f,-0.45f), null }, { Vector2.right, null }, { new Vector2(0.75f,0.45f), null },
                                                        {  new Vector2(-0.75f,0.45f), null }, {  Vector2.left, null }, {  new Vector2(-0.75f,-0.45f), null }};

        healthBar = GetComponentsInChildren<HealthBar>()[0];
        unitTypeImg = GetComponentsInChildren<UnitTypeImg>()[0];
        sortingLayerUnit = GetComponent<SortingLayer>();
        skeletonAni = GetComponent<SkeletonAnimation>();
        safeArea = 0.8f;
        defaultLoc = transform.position;
        sortingLayerUnit.Sorting();
        LoadData();
        //timerAttack = 0f;
    }
    public void LoadData()
    {
        this.NameUnit = gameObject.name = Data.unitName;
        this.AttackDame = this.CurrentAttackDame = Data.dame;
        this.Hp = this.CurrentHp = Data.hp;
        this.AttackRange = Data.attackRange;
        this.AttackRate = CurrentAttackRate = Data.attackRate;
        this.Childs = new List<UnitData>();
        this.Childs = Data.childs;
        this.DelayDame = Data.delayDame;
        if (AttackRange != 1)
        {
            spriteBullet = Resources.LoadAll<Sprite>("Sprites/").Where(s => s.name == Data.unitName + " Bullet").FirstOrDefault();
        }

        skeletonAni.ClearState();
        skeletonAni.skeletonDataAsset = Data.skeletonData;
        skeletonAni.Initialize(true);
        gameObject.AddComponent<BoxCollider2D>();
        GetComponentInChildren<UnitTier>().SetHealthBarLoc();

        unitStateManager = gameObject.AddComponent<UnitStateManager>();
        unitStateManager.unitController = this;

        healthBar.healthBarImage.color = healthColor;
        healthBar.gameObject.SetActive(false);

        unitTypeImg.LoadTypeUnit(GameManager.Instance.GetIconTypeByname(Data.unitType.ToString()));
        pointShot = Data.shotLoc;
    }
    public void TakeDame(int dame)
    {
        healthBar.gameObject.SetActive(true);
        CurrentHp -= dame;
        CurrentHp = CurrentHp < 0 ? 0 : CurrentHp;
        healthBar.UpdateHealthBar((float)CurrentHp / Hp);
        if (CurrentHp == 0)
        {
            healthBar.gameObject.SetActive(false);
            Die();
        }
    }
    public void Die()
    {
        Destroy(this);
        unitStateManager.SwitchState(unitStateManager.unitDieState);
    }
    private Vector2 slotMove;
    public bool MoveToTarget(Unit target)
    {
        if (target == null)
            return false;

        if (this.AttackRange == 1)
        {
            if (!target.standPoints.ContainsValue(this))
            {
                if (!target.standPoints.Any(s => s.Value == null))
                {
                    return false;
                }
                slotMove = target.standPoints.Where(s => s.Value == null).OrderBy(o => Vector2.Distance(transform.position, (Vector2)target.transform.position + o.Key)).FirstOrDefault().Key;
                target.standPoints[slotMove] = this;
            }
        }
        else
        {
            slotMove = Vector2.zero;
        }
        //float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position + Vector2.up * 0.5f);        
        float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position + slotMove);

        //transform.localScale = new Vector2((target.transform.position - transform.position).normalized.x > 0 ? -1 : 1, 1);
        if ((target.transform.position - transform.position).normalized.x > 0)
            transform.localScale = transform.GetChild(0).transform.localScale = new Vector2(-1, 1);
        else
            transform.localScale = transform.GetChild(0).transform.localScale = new Vector2(1, 1);

        if (this.AttackRange == 1)
        {
            if (distance > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, (Vector2)target.transform.position + slotMove, Time.deltaTime * 2f);
                sortingLayerUnit.Sorting();
                return false;
            }
        }
        else
        {
            if (distance > AttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, (Vector2)target.transform.position + slotMove, Time.deltaTime * 2f);
                sortingLayerUnit.Sorting();
                return false;
            }
        }
        return true;
    }

    //private float timerAttack;
    //private float timerDelayDame;
    public void Attack(Unit target, float timerAni)
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position + slotMove);

        if (this.AttackRange == 1)
        {
            if (distance > 0)
            {
                //if (unitStateManager.unitController.MoveToTarget(target))
                unitStateManager.SwitchState(unitStateManager.unitFindEnemyState);
            }
        }

        Vector2 spawnPoint = new Vector2();
        int speedBullet = -1;
        if (AttackRange == 1)
        {
            spawnPoint = (Vector2)target.transform.position;
            speedBullet = 0;
        }
        else
        {
            spawnPoint = (Vector2)transform.position + new Vector2(pointShot.x * transform.localScale.x, pointShot.y);
        }

        if (CurrentAttackDame == 4)
            Debug.Log("zsf");
        float dame = CurrentAttackDame * GameManager.Instance.MultipleDame(Data.unitType, target.Data.unitType);
        Instantiate(Data.bullet, spawnPoint, Quaternion.identity, transform).GetComponent<UnitBullet>().SetDataBullet(target, (int)dame, speedBullet, spriteBullet);

        //timerDelayDame += Time.deltaTime;

        //if (timerDelayDame >= DelayDame && timerAttack < DelayDame)
        //{
        //    Vector2 spawnPoint = new Vector2();
        //    int speedBullet = -1;
        //    if (AttackRange == 1)
        //    {
        //        spawnPoint = target.transform.position;
        //        speedBullet = 0;
        //    }
        //    else
        //    {
        //        spawnPoint = pointShot.position;
        //    }
        //    Instantiate(Data.bullet, spawnPoint, Quaternion.identity).GetComponent<UnitBullet>().SetDataBullet(target, CurrentAttackDame, speedBullet);

        //    //Debug.Log(timerDelayDame + "//" + timerAttack);
        //}

        //timerAttack += Time.deltaTime;
        //if (timerAttack >= timerAni)
        //{
        //    timerAttack = 0f;
        //    timerDelayDame = 0f;
        //}
    }
    public void SetDefaulLoc()
    {
        defaultLoc = transform.position;
        sortingLayerUnit.Sorting();
    }
    public Vector2 GetDefaulLoc()
    {
        return defaultLoc;
    }
    public virtual void OnDestroy()
    {
        if (!GameManager.fields.ContainsKey(defaultLoc))
            return;

        Unit unitInField = GameManager.fields[defaultLoc];
        if (unitInField == this)
        {
            GameManager.fields[defaultLoc] = null;
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeArea);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointShot, 0.1f);

        Gizmos.color = Color.black;
        foreach (var stand in standPoints)
        {
            Gizmos.DrawSphere((Vector2)transform.position + stand.Key, 0.1f);
        }
    }
#endif
}


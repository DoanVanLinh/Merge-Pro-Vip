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
    public Transform pointShot;
    public List<UnitData> Childs { get; set; }
    public UnitData Data { get; set; }
    public Color healthColor { get; set; }

    private Vector2 defaultLoc;
    private float safeArea;
    private SkeletonAnimation skeletonAni;
    private UnitStateManager unitStateManager;
    private HealthBar healthBar;
    public Dictionary<Vector2, Unit> standPoints;

    private void Start()
    {
        standPoints = new Dictionary<Vector2, Unit>() { {  new Vector2(0.75f,-0.45f), null }, { Vector2.right, null }, { new Vector2(0.75f,0.45f), null },
                                                        {  new Vector2(-0.75f,0.45f), null }, {  Vector2.left, null }, {  new Vector2(-0.75f,-0.45f), null }};
        healthBar = GetComponentsInChildren<HealthBar>()[0];
        healthBar.gameObject.SetActive(false);
        skeletonAni = GetComponent<SkeletonAnimation>();
        safeArea = 0.8f;
        defaultLoc = transform.position;
        LoadData();
        timerAttack = 0f;
    }
    public void LoadData()
    {
        this.NameUnit = gameObject.name = Data.nameUnit;
        this.AttackDame = this.CurrentAttackDame = Data.dame;
        this.Hp = this.CurrentHp = Data.hp;
        this.AttackRange = Data.attackRange;
        this.AttackRate = CurrentAttackRate = Data.attackRate;
        this.Childs = new List<UnitData>();
        this.Childs = Data.childs;
        this.DelayDame = Data.delayDame;

        GameManager.fields[defaultLoc] = this;
        skeletonAni.ClearState();
        skeletonAni.skeletonDataAsset = Data.skeletonData;
        skeletonAni.Initialize(true);

        unitStateManager = gameObject.AddComponent<UnitStateManager>();
        unitStateManager.unitController = this;

        healthBar.healthBarImage.color = healthColor;
        pointShot = transform;
    }
    public void TakeDame(int dame)
    {
        healthBar.gameObject.SetActive(true);
        CurrentHp -= dame;
        CurrentHp = CurrentHp < 0 ? 0 : CurrentHp;
        healthBar.UpdateHealthBar((float)CurrentHp / Hp);
        if (CurrentHp == 0)
        {
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

       
        if (!target.standPoints.ContainsValue(this))
        {
            if (!target.standPoints.Any(s=>s.Value == null))
            {
                return false;
            }
            slotMove = target.standPoints.Where(s => s.Value == null).FirstOrDefault().Key;
            target.standPoints[slotMove] = this;
        }
        //float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position + Vector2.up * 0.5f);        
        float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position + slotMove);

        //transform.localScale = new Vector2((target.transform.position - transform.position).normalized.x > 0 ? -1 : 1, 1);
        Vector2 direction = new Vector2();
        if ((target.transform.position - transform.position).normalized.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
            direction = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            direction = new Vector2(-1, 1);
        }

        if (this.AttackRange == 1)
        {
            if (distance > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, (Vector2)target.transform.position + slotMove, Time.deltaTime * 2f);
                return false;
            }
        }
        else
        {
            if (distance > AttackRange)
            {
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position + new Vector3(0.5f, 0.5f, 0), Time.deltaTime * 2f);
                transform.position = Vector3.MoveTowards(transform.position, (Vector2)target.transform.position + slotMove, Time.deltaTime * 2f);
                //Collider2D[] otherUnits = Physics2D.OverlapCircleAll(transform.position, safeArea);

                //foreach (var otherUnit in otherUnits)
                //{
                //    if (otherUnit.CompareTag(Helper.UNIT_BULLET_TAG) || otherUnit.gameObject == this.gameObject)
                //        continue;

                //    float offset = Vector2.Distance(transform.position, otherUnit.transform.position);
                //    if (offset < safeArea)
                //        transform.position += (safeArea - offset) * ((transform.position - otherUnit.transform.position).normalized);
                //}
                return false;
            }
        }
        return true;
    }

    private float timerAttack;
    private float timerDelayDame;
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

        timerDelayDame += Time.deltaTime;

        if (timerDelayDame >= DelayDame && timerAttack < DelayDame)
        {
            Vector2 spawnPoint = new Vector2();
            int speedBullet = -1;
            if (AttackRange == 1)
            {
                spawnPoint = target.transform.position;
                speedBullet = 0;
            }
            else
            {
                spawnPoint = pointShot.position;
            }
            Instantiate(Data.bullet, spawnPoint, Quaternion.identity).GetComponent<UnitBullet>().SetDataBullet(target, CurrentAttackDame, speedBullet);

            //Debug.Log(timerDelayDame + "//" + timerAttack);
        }

        timerAttack += Time.deltaTime;
        if (timerAttack >= timerAni)
        {
            timerAttack = 0f;
            timerDelayDame = 0f;
        }
    }
    public void SetDefaulLoc()
    {
        defaultLoc = transform.position;
    }
    public Vector2 GetDefaulLoc()
    {
        return defaultLoc;
    }
    public virtual void OnDestroy()
    {
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
        Gizmos.DrawSphere(pointShot.position, 0.1f);

        Gizmos.color = Color.black;
        foreach (var stand in standPoints)
        {
            Gizmos.DrawSphere(stand.Key, 0.1f);
        }
    }
#endif
}


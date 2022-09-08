using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Collections;

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
    private UnitStateManager unitState;
    private HealthBar healthBar;
    private void Start()
    {
        healthBar = GetComponentsInChildren<HealthBar>()[0];
        skeletonAni = GetComponent<SkeletonAnimation>();
        safeArea = 0.5f;
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

        unitState = gameObject.AddComponent<UnitStateManager>();
        unitState.unitController = this;

        healthBar.healthBarImage.color = healthColor;
        pointShot = transform;
    }
    public void TakeDame(int dame)
    {
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
        unitState.SwitchState(unitState.unitDieState);
    }
    public bool MoveToTarget(Unit target)
    {
        if (target == null)
            return false;

        float distance = Vector2.Distance(transform.position, target.transform.position);

        transform.localScale = new Vector2((target.transform.position - transform.position).normalized.x > 0 ? -1 : 1, 1);

        if (distance > AttackRange)
        {

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 2f);

            Collider2D[] otherUnits = Physics2D.OverlapCircleAll(transform.position, safeArea);
            foreach (var unit in otherUnits)
            {
                if (unit.CompareTag(Helper.UNIT_BULLET_TAG))
                    continue;

                float offset = Vector2.Distance(transform.position, unit.transform.position);
                if (offset < safeArea)
                    transform.position += (safeArea - offset) * (transform.position - unit.transform.position).normalized;
            }

            return false;
        }

        return true;
    }

    private float timerAttack;
    private float timerDelayDame;
    public void Attack(Unit target, float timerAni)
    {
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

            Debug.Log(timerDelayDame + "//" + timerAttack);
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeArea);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointShot.position, 0.1f);
    }
#endif
}


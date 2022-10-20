using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using WE.Unit.Move;
using WE.Unit.Attack;
using WE.Unit.Skill;
using WE.Unit.Target;
using WE.Unit.Animation;

namespace WE.Unit
{
    [System.Serializable]
    public class UnitStats
    {
        public float hp;
        public float mp;
        public float damage;
        public float moveSpeed;
        public float attackSpeed;
        public float attackRange;
        public float manaRegen;
        public NameTypeUnit unitType;
    }
    public class UnitAttributte
    {
        public float Value => ValueCount * (1 + ValuePercent / 100);
        public float ValueCount
        {
            get;
            private set;
        }
        public float ValuePercent
        {
            get;
            private set;
        }
        public void SetValueCount(float v)
        {
            ValueCount = v;
        }
        public void AddValueCount(float v)
        {
            ValueCount += v;
            ValueCount = ValueCount < 0 ? 0 : ValueCount;
        }
        public void AddValuePercent(float v)
        {
            ValuePercent += v;
            ValuePercent = ValuePercent < 0 ? 0 : ValuePercent;
        }
    }
    public class BaseUnit : MonoBehaviour
    {
        public BaseMover mover;
        public BaseAttack attacker;
        public BaseSkill skill;
        public BaseTargeting targeter;
        public BaseSkeletonAnimation skeletonAnimation;
        public UnitTier unitTier;
        public UnitTypeImg unitTypeImg;
        public HealthBar healthBar;

        public BaseUnit Target => targeter.Target;
        public UnitData unitStats;
        public UnitAttributte hpAttribute;
        public UnitAttributte mpAttribute;
        public UnitAttributte damageAttribute;
        public UnitAttributte moveSpeedAttribute;
        public UnitAttributte attackSpeedAttribute;
        public UnitAttributte attackRangeAttribute;
        public UnitAttributte manaRegenAttribute;

        public float MaxHp => hpAttribute.Value;
        public float MaxMp => mpAttribute.Value;
        public float MaxDamage => damageAttribute.Value;
        public float MaxMoveSpeed => moveSpeedAttribute.Value;
        public float MaxAttackSpeed => attackSpeedAttribute.Value;
        public float MaxAttackRange => attackRangeAttribute.Value;
        public float MaxManaRegen => manaRegenAttribute.Value;

        public float CurrentHp => currentHp;
        public float CurrentMp { get => currentMp; set => currentMp = value; }
        public float CurrentDamage => currentDamage;
        public float CurrentMoveSpeed => currentMoveSpeed;
        public float CurrentAttackSpeed => currentAttackSpeed;
        public float CurrentAttackRange => currentAttackRange;
        public float CurrentManaRegen => currentManaRegen;
        public NameTypeUnit UnitType => unitStats.unitType;
        public int Tier => unitStats.tier;
        public int Level => unitStats.level;
        public Vector2 ShotLoc => unitStats.shotLoc;
        public List<UnitData> Childs => unitStats.childs;
        public string ColorTier => unitStats.colorTier;

        protected float currentHp;
        protected float currentMp;
        protected float currentDamage;
        protected float currentMoveSpeed;
        protected float currentAttackSpeed;
        protected float currentAttackRange;
        protected float currentManaRegen;

        public bool IsAlive => isAlive;
        protected bool isAlive;
        protected bool IsInited;

        public System.Action<BaseUnit> OnUnitDie;

        public Vector2 defaultLoc;
        //protected virtual void Start()
        //{
        //    Init();
        //}
        public virtual void Init()
        {
            if (!IsInited)
            {
                defaultLoc = transform.position;
                unitTypeImg.LoadTypeUnit(GameManager.Instance.GetIconTypeByname(unitStats.unitType.ToString()));
                healthBar.healthBarImage.color = gameObject.tag == Helper.PLAYER_UNIT_TAG ? Color.green : Color.red;
                healthBar.gameObject.SetActive(false);

                hpAttribute = new UnitAttributte();
                mpAttribute = new UnitAttributte();
                damageAttribute = new UnitAttributte();
                attackSpeedAttribute = new UnitAttributte();
                attackRangeAttribute = new UnitAttributte();
                moveSpeedAttribute = new UnitAttributte();
                manaRegenAttribute = new UnitAttributte();

                hpAttribute.SetValueCount(unitStats.hp);
                mpAttribute.SetValueCount(unitStats.mp);
                damageAttribute.SetValueCount(unitStats.damage);
                attackSpeedAttribute.SetValueCount(unitStats.attackSpeed);
                attackRangeAttribute.SetValueCount(unitStats.attackRange);
                moveSpeedAttribute.SetValueCount(unitStats.moveSpeed);
                manaRegenAttribute.SetValueCount(unitStats.manaRegen);

                currentHp = MaxHp;
                currentMp = 0;
                currentDamage = MaxDamage;
                currentAttackSpeed = MaxAttackSpeed;
                currentAttackRange = MaxAttackRange;
                currentMoveSpeed = MaxMoveSpeed;
                currentManaRegen = MaxManaRegen;

                targeter.Init(this);
                mover.Init(this);
                attacker?.Init(this);
                attacker?.SetTargeter(targeter);
                skill?.Init(this);
                skeletonAnimation?.Init(this);
                gameObject.AddComponent<BoxCollider2D>();
                unitTier.SetHealthBarLoc();

                IsInited = true;
                isAlive = true;
            }
            targeter.OnNewTarget += OnNewTarget;
            skeletonAnimation.SetUnitAni(Helper.IDLE_STATE_ANI, true);
            //StartAction();
        }
        public virtual void StartAction()
        {
            targeter.GetTarget();
        }
        public virtual void OnNewTarget()
        {
            if (mover.OnMoveDone == null)
                mover.OnMoveDone += OnMoveToAttackPosition;
            skeletonAnimation.SetUnitAni(Helper.MOVE_STATE_ANI, true);
            mover.MoveToAttackPosition();
        }
        public virtual void OnMoveToAttackPosition()
        {
            mover.OnMoveDone -= OnMoveToAttackPosition;

            if (Target == null)
                return;

            attacker.StartAttack();
        }
        public virtual void TakeDamage(float dmg, BaseUnit source)
        {
            currentHp -= CalculateDamageTaken(dmg, source);
            if (currentHp <= 0 && isAlive)
            {
                currentHp = 0;
                healthBar.gameObject.SetActive(false);
                Die();
            }
            healthBar.gameObject.SetActive(true);
            healthBar.UpdateHealthBar((float)currentHp / MaxHp);
        }
        protected virtual float CalculateDamageTaken(float dmg, BaseUnit source)
        {
            return dmg * GameManager.Instance.MultipleDame(source.UnitType, UnitType);
        }
        public virtual void Die()
        {
            if (!isAlive)
                return;

            isAlive = false;
            //Stop();
            OnUnitDie?.Invoke(this);
            Destroy(gameObject);
        }
        public bool IsOnAttackRange()
        {
            return Vector2.Distance(transform.position, Target.transform.position) <= CurrentAttackRange;
        }
        protected virtual void OnDisable()
        {
            targeter.OnNewTarget -= OnNewTarget;
        }
        protected virtual void OnDestroy()
        {
            targeter.OnNewTarget -= OnNewTarget;
        }
        public virtual void Stop()
        {

        }
        public virtual void OnTargetDie()
        {
            Stop();
        }
        public virtual void Resume()
        {
            this.enabled = true;
        }
        public Vector2 GetDefaulLoc()
        {
            return defaultLoc;
        }
        public void SetDefaulLoc()
        {
            defaultLoc = transform.position;
        }

        public virtual T AddEffect<T>() where T : BaseEffect
        {
            T fx = gameObject.AddComponent<T>();
            if (fx.effectType == EffectType.Buff)
            {
                //Add to bufflist
            }
            if (fx.effectType == EffectType.Debuff)
            {
                //Add to debufflist;
            }

            return fx;
        }
        public virtual T GetEffect<T>() where T : BaseEffect
        {
            T fx = GetComponent<T>();
            return fx;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTest : MonoBehaviour
{
    public int attackDame;
    public int currentAttackDame;
    public int hp;
    public int currentHp;
    public float mp;
    public float currentMp;
    public float attackRange;
    public float attackRate;
    public float currentAttackRate;
    public Vector2 pointShot;
    public GameObject bulletPrefab;
    public Sprite spriteBullet;
    public MoveType moveType;
    public UnitTest target;


    private HealthBar healthBar;
    private UnitState currentUnitState;
    private SkillTest skill;
    private void OnEnable()
    {
        UnitManager.Instance.listUnit.Add(this);
        currentUnitState = UnitState.Idle;
        gameObject.TryGetComponent<SkillTest>(out skill);

        if (skill != null)
        {
            skill.unitInfo = this;
            mp = currentMp = 0;
        }
    }
    private void Start()
    {
        target = UnitManager.Instance.listUnit.Where(u => u != null && !u.gameObject.CompareTag(gameObject.tag)).OrderBy(o => Vector2.Distance(transform.position, o.transform.position)).Select(u => u).FirstOrDefault();
    }
    private void RegenMp(float value)
    {
        if (mp == 0)
            return;

        currentMp += value;
        currentMp = currentMp >= mp ? mp : currentMp;
    }
    private void Update()
    {
        skill.ExcuteSkill();
        RegenMp(Time.deltaTime);

        switch (currentUnitState)
        {
            case UnitState.Idle:
                break;
            case UnitState.Move:
                Move(target);
                break;
            case UnitState.Attack:
                Attack(target);
                break;
            case UnitState.Die:
                break;
            default:
                break;
        }
    }
    public void TakeDame(int dame)
    {
        healthBar.gameObject.SetActive(true);
        currentHp -= dame;
        currentHp = currentHp < 0 ? 0 : currentHp;
        healthBar.UpdateHealthBar((float)currentHp / hp);
        if (currentHp == 0)
        {
            healthBar.gameObject.SetActive(false);
            Die();
        }
    }
    public void Die()
    {
        currentUnitState = UnitState.Die;
        Destroy(this);
    }
    public void Attack(UnitTest target)
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(transform.position, (Vector2)target.transform.position);

        if (distance > attackRange)
        {
            currentUnitState = UnitState.Move;
            return;
        }

        Vector2 spawnPoint = new Vector2();
        int speedBullet = -1;
        if (attackRange == 1f)
        {
            spawnPoint = (Vector2)target.transform.position;
            speedBullet = 0;
        }
        else
        {
            spawnPoint = (Vector2)transform.position + new Vector2(pointShot.x * transform.localScale.x, pointShot.y);
        }

        Instantiate(bulletPrefab, spawnPoint, Quaternion.identity, transform).GetComponent<UnitBulletTest>().SetDataBullet(target, currentAttackDame, speedBullet, spriteBullet);
    }

    public void OnAttackDone()
    {

    }
    public void Move(UnitTest target)
    {
        if (target == null)
            currentUnitState = UnitState.Idle;

        float distanceToTaret = Vector2.Distance(transform.position, target.transform.position);

        switch (moveType)
        {
            case MoveType.Ground:

                break;
            case MoveType.Fly:

                break;
            default:
                break;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 10f);
        if (attackRange >= distanceToTaret)
        {
            currentUnitState = UnitState.Attack;
        }
    }
}
public enum UnitState
{
    Idle,
    Move,
    Attack,
    Die
}
public enum MoveType
{
    Ground,
    Fly
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseDestructibleObj
{
    [Header("Base Enemy")]
    public int Level;
    public int Worth;
    public float DamageDealt;
    public float AttackSpeed;
    public float MovementSpeed;

    [Header("Base Enemy Set Up")]
    public Vector3 Direction;
    public Vector3 StartPos;
    public Vector3 EndPos;

    private GameObject target;
    private float attackCount = 0f;
    private EnemyColor enemyColor;

    protected override void Awake()
    {
        base.Awake();
        enemyColor = GetComponent<EnemyColor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Die())
            return;

        if (Attack())
            return;

        Move();
    }

    protected override bool Die()
    {
        bool isAtEnd = CheckEndPos();
        if (CurrentHealth <= 0f  || isAtEnd)
        {
            if (!isAtEnd) DeathHandling();
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    protected override void DeathHandling()
    {
        base.DeathHandling();

        WaveCompleteUIManager.Instance.EnemiesKilledPerWave += 1;
        WaveCompleteUIManager.Instance.EnemiesKilledTotalWorth += Worth;

        //PlayerStats.Money += Worth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.GetComponent<BaseStructure>() != null)
        {
            target = other.gameObject;
        }
    }

    protected virtual bool Attack()
    {
        if (target != null)
        {
            if (attackCount <= 0f)
            {

                BaseStructure obj = target.GetComponent<BaseStructure>();
                if (obj != null && !obj.IsDestroyed)
                {
                    obj.TakeDamage(DamageDealt);
                }
                else
                {
                    target = null;
                    return false;
                }

                attackCount = AttackSpeed;
            }

            attackCount -= Time.deltaTime;
            return true;
        }

        return false;
    }

    protected virtual void Move()
    {
        transform.Translate(Direction * MovementSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void AttackHandling()
    {

    }

    private bool CheckEndPos()
    {

        if (Direction == Vector3.forward )
        {
            return ( transform.position.z >= EndPos.z);
        }
        else if (Direction == Vector3.right)
        {
            return (transform.position.x >= EndPos.x);
        }
        else if (Direction == Vector3.back)
        {
            return (transform.position.z <= EndPos.z);
        }
        else
        {
            return (transform.position.x <= EndPos.x);
        }
    }

    public virtual void SetLevel(int level)
    {
        // set level
        Level = level;

        // set speed for level
        //Data.MovementSpeed = level;

        // set damage for level
        DamageDealt = 1 + (level*.5f);

        // set health for level
        MaxHealth = CurrentHealth = 30 + (level * .5f);

        TakeDamage(0);

        //set color for this level
        if (enemyColor != null)
        {
            GetComponent<Renderer>().material.color = enemyColor.GetEnemeyColorByLevel(Level);
        }
    }
}

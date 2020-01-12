using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseDestructibleObj
{
    public float MovementSpeed = 1f;
    public float DamageDealt = 1f;
    public float AttackSpeed = 2f;
    private float attackCount = 0f;

    public Vector3 Direction;
    public Vector3 StartPos;
    public Vector3 EndPos;

    private GameObject target;

    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Die())
            return;

        if (Attack())
            return;

        Move();
    }



    protected override bool Die()
    {
        if (Health <= 0f || CheckEndPos())
        {
            DeathHandling();
            Destroy(gameObject);
            return true;
        }

        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.GetComponent<BaseBuilding>() != null)
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

                BaseDestructibleObj obj = target.GetComponent<BaseDestructibleObj>();
                if (obj != null)
                {
                    obj.TakeDamage(DamageDealt);
                }
                else
                {
                    target = null;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : BaseStructure
{
    protected BaseEnemy target;

    [Header("Base Tower")]
    public float range = 15f;
    public float Rotatespeed = 5f;
    public float Damage = 1f;

    [Header("Tower Setup")]
    public GameObject projectile;
    public Transform PartToRotate;
    public Transform FirePoint;
    public RangeManager rManager;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        if (rManager == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Range Manager attached for obj");
            return;
        }
        rManager.InitRange(range);
        ShowRange(false);

        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void UpdateTarget()
    {
        var enemys = rManager.GetTarget();
        if (enemys != null && enemys.Count >= 0)
        {
            float shoretDistToEnemy = Mathf.Infinity;
            BaseEnemy nearestEnemy = null;

            foreach (BaseEnemy enemy in enemys)
            {
                if (enemy == null)
                    continue;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(shoretDistToEnemy > distanceToEnemy)
                {
                    shoretDistToEnemy = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shoretDistToEnemy <= range)
            {
                target = nearestEnemy;
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Die())
            return;

        Attack();
    }

    #region Attack
    protected virtual void Attack()
    {

    }

    protected virtual void StopAttack()
    {

    }

    protected virtual bool LockOnTarget()
    {
        if (target == null )
            return false;

        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotaion = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * Rotatespeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotaion.y, 0f);

        if (Vector3.Angle(PartToRotate.transform.forward, target.transform.position - PartToRotate.transform.position) < 10f)
            return true;

        return false;
    }

    protected override void DeathHandling()
    {
        base.DeathHandling();

        StopAttack();
    }

    protected virtual void Shoot()
    {
        GameObject proj = (GameObject)Instantiate(projectile, FirePoint.position, FirePoint.rotation, this.transform);
        BaseProjectile baseProj = proj.GetComponent<BaseProjectile>();

        if (baseProj == null)
            return;

        baseProj.SetDamage(Damage);
        baseProj.SetTarget(target);
        baseProj.ShootStartEffects();
    }
    #endregion

    public void ShowRange(bool showRange)
    {
        rManager.ToggleRangeDisplay(showRange);
    }

}

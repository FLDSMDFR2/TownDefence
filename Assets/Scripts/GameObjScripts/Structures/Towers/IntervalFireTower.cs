using UnityEngine;

public class IntervalFireTower : BaseTower
{
    [Header("DirectDamageTower")]
    public float FireRate = 1f;
    protected float fireCountDown = 0f;

    protected override void Attack()
    {
        if (!LockOnTarget())
            return;

        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = 1f / FireRate;
        }

        fireCountDown -= Time.deltaTime;
    }
}

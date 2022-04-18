using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantFireTower : BaseTower
{
    protected GameObject _activeProjectile = null;

    protected override void Attack()
    {
        if (!LockOnTarget())
        {
            StopAttack();
            return;
        }

        Shoot();
    }

    protected override void Shoot()
    {
        if (target == null)
            return;

        if (_activeProjectile != null)
            return;

        _activeProjectile = (GameObject)Instantiate(projectile, FirePoint.position, FirePoint.rotation, this.transform);
        BaseProjectile proj = _activeProjectile.GetComponent<BaseProjectile>();
        if (proj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Invalide Project for DOT Tower");
            return;
        }

        proj.SetDamage(Damage);
        proj.SetTarget(target);
        proj.ShootStartEffects();
    }

    protected override void StopAttack()
    {
        if (_activeProjectile == null)
            return;

        BaseProjectile proj = _activeProjectile.GetComponent<BaseProjectile>();
        if (proj != null)
        {
            proj.StopFireSound();
        }

        Destroy(_activeProjectile);
    }
}

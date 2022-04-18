using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeProjectile : BaseProjectile
{
    protected override void DealDamage()
    {
        if (_target != null)
        {
            _target.TakeDamage(_damageDealt * Time.deltaTime);
        }
    }
}

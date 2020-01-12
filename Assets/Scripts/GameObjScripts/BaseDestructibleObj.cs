using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestructibleObj : BaseGameObj
{
    public float Health;

    protected virtual bool Die()
    {
        return false;
    }
    protected virtual void DeathHandling()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        DamageHandling();
    }
    protected virtual void DamageHandling()
    {

    }
}

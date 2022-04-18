using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEProjectile : BaseProjectile
{
    public float AOERadios =  0f;

    protected override void DealDamage()
    {
        if (AOERadios <= 0)
            return;

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, AOERadios);
        foreach (Collider c in hitObjects)
        {
            BaseEnemy target = c.gameObject.GetComponent<BaseEnemy>();
            if (target == null)
                continue;

            target.TakeDamage(_damageDealt);
        }

        DestroyProjectile();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AOERadios);  
    }

}

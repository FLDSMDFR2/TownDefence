using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{

    private Transform _target;

    public float speed = 70f;
    public float DamageDealt = 1f;
    public GameObject ImpactEffect;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceToTravel = speed * Time.deltaTime;

        if  (dir.magnitude <= distanceToTravel)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceToTravel, Space.World);
    }

    protected virtual void HitTarget()
    {
        Destroy(Instantiate(ImpactEffect, transform.position, transform.rotation),2f);
        Destroy(gameObject);

        BaseEnemy target = _target.gameObject.AddComponent<BaseEnemy>();
        if (target != null)
        {
            target.TakeDamage(DamageDealt);
        }
    }


}

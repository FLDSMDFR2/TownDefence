using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : DamageOverTimeProjectile
{
    public LineRenderer LaserBeam;
    protected override void UpdateProjectile()
    {
        if (_target == null)
        {
            return;
        }
  
        //assume we hit the target
        HitTarget();
    }

    protected override void HitTarget()
    {

        PlayHitSound();

        //update the effect
        ShootStartEffects();
        StartImpactEffect();

        DealDamage();
    }

    public override void ShootStartEffects()
    {
        LaserBeam.SetPosition(0, gameObject.transform.position);
        LaserBeam.SetPosition(1, _target.transform.position);
        LaserBeam.enabled = true;
    }

    protected override void ShootStopEffects()
    {
        LaserBeam.enabled = false;
    }

    protected override void StartImpactEffect()
    {
        ParticleSystem ps = ImpactEffect.GetComponent<ParticleSystem>();
        if (ps == null)
            return;

        Vector3 dir = gameObject.transform.position - _target.transform.position;
        ps.transform.position = _target.transform.position + dir.normalized * .5f;
        ps.transform.rotation = Quaternion.LookRotation(dir);
        ps.Play();
    }

    protected override void StopImpactEffect()
    {
        ParticleSystem ps = ImpactEffect.GetComponent<ParticleSystem>();
        if (ps == null)
            return;

        ps.Stop();
    }
}

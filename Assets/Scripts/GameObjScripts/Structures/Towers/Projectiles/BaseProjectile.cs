using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{

    public BaseEnemy _target;
    protected float _damageDealt = 1f;

    [Header("Base Projectile")]
    public float speed = 70f;
    public GameObject Visual;
    public GameObject ImpactEffect;
    public string FireSound;
    public string ImpactSound;

    public AudioSource _fireAudio;
    protected AudioSource _impactAudio;
    protected float _numTimesToPlayAudioPerSec = 2;
    protected float _playFireAudioCountDown = 0;
    protected bool _waitingDestroy = false;

    public void Start()
    {
        _fireAudio = AudioManager.Instance.GetAudio(FireSound, this.gameObject);
        _impactAudio = AudioManager.Instance.GetAudio(ImpactSound, this.gameObject);

        PlayFireSound();
    }

    public virtual void SetUpProjectile()
    {

    }

    public virtual void PlayHitSound()
    {
        if (_impactAudio != null) _impactAudio.Play();
    }

    public virtual void PlayFireSound()
    {
        if (_fireAudio != null) _fireAudio.Play();
    }

    public virtual void StopFireSound()
    {
        if (_fireAudio != null) _fireAudio.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateProjectile();
    }

    public virtual void SetTarget(BaseEnemy target)
    {
        _target = target;
    }

    public virtual void SetDamage(float d)
    {
        _damageDealt = d;
    }

    protected virtual void UpdateProjectile()
    {
        if (_waitingDestroy)
            return;

        if (_target == null)
        {
            DestroyProjectile();
            return;
        }

        Vector3 dir = _target.transform.position - transform.position;
        float distanceToTravel = speed * Time.deltaTime;

        if (dir.magnitude <= distanceToTravel)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceToTravel, Space.World);
    }

    protected virtual void HitTarget()
    {
        StartImpactEffect();
        PlayHitSound();
        DealDamage();
    }

    protected virtual void DealDamage()
    {
        if (_target != null)
        {
            _target.TakeDamage(_damageDealt);
        }
        DestroyProjectile();
    }

    protected virtual void DestroyProjectile()
    {
        _waitingDestroy = true;
        StartCoroutine(DestroyProjectileRoutine());
    }

    private IEnumerator DestroyProjectileRoutine()
    {
        if (Visual != null)
            Visual.SetActive(false);

        if (_impactAudio != null && _impactAudio.isPlaying)
            yield return new WaitWhile(() => _impactAudio.isPlaying);

        if (_fireAudio != null && _fireAudio.isPlaying)
            yield return new WaitWhile(() => _fireAudio.isPlaying);

        StopImpactEffect();
        Destroy(gameObject);
    }

    public virtual void ShootStartEffects()
    {

    }

    protected virtual void ShootStopEffects()
    {

    }

    protected virtual void StartImpactEffect()
    {
        Destroy(Instantiate(ImpactEffect, transform.position, transform.rotation, this.transform), 2f);
    }

    protected virtual void StopImpactEffect()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : BaseGameObj
{

    private Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float Rotatespeed = 5f;
    public float FireRate = 1f;
    private float fireCountDown = 0f;

    [Header("Setup")]
    public Transform PartToRotate;
    public Transform FirePoint;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        Object[] enemies = FindObjectsOfType(typeof(BaseEnemy));

        float shortestDist = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach(Object obj in enemies)
        {
            BaseEnemy enemyScript = (BaseEnemy)obj;
            GameObject enemy = enemyScript.gameObject;
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDist)
            {
                shortestDist = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && shortestDist <= range)
        {
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotaion = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * Rotatespeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f,rotaion.y, 0f);


        if (fireCountDown <=  0f)
        {
            Shoot();
            fireCountDown = 1f / FireRate;
        }

        fireCountDown -= Time.deltaTime;

    }

    public void Shoot()
    {
        GameObject proj =  (GameObject)Instantiate(projectile, FirePoint.position, FirePoint.rotation);
        BaseProjectile baseProj = proj.GetComponent<BaseProjectile>();

        if (baseProj == null)
            return;

        baseProj.SetTarget(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

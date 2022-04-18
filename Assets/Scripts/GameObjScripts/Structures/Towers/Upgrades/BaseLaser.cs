using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLaser : ConstantFireTower
{
    protected override void Shoot()
    {
        base.Shoot();
        PowerSupply.Instance.UserSupply(.01f);
    }
}

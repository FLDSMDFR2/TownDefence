using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeableObj : BasePurchasableObject
{
    [Header("Upgradeable Object")]
    public int Level = 1;

    public void SetUpgradeManager(GaneObjectType t)
    {
        Type = t;
    }

    public bool CanUpgrade
    {
        get
        {
            return Type != GaneObjectType.None;
        }
    }
}

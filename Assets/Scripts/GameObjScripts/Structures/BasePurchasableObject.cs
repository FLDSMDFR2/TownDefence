using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePurchasableObject : BaseDestructibleObj
{
    [Header("Purchasable Object")]
    public int Cost;

    public virtual int GetSellCost()
    {
        return Mathf.RoundToInt(Cost / 2);
    }

    public virtual int GetRepairCost()
    {
        return Mathf.RoundToInt(Cost / 2);
    }
}

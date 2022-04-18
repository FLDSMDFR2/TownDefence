using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSupplyManager : MonoBehaviour
{
    public UISupplyManager UIManager;

    protected float currentSupply = 100f;
    protected float maxSupply = 100f;

    public virtual void UpdateMaxSupply(float NewMax)
    {
        maxSupply = NewMax;
    }

    public void UserSupply(float SupplyUsed)
    {
        currentSupply -= SupplyUsed;
        if (currentSupply <= 0)
            currentSupply = 0;

        UIManager.UpdateSupplyDisplay(currentSupply / maxSupply);
    }

    public float CurrentSupply { get { return currentSupply; } }
    public float MaxSupply { get { return maxSupply; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSupply : BaseSupplyManager
{
    public static EquipmentSupply Instance;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi EquipmentSupply");
            return;
        }
        Instance = this;
    }
}

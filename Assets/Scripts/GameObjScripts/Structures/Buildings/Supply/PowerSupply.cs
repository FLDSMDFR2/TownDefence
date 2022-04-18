using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : BaseSupplyManager
{
    public static PowerSupply Instance;
   
    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi PowerSupply");
            return;
        }
        Instance = this;
    }

}

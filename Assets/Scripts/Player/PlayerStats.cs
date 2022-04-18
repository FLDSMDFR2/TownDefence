using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseSaveableObj
{
    public static PlayerData data;

    private void Awake()
    {
        Load();
    }

    #region "Save / Load"
    public override void Save()
    {
        if (data != null)
        {
            data.Save();
        }
    }
    public override void Load()
    {
        data = new PlayerData().Load<PlayerData>();
        if (data == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "PalyerData Failed to load");
            data = new PlayerData();
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{

    // Create dummy entry type
    [System.Serializable]
    public class Entry : GenericDictionaryItem<GaneObjectType, List<GameObject>>
    {
    }
    [System.Serializable]
    public class MyDictionary : GenericDictionary<GaneObjectType, List<GameObject>, Entry>
    {
    }

    public MyDictionary Upgrades;

    public static UpgradeManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi UpgradeManager");
            return;
        }
        Instance = this;
    }

    public void Purchase(GaneObjectType type)
    {
        GameObject item = GetUpgradeByLevel(type, 1);
        if (item == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Upgarde not avialable for level");
            return;
        }
        // if we are Purchasing it will start at level 1
        BuildManager.Instance.SetStructureToBuild(item);
        UIManager.Instance.ShowCanel();
    }

    public void Upgarde(GaneObjectType type, int level)
    {
        GameObject item = GetUpgradeByLevel(type, level);
        if (item == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Upgarde not avialable for level");
            return;
        }

        BuildManager.Instance.SetStructureToBuild(item);
    }

    private GameObject GetUpgradeByLevel(GaneObjectType type, int level)
    {
        if (level == 0 || !Upgrades.ContainsKey(type) || level > Upgrades[type].Count)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Upgarde not avialable for level");
            return null;
        }

        return Upgrades[type][level - 1];
    }

}

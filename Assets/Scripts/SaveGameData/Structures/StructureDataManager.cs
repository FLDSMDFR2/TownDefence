using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class StructureDataManager : SerializableObject
{
    private static string DefaultPath = "/Structures/Structures";

    private StructureDictionary baseStructures = new StructureDictionary();

    #region Singleton
    [System.NonSerialized]
    private static StructureDataManager _instance;
    protected StructureDataManager()
    {

    }
    public static StructureDataManager Instance()
    {
        if (_instance == null)
        {
            _instance = new StructureDataManager();
        }

        return _instance;
    }
    #endregion

    public int AddData(BaseStructure structure)
    {

        StructureData data = (StructureData)structure.GetData();

        // check if this type is in the dictionary add if its not
        if (!baseStructures.ContainsKey(data.Type))
        {
            baseStructures.Add(data.Type, new InnerDictionary());
        }

        // if the data we are trying to add already has a key then check if we know about it
        // if we do then just return the key and dont update the data, update date method should
        // be called if we want to update the data
        if (data.ID > 0)
        {
            if (baseStructures[data.Type].ContainsKey(data.ID))
            {
                baseStructures[data.Type][data.ID].Structure = structure;
                return data.ID;
            }
        }

        // loop to find first avaialable id
        int id = 1;
        while(baseStructures[data.Type].ContainsKey(id))
        {
            id++;
        }

        // we found the id to use add data / set id
        data.ID = id;
        data.Structure = structure;
        baseStructures[data.Type][id] = data;

        return data.ID;
    }

    public bool RemoveStructure(GaneObjectType type, int id)
    {
        if (!baseStructures.ContainsKey(type))
            return false;

        if (!baseStructures[type].ContainsKey(id))
            return false;

        return baseStructures[type].Remove(id);
    }

    public StructureData GetData(GaneObjectType type, int id)
    {
        if (!baseStructures.ContainsKey(type))
            return null;

        if (!baseStructures[type].ContainsKey(id))
            return null;

        return baseStructures[type][id];
    }

    #region "Save / Load"

    public override string Path { get { return DefaultPath; } }

    public override void Save()
    {
        SaveHelper();
        SerializeManager.Save(this);
    }

    private void  SaveHelper()
    {
        foreach (var typeKey in baseStructures.Keys)
        {
            foreach (var structId in baseStructures[typeKey].Keys)
            {
                // if the structure was not attached to the data just remove it on save
                if (baseStructures[typeKey][structId].Structure == null)
                {
                    baseStructures[typeKey].Remove(structId);
                    continue;
                }

                baseStructures[typeKey][structId] = (StructureData)baseStructures[typeKey][structId].Structure.GetData();
            }
        }
    }

    public void LoadHelper()
    {
        StructureDataManager loadManager = SerializeManager.Load<StructureDataManager>(this);

        if (loadManager != null)
        {
            _instance = loadManager;
        }
    }

    public override T Load<T>()
    {
        return SerializeManager.Load<T>(this);
    }

    #endregion
}


#region Dictionary

// Create dummy entry type
[System.Serializable]
public class Entry : GenericDictionaryItem<int, StructureData>
{
}
[System.Serializable]
public class InnerDictionary : GenericDictionary<int, StructureData, Entry>
{
}
[System.Serializable]
public class InnerEntry : GenericDictionaryItem<GaneObjectType, InnerDictionary>
{
}

[System.Serializable]
public class StructureDictionary : GenericDictionary<GaneObjectType, InnerDictionary, InnerEntry>
{
}
#endregion

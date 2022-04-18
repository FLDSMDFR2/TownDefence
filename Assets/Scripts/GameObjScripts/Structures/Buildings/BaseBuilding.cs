using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : BaseStructure
{

    [Header("Base Building")]
    public float MaxReasourceAmount = 0f;
    public float CurrentReasourceAmount = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Die())
            return;
    }

    public override void SetData(int id)
    {
        BuildingData data = (BuildingData)StructureDataManager.Instance().GetData(Type, id);
        CurrentReasourceAmount = data.CurrentReasourceAmount;

        base.SetData(id);
    }

    public override BaseData GetData()
    {
        BuildingData data = new BuildingData();
        data.ID = ID;
        data.Name = Name;
        data.Type = Type;
        data.IsDestroyed = isDestroyed;
        data.CurrentHealth = CurrentHealth;
        data.CurrentReasourceAmount = CurrentReasourceAmount;
        return data;
    }

}

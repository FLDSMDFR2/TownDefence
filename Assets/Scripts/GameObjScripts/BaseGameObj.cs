using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GaneObjectType
{
    None = 0,
    Enemy,
    MachineGun,
    Laser,
    Building,
    Rocket
}

public class BaseGameObj : MonoBehaviour
{
    [Header("Base Game Object")]
    public int ID;
    public GaneObjectType Type = GaneObjectType.None;
    public string Name;

    public virtual void SetData(int id)
    {
        BaseData data = StructureDataManager.Instance().GetData(Type, id);
        ID = data.ID;
        Name = data.Name;
    }

    public virtual BaseData GetData()
    {
        BaseData data = new BaseData();
        data.ID = ID;
        data.Name = Name;
        data.Type = Type;

        return data;
    }
}

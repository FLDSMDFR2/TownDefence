using UnityEngine;

[System.Serializable]
public class BaseData : SerializableObject
{
    private static string DefaultPath = "/Data/BaseData";

    [Header("Base Game Object")]
    public int ID;
    public GaneObjectType Type = GaneObjectType.None;
    public string Name;

    public BaseData()
    {
        path = DefaultPath;
    }

    #region "Save / Load"
    public override void Save()
    {
        SerializeManager.Save(this);
    }

    public override T Load<T>()
    {
        return SerializeManager.Load<T>(this);
    }
    #endregion
}

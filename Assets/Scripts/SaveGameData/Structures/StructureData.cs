using UnityEngine;

[System.Serializable]
public class StructureData : BaseData
{
    private static string DefaultPath = "/Structures/StructureData";

    [Header("Structure Data")]

    [Header("Upgradeable Data")]
    public int Level = 1;

    [Header("Destructible Data")]
    public bool IsDestroyed = false;
    public float CurrentHealth = -1f;

    [System.NonSerialized]
    public BaseStructure Structure;

    #region "Save / Load"
    public override string Path { get { return DefaultPath; } }

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

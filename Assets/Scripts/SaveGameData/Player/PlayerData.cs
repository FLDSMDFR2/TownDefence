using UnityEngine;

[System.Serializable]
public class PlayerData : SerializableObject
{
    private static int DefaultStartMoney = 1000;
    private static string DefaultPath = "/Player/PlayerData";

    public int Money;

    public PlayerData()
    {
        path = DefaultPath;

        Money = DefaultStartMoney;
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

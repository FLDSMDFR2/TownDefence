using System.Collections.Generic;

[System.Serializable]
public class MapData : SerializableObject
{
    private static string DefaultPath = "/Environments/MapData";
    public List<List<FloorData>> ListFloorData;

    public int XValue = 30;
    public int ZValue = 30;

    public float XbaseSize = 1f;
    public float ZbaseSize = 1f;

    public SerializableVector3 StartPoint = new SerializableVector3(0f,0f,0f);

    public MapData()
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
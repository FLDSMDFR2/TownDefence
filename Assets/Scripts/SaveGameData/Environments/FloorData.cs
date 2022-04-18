using UnityEngine;

[System.Serializable]
public class FloorData : SerializableObject
{

    private static string DefaultPath = "/Environments/FloorData";

    public SerializableVector3 Location;
    public float XbaseSize;
    public float ZbaseSize;

    public StructureInfoData Structure = new StructureInfoData();

    [System.NonSerialized] public BaseGameObj GameObj;

    public FloorData(float xSize, float zSize, Vector3 loc, BaseGameObj prefab)
    {
        path = DefaultPath;

        XbaseSize = xSize;
        ZbaseSize = zSize;
        Location = loc;
        GameObj = prefab;
        GameObj.transform.localScale = new Vector3(XbaseSize, .1f, ZbaseSize);
    }

    public void DrawLocation()
    {
        Debug.DrawLine(new Vector3(Location.x - (XbaseSize / 2), Location.y, Location.z - (ZbaseSize / 2)), new Vector3(Location.x - (XbaseSize / 2), Location.y, Location.z + (ZbaseSize / 2)), Color.red);
        Debug.DrawLine(new Vector3(Location.x + (XbaseSize / 2), Location.y, Location.z - (ZbaseSize / 2)), new Vector3(Location.x + (XbaseSize / 2), Location.y, Location.z + (ZbaseSize / 2)), Color.red);

        Debug.DrawLine(new Vector3(Location.x - (XbaseSize / 2), Location.y, Location.z + (ZbaseSize / 2)), new Vector3(Location.x + (XbaseSize / 2), Location.y, Location.z + (ZbaseSize / 2)), Color.red);
        Debug.DrawLine(new Vector3(Location.x - (XbaseSize / 2), Location.y, Location.z - (ZbaseSize / 2)), new Vector3(Location.x + (XbaseSize / 2), Location.y, Location.z - (ZbaseSize / 2)), Color.red);
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

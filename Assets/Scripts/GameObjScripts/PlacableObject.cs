using UnityEngine;
public class PlaceableObject : BaseSaveableObj
{
    [Header("Placeable Location")]
    public BaseFloor floor;
    public GameObject BuildEffect;

    [Header("Placeable Data")]
    public Vector3 PositionOffset;

    public BaseFloor Floor
    {
        get
        {
            return floor;
        }
        set
        {
            floor = value;
        }
    }
}

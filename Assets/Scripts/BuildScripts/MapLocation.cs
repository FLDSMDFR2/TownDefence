using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public float XbaseSize;
    public float ZbaseSize;
    public Vector3 Location;
    public BaseGameObj GameObj;

    public MapLocation(float xSize, float zSize, Vector3 loc, BaseGameObj prefab)
    {
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

}

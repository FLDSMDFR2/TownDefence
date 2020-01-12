using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameMapGenerator : MonoBehaviour
{
    public static GameMapGenerator Instance;

    public Vector3 StartPoint;

    public int XValue = 10;
    public int ZValue = 10;

    public float XbaseSize = 0.4f;
    public float ZbaseSize = 0.4f;

    public BaseGameObj Prefab;

    private List<List<MapLocation>> mapLocList;
    private bool init = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi GameMapGenerator");
            return;
        }
        Instance = this;

        StartPoint = new Vector3(0f, 0f, 0f);
        if (Prefab == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Missing prefab");
            return;
        }

        BuildMap();
        init = true;
    }

    void Update()
    {
        if (mapLocList == null && init == false) return;
        DrawMap();
    }

    private void BuildMap()
    {
        TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Building Map");

        mapLocList = new List<List<MapLocation>>();

        for (float i = 0; i < XValue; i++)
        {
            mapLocList.Add(new List<MapLocation>());
            for (float j = 0; j < ZValue; j++)
            {
                var obj = Instantiate(Prefab, StartPoint + new Vector3(i * XbaseSize, StartPoint.y, j * ZbaseSize), Quaternion.identity);
                var mapObj = new MapLocation(XbaseSize, ZbaseSize, StartPoint + new Vector3(i * XbaseSize, StartPoint.y, j * ZbaseSize), obj);
                obj.transform.parent = gameObject.transform;

                mapLocList[(int)i].Add(mapObj);
            }
        }
    }

    private void DrawMap()
    {
        foreach (List<MapLocation> lst in mapLocList)
        {
            foreach (MapLocation obj in lst)
            {
                obj.DrawLocation();
            }
        }
    }

    public List<List<MapLocation>> MapLocList
    {
        get { return mapLocList; }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameMapGenerator : BaseSaveableObj
{
    public static GameMapGenerator Instance;

    public BaseGameObj Prefab;
    public MapData data;

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

        data.StartPoint = new SerializableVector3(0f, 0f, 0f);
        if (Prefab == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Missing prefab");
            return;
        }     
    }
    private void Start()
    {
        Load();
        BuildMap();
        init = true;

        if (data == null || data.ListFloorData == null || init == false) return;
        DrawMap();
    }

    private void BuildMap()
    {
        if (data.ListFloorData != null)
        {
            LoadMap();
            return;
        }

        data.ListFloorData = new List<List<FloorData>>();

        for (float i = 0; i < data.XValue; i++)
        {
            data.ListFloorData.Add(new List<FloorData>());
            for (float j = 0; j < data.ZValue; j++)
            {
                var obj = Instantiate(Prefab, data.StartPoint + new Vector3(i * data.XbaseSize, data.StartPoint.y, j * data.ZbaseSize), Quaternion.identity);

                BaseFloor floor = obj.GetComponent<BaseFloor>();
                if (floor == null)
                {
                    TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
                    return;
                }
                
                SerializableVector3 loc = data.StartPoint + new Vector3(i * data.XbaseSize, data.StartPoint.y, j * data.ZbaseSize);
                var mapObj = new FloorData(data.XbaseSize, data.ZbaseSize, loc, obj);
                floor.SetData(ref mapObj);

                obj.transform.parent = gameObject.transform;

                data.ListFloorData[(int)i].Add(mapObj);
            }
        }
    }

    private void LoadMap()
    {
        foreach (List<FloorData> lData in data.ListFloorData)
        {
            foreach(FloorData d in lData)
            {
                var obj = Instantiate(Prefab, d.Location, Quaternion.identity);
                BaseFloor floor = obj.GetComponent<BaseFloor>();
                if (floor == null)
                {
                    TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
                    return;
                }
                FloorData fdata = lData[lData.IndexOf(d)];
                floor.SetData(ref fdata);

                d.GameObj = obj;
                obj.transform.parent = gameObject.transform;

                if (d.Structure != null && d.Structure.ID > 0)
                {
                    StructureData sData = StructureDataManager.Instance().GetData(d.Structure.Type, d.Structure.ID);
                    if (sData != null)
                    {
                        UpgradeManager.Instance.Upgarde(sData.Type, sData.Level);
                        BuildManager.Instance.PlaceStructure(floor, sData);
                    }
                }
            }
        }
    }

    private void DrawMap()
    {
        foreach (List<FloorData> lst in data.ListFloorData)
        {
            foreach (FloorData obj in lst)
            {
                obj.DrawLocation();
            }
        }
    }

    public List<List<FloorData>> MapLocList
    {
        get { return data.ListFloorData; }
    }

    #region "Save / Load"
    public override void Save()
    {
        if (data != null)
        {
            data.Save();
        }
    }
    public override void Load()
    {
        data = new MapData().Load<MapData>();
        if (data == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "MapData Failed to load");
            data = new MapData();
        }
    }
    #endregion
}

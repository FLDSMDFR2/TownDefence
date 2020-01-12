using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager Instance;

    private GameObject StructureToBuild;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi BuildManager");
            return;
        }
        Instance = this;
    }

    public  GameObject GetStructureToBuild()
    {
        if (StructureToBuild == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No  Structure to build");
        }

        return StructureToBuild;
    }

    public void SetStructureToBuild(GameObject buildObj)
    {
        if (buildObj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No  Structure to build");
            return;
        }

        StructureToBuild = buildObj;
    }
}

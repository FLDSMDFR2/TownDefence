using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public static WorldBuilder Instance;
    public GameObject MainCamera;

    public void Awake() 
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi WorldBuilders");
            return;
        }
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (MainCamera == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "MainCamera missing");
            return;
        }
        SpawnCamera();
        SetUpWorld();
    }

    private void SpawnCamera()
    {
        Instantiate(MainCamera, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    private void SetUpWorld()
    {
        WaveSpawner.Instance.WavesEnabled = true;
    }
}

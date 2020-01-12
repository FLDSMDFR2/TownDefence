using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour   
{
    public static WaveSpawner Instance;

    public bool WavesEnabled = false;
    public GameObject Enemies;
    public float TimeBetweenWaves = 5f;
    public float TotalWaves = 1f;
    private float waveCount = 0f;
    private float enemiesPerWave = 5f;

    private ArrayList ActiveEnemies = new ArrayList();
    private float countdown = 5f;

    private List<MapLocation> spawnLocations;
    private float maxX;
    private float maxZ;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi WaveSpawner");
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Enemies == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Missing Enemy!");
            return;
        }
        BuildSpawnLocList();
    }

    // Update is called once per frame
    void Update()
    {

        if (TotalWaves >= waveCount)
        {
            waveCount = 0;
            WavesEnabled = false;
        }

        if (WavesEnabled && !WaveComplete())
            return;

        if (countdown <= 0f)
        {
            SpawnNewWave();
            countdown = TimeBetweenWaves;
        }

        countdown -= Time.deltaTime;

    }

    private bool WaveComplete()
    {

        return true;
    }

    private void SpawnNewWave()
    {
        BuildSpawnLocList();

        for (float i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy(Enemies);
        }
    }

    private void SpawnEnemy(GameObject NextEnemy)
    {
        BaseEnemy enemy = NextEnemy.GetComponent<BaseEnemy>();
        if (enemy == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "BaseEnemy  not on object!");
            return;
        }

        // find random start pos based on map
        if (spawnLocations.Count <= 0)
            return;

        Vector3 StartPos = new Vector3(0f, .2f, 0f);
        Vector3 EndPos = new Vector3(0f, .2f, 0f);
        Vector3 Direction = Vector3.forward;

        FindEnemySpawn(ref StartPos, ref EndPos, ref Direction);

        enemy.StartPos = StartPos;
        enemy.EndPos = EndPos;
        enemy.Direction = Direction;

        Instantiate(NextEnemy, StartPos, Quaternion.identity);
    }

    private void FindEnemySpawn(ref Vector3 StartPos, ref Vector3 EndPos,  ref Vector3 Direction)
    {
        StartPos = spawnLocations[RandomNumber(0, spawnLocations.Count - 1)].Location + new Vector3(0f, .2f, 0f);
        EndPos = StartPos;
        if (StartPos.x == 0f )
        {
            EndPos.x = maxX;
            Direction = Vector3.right;
        }
        else if (StartPos.x == maxX)
        {
            EndPos.x = 0f;
            Direction = Vector3.left;
        }
        else if (StartPos.z == 0)
        {
            EndPos.z = maxZ;
            Direction = Vector3.forward;
        }
        else if (StartPos.z == maxZ)
        {
            EndPos.z = 0f;
            Direction = Vector3.back;
        }
    }

    private void BuildSpawnLocList()
    {

    maxX = (GameMapGenerator.Instance.XValue * GameMapGenerator.Instance.XbaseSize) - GameMapGenerator.Instance.XbaseSize;
    maxZ = (GameMapGenerator.Instance.ZValue * GameMapGenerator.Instance.ZbaseSize) - GameMapGenerator.Instance.ZbaseSize;
    spawnLocations = new List<MapLocation>();
        foreach (List<MapLocation> mlist in GameMapGenerator.Instance.MapLocList)
        {
            foreach (MapLocation loc in mlist)
            {
                //  find the border locations
                if (loc.Location.x == 0f || loc.Location.z == 0f || loc.Location.x == maxX || loc.Location.z == maxZ)
                {
                    if (CheckForStructInPath(loc))
                    {
                        spawnLocations.Add(loc);
                        loc.GameObj.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    private bool CheckForStructInPath(MapLocation loc)
    {

        if (loc.Location.z == 0 || loc.Location.z == maxX)
        {
            float indexX = loc.Location.x / GameMapGenerator.Instance.XbaseSize;
            foreach (MapLocation l in GameMapGenerator.Instance.MapLocList[(int)indexX])
            {
                BaseFloor floor = l.GameObj.GetComponent<BaseFloor>();
                if (floor == null)
                    continue;

                if (floor.HasStructure()) return true;
            }
        }
        else if (loc.Location.x == 0 || loc.Location.x == maxZ)
        {
            float indexZ = loc.Location.z / GameMapGenerator.Instance.ZbaseSize;
            foreach (List<MapLocation> mlist in GameMapGenerator.Instance.MapLocList)
            {
                BaseFloor floor = mlist[(int)indexZ].GameObj.GetComponent<BaseFloor>();
                if (floor == null)
                    continue;

                if (floor.HasStructure()) return true;
            }
        }

        return false;
    }

    public int RandomNumber(int min, int max)
    {
        System.Random rand = new System.Random();
        return rand.Next(min, max);
    }
}

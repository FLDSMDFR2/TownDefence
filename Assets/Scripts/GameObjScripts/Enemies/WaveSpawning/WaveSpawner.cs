using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawner : MonoBehaviour   
{
    [Header("WaveSpawner")]
    public List<WaveEnemy> Enemies;
    public float TimeBetweenWaves = 20f;
    public float TimebetweenEnemies = .8f;
    public bool InfiniteWaveMode = false;

    protected float countdown = 20f;
    protected bool WavesEnabled = false;
    protected int currentWaveNumber = 0;
    protected int enemiesPerWaveStart = 5;
    protected int enemiesPerWave = 0;
    protected float enemiesSpawned = 0f;
    protected bool waveSpawned = false;

    protected List<FloorData> spawnLocations;
    protected List<BaseEnemy> enemiesSpawnedList = new List<BaseEnemy>();
    protected float maxX;
    protected float maxZ;

    protected float fixedUpdateCheckCountdown = 20f;
    public float TimebetweenFixedUpdateCheck = 1f;

    void FixedUpdate()
    {

        if (!WavesEnabled)
        {
            SetDisplyText("", "");
            return;
        }

        // once we start a wave only run update at intervel
        if (waveSpawned && fixedUpdateCheckCountdown >= 0)
        {
            fixedUpdateCheckCountdown -= Time.deltaTime;
            return;
        }
        fixedUpdateCheckCountdown = TimebetweenFixedUpdateCheck;

        if (!CanContinue())
        {
            SetDisplyText("", "");
            EndWaveCleanUp();
            WaveCompleteUIManager.Instance.ShowWaveComplete(false);
            return;
        }

        if (waveSpawned && IsWaveComplete())
        {
            if (InfiniteWaveMode)
            {
                StartNextWave();
            }
            else
            {
                SetDisplyText("", "");
                EndWaveCleanUp();
                WaveCompleteUIManager.Instance.ShowWaveComplete(true);
            }

            return;
        }

        if (!waveSpawned)
        {
            //Display.text = "Wave Complete";

            if (countdown <= 0f)
            {
                waveSpawned = true;
                enemiesSpawned = 0;

                SpawnNewWave();

                countdown = TimeBetweenWaves;
            }
            else
            {
                SetDisplyText("", "Next Wave " + string.Format("{0:00}", Mathf.Clamp(countdown, 0f, Mathf.Infinity)));
            }

            countdown -= Time.deltaTime;

            return;
        }
        else
        {
            SetDisplyText("Wave "+ (currentWaveNumber).ToString(), "Enemies Remaining " + (EnemiesRemaining () + (enemiesPerWave - enemiesSpawned)).ToString());
        }
    }

    public virtual void StartNextWave()
    {
        currentWaveNumber += 1;
        enemiesPerWave = currentWaveNumber * enemiesPerWaveStart;
        DayNightManager.Instance.SetNextInterval((currentWaveNumber) / 10f);
        RestWaveDtl();
    }

    protected virtual void RestWaveDtl()
    {
        enemiesSpawned = 0f;
        waveSpawned = false;
        WavesEnabled = true;
    }

    protected virtual void SetDisplyText(string waveNumber, string displayText)
    {

    }

    protected virtual void EndWaveCleanUp()
    {
        foreach (FloorData floor in spawnLocations)
        {
            BaseFloor f = floor.GameObj.GetComponent<BaseFloor>();
            if (f == null)
                continue;

            f.ResetFloorColor();
        }

        enemiesSpawnedList.Clear();
    }

    protected virtual bool IsWaveComplete()
    {
        return enemiesSpawned >= enemiesPerWave && EnemiesRemaining() == 0;
    }

    protected virtual bool CanStart()
    {
        BuildSpawnLocList();
        if (spawnLocations == null || spawnLocations.Count <= 0)
        {
            return false;
        }
        return true;
    }

    protected virtual bool CanContinue()
    {
        return CheckSpawnPathStillValide();
    }

    protected virtual int EnemiesRemaining()
    {
        int cnt = 0;
        foreach (BaseEnemy e in enemiesSpawnedList)
        {
            if (e != null)
                cnt++;
        }

        return cnt;
    }

    protected virtual void SpawnNewWave()
    {
        BuildSpawnLocList();
        StartCoroutine(SelectEnemyToSpawn());
    }

    IEnumerator SelectEnemyToSpawn()
    {
        ColorSpawnLocations();

        while (WavesEnabled && enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy(GetEnemyToSpawn());
            enemiesSpawned += 1;
            yield return new WaitForSeconds(TimebetweenEnemies);
        }
    }

    protected void ColorSpawnLocations()
    {
        foreach (FloorData floor in spawnLocations)
        {
            floor.GameObj.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    protected virtual void SpawnEnemy(GameObject NextEnemy)
    {
        if (NextEnemy == null)
            return;

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

        FindEnemySpawn(ref StartPos, ref EndPos, ref Direction, enemy.PositionOffset);

        enemy.StartPos = StartPos;
        enemy.EndPos = EndPos;
        enemy.Direction = Direction;

        GameObject e = Instantiate(NextEnemy, StartPos, Quaternion.identity);
        BaseEnemy be = e.GetComponent<BaseEnemy>();
        if (be == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "BaseEnemy  not on object!");
            return;
        }

        enemiesSpawnedList.Add(be);

    }

    protected GameObject GetEnemyToSpawn()
    {
        float total = 0;
        int waveMod = currentWaveNumber % 10;
        if (waveMod == 0) waveMod = 10;

        foreach (WaveEnemy we in Enemies)
        {
            if (!( waveMod >= we.ValideWaveMin && waveMod <= we.ValideWaveMax))
                continue;

            total += we.Probability;
        }

        float mRand = Random.value * total;

        foreach(WaveEnemy we in Enemies)
        {
            if (!( waveMod >= we.ValideWaveMin && waveMod <= we.ValideWaveMax))
                continue;

            if (mRand < we.Probability)
            {
                return we.Enemy;
            }
            mRand -= we.Probability;
        }

        return null;
    }

    protected virtual void FindEnemySpawn(ref Vector3 StartPos, ref Vector3 EndPos,  ref Vector3 Direction, Vector3 EnimeyOffset)
    {
        StartPos = spawnLocations[RandomNumber(0, spawnLocations.Count - 1)].Location + EnimeyOffset;
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

    protected virtual bool CheckSpawnPathStillValide()
    {
        foreach (FloorData loc in spawnLocations)
        {
            if (CheckForStructInPath(loc))
            {
                return true;
            }
        }

        return false;
    }

    protected virtual void BuildSpawnLocList()
    {
        if (GameMapGenerator.Instance.data == null || GameMapGenerator.Instance.MapLocList == null) return;

        maxX = (GameMapGenerator.Instance.data.XValue * GameMapGenerator.Instance.data.XbaseSize) - GameMapGenerator.Instance.data.XbaseSize;
        maxZ = (GameMapGenerator.Instance.data.ZValue * GameMapGenerator.Instance.data.ZbaseSize) - GameMapGenerator.Instance.data.ZbaseSize;
        spawnLocations = new List<FloorData>();
        foreach (List<FloorData> mlist in GameMapGenerator.Instance.MapLocList)
        {
            foreach (FloorData loc in mlist)
            {
                //  find the border locations
                if (loc.Location.x == 0f || loc.Location.z == 0f || loc.Location.x == maxX || loc.Location.z == maxZ)
                {
                    if (CheckForStructInPath(loc))
                    {
                        spawnLocations.Add(loc);                    
                    }
                }
            }
        }
    }

    protected virtual bool CheckForStructInPath(FloorData loc)
    {

        if (loc.Location.z == 0 || loc.Location.z == maxX)
        {
            float indexX = loc.Location.x / GameMapGenerator.Instance.data.XbaseSize;
            foreach (FloorData l in GameMapGenerator.Instance.MapLocList[(int)indexX])
            {
                BaseFloor floor = l.GameObj.GetComponent<BaseFloor>();
                if (floor == null)
                    continue;

                if (floor.HasActiveStructure()) return true;
            }
        }
        else if (loc.Location.x == 0 || loc.Location.x == maxZ)
        {
            float indexZ = loc.Location.z / GameMapGenerator.Instance.data.ZbaseSize;
            foreach (List<FloorData> mlist in GameMapGenerator.Instance.MapLocList)
            {
                BaseFloor floor = mlist[(int)indexZ].GameObj.GetComponent<BaseFloor>();
                if (floor == null)
                    continue;

                if (floor.HasActiveStructure()) return true;
            }
        }

        return false;
    }

    public int LastWaveNumber { get { return currentWaveNumber - 1; } }
    public int CurrentWaveNumber { get { return currentWaveNumber; } }

    protected int RandomNumber(int min, int max)
    {
        System.Random rand = new System.Random();
        return rand.Next(min, max);
    }
}

[System.Serializable]
public class WaveEnemy
{
    public GameObject Enemy;
    public float Probability;
    public int ValideWaveMin;
    public int ValideWaveMax;
}

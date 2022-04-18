using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : WaveSpawner
{
    public static WaveManager Instance;

    public Text WaveDisplay;
    public Text Display;
    public GameObject StartButton;
    public int StartWaveNum = 1;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi WaveManager");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (Enemies == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Missing Enemy!");
            return;
        }
        if (Display == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Missing Display!");
            return;
        }

        UIEvents.Instance.OnStartWave += StartWave;

        countdown = TimeBetweenWaves;
        BuildSpawnLocList();
    }

    protected override void SetDisplyText(string waveNumber, string displayText)
    {
        WaveDisplay.text = waveNumber;
        Display.text = displayText;     
    }

    public void StartWave()
    {
        if (!CanStart())
            return;

        currentWaveNumber = StartWaveNum;
        WaveCompleteUIManager.Instance.EnemiesKilledPerWave = 0;
        WaveCompleteUIManager.Instance.EnemiesKilledTotalWorth = 0;
        enemiesPerWave = currentWaveNumber * enemiesPerWaveStart;

        DayNightManager.Instance.SetNextInterval((currentWaveNumber) / 10f);

        RestWaveDtl();
        LockUIActivity();
    }

    protected override void RestWaveDtl()
    {
        base.RestWaveDtl();
        StartButton.SetActive(false);
    }

    protected override void EndWaveCleanUp()
    {
        base.EndWaveCleanUp();

        WavesEnabled = false;
        StartButton.SetActive(true);
        ClearSpawnedEnenmies();
    }

    private void ClearSpawnedEnenmies()
    {
        foreach (BaseEnemy enemy in FindObjectsOfType<BaseEnemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    private void LockUIActivity()
    {
        UIManager.Instance.CloseAll();
        //  TODO PREVENT SHOPPING
    }

    private void OnDestroy()
    {
        UIEvents.Instance.OnStartWave -= StartWave;
    }

}

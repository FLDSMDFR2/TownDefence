using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCompleteUIManager : MonoBehaviour
{

    public static WaveCompleteUIManager Instance;

    public Text WaveNumCompleteText;
    public Text WaveCompleteText;

    public Text EniemesKilled;
    public Text EniemesKilledMoney;

    public Text StructuresLeft;
    public Text StructuresLeftMoney;

    public Text TotalMoneyEarned;

    public Button ContinueButton;

    public int CostPerStructureLeft = 50;

    public int EnemiesKilledPerWave;
    public int EnemiesKilledTotalWorth;

    private int moneyEarned;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi UIWaveCompleteUIManagerManager");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UIEvents.Instance.OnContinueWave += Continue;
        UIEvents.Instance.OnEndWave += Stop;
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        EnemiesKilledPerWave = 0;
        //EnemiesKilledTotalWorth = 0;
        WaveManager.Instance.StartNextWave();
    }

    public void Stop()
    {
        DayNightManager.Instance.SetDay();
        gameObject.SetActive(false);
        //  TODO: ADD  WAVE COMPLETE STATES TO PLAYER
        PlayerStats.data.Money += moneyEarned;
    }

    public void ShowWaveComplete(bool canContinue)
    {

        ContinueButton.gameObject.SetActive(canContinue);
        if (canContinue)
        {
            WaveCompleteText.text = "Survived";
        }
        else
        {
            WaveCompleteText.text = "Failed";
        }

        EniemesKilled.text = EnemiesKilledPerWave.ToString();
        EniemesKilledMoney.text = "$" + EnemiesKilledTotalWorth.ToString();
        moneyEarned = EnemiesKilledTotalWorth;

        BaseStructure[] structsLeft = FindObjectsOfType<BaseStructure>();
        int structsLeftCnt = 0;
        foreach (BaseStructure  struc in structsLeft)
        {
            if (!struc.IsDestroyed)
                structsLeftCnt++;
        }
        StructuresLeft.text = structsLeftCnt.ToString();
        StructuresLeftMoney.text = structsLeftCnt.ToString() + " x "+CostPerStructureLeft+" x " + WaveManager.Instance.CurrentWaveNumber.ToString() +
            " = $" + (structsLeftCnt * CostPerStructureLeft * WaveManager.Instance.CurrentWaveNumber).ToString();
        moneyEarned += structsLeftCnt * 100 * WaveManager.Instance.CurrentWaveNumber;

        //TODO OTHER WAS TO EARN MONEY

        TotalMoneyEarned.text = moneyEarned.ToString();

        WaveNumCompleteText.text = "Wave " + WaveManager.Instance.CurrentWaveNumber.ToString(); 
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        UIEvents.Instance.OnContinueWave -= Continue;
        UIEvents.Instance.OnEndWave -= Stop;
    }
}

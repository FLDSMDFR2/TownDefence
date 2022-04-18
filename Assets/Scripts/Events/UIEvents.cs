using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents Instance;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi UIEvents");
            return;
        }
        Instance = this;
    }

    #region HUD
    //update game speed
    public event Action OnGameSpeedChange;
    public void GameSpeedChange()
    {
        if (OnGameSpeedChange != null)
        {
            OnGameSpeedChange();
        }
    }

    // heal all structures
    public event Action OnHealAll;
    public void HealAll()
    {
        if (OnHealAll != null)
        {
            OnHealAll();
        }
    }

    public event Action OnShowCancel;
    public void ShowCanel()
    {
        if (OnShowCancel != null)
        {
            OnShowCancel();
        }
    }
    #endregion

    #region Item
    //Show Item Info dialog
    public event Action OnShowItemInfo;
    public void ShowItemInfo()
    {
        if (OnShowItemInfo != null)
        {
            OnShowItemInfo();
        }
    }

    //Show Item Info dialog
    public event Action OnCloseItemInfo;
    public void CloseItemInfo()
    {
        if (OnCloseItemInfo != null)
        {
            OnCloseItemInfo();
        }
    }
    #endregion

    #region Wave

    // start wave
    public event Action OnStartWave;
    public void StartWave()
    {
        if (OnStartWave != null)
        {
            OnStartWave();
        }
    }

    // start wave
    public event Action OnContinueWave;
    public void ContinueWave()
    {
        if (OnContinueWave != null)
        {
            OnContinueWave();
        }
    }

    // start wave
    public event Action OnEndWave;
    public void EndWave()
    {
        if (OnEndWave != null)
        {
            OnEndWave();
        }
    }

    #endregion

    #region Shop
    public event Action OnOpenShop;
    public void OpenShop()
    {
        if (OnOpenShop != null)
        {
            OnOpenShop();
        }
    }
    public event Action OnCloseShop;
    public void CloseShop()
    {
        if (OnCloseShop != null)
        {
            OnCloseShop();
        }
    }
    #endregion

}

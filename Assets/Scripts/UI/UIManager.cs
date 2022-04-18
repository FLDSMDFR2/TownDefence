using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<GameObject> Displays;

    public bool ShopOpen = false;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi UIManager");
            return;
        }
        Instance = this;
            
    }

    private void Start()
    {
        StartCoroutine(OpenAll());
        
        UIEvents.Instance.OnOpenShop += ShowShop;
        UIEvents.Instance.OnCloseShop += CloseAll;
        UIEvents.Instance.OnShowCancel += ShowCanel;
    }

    private IEnumerator OpenAll()
    {
        
        foreach (GameObject obj in Displays)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(.0001f);
        }

        SetDefault();
        yield return null;
    }

    public void CloseAll()
    {

        foreach (GameObject obj in Displays)
        {
            obj.SetActive(false);
        }

        ShopOpen = false;
    }

    public void SetDefault()
    {
        if (Displays.Count <= 0)
            return;

        CloseAll();
        
    }

    public void ShowShop()
    {
        if (ShopOpen)
        {
            CloseAll();
            return;
        }

        SetDefault();
        ItemSelectedManager.Instance.ClearSelectedItem();
        ShopOpen = true;
        Displays[0].SetActive(true);
    }

    public void ShowCanel()
    {
        if (Displays.Count <= 1)
            return;

        SetDefault();
        Displays[1].SetActive(true);
    }


    private void OnDestroy()
    {
        UIEvents.Instance.OnOpenShop -= ShowShop;
        UIEvents.Instance.OnCloseShop -= CloseAll;
        UIEvents.Instance.OnShowCancel -= ShowCanel;      
    }
}

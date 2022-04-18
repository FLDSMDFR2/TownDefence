using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSelectedManager : MonoBehaviour
{

    public static ItemSelectedManager Instance;

    [Header("Manager  Object")]
    public Text ObjectName;

    private GameObject itemSelected;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi ItemManager");
            return;
        }
        Instance = this;
    }

    public void SetSelectedItem(GameObject SelectedItem)
    {
        if (BuildManager.Instance.BuildSelected())
            return;

        if (itemSelected != null)
        {
            ClearSelectedItem();
        }

        itemSelected = SelectedItem;

        InitDisplay();
        DisplaySelectedItem();
    }

    public void ClearSelectedItem()
    {
        CheckForRangeDisplay(false);
        itemSelected = null;
    }

    public void ClearSelectedItemAndUIDisplay()
    {
        UIManager.Instance.SetDefault();
        ClearSelectedItem();
    }

    public void ShowItemInfo()
    {

    }
    public void UpgardeItem()
    {

    }

    public void SellItemSelected()
    {
        if (itemSelected == null)
            return;

        BasePurchasableObject obj = itemSelected.GetComponent<BasePurchasableObject>();
        if (obj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Can not delete this object");
            return;
        }

        PlayerStats.data.Money += obj.GetSellCost();

        obj.Floor.ClearStructure();
        Destroy(itemSelected);
        ClearSelectedItemAndUIDisplay();
    }

    private bool InitDisplay()
    {

        BaseSelectableObject obj = itemSelected.GetComponent<BaseSelectableObject>();
        if (obj != null)
        {

            var displayText = obj.Name;

            BaseUpgradeableObj upObj = itemSelected.GetComponent<BaseUpgradeableObj>();
            if (upObj != null && upObj.Level > 0)
            {
                displayText += "(level " + upObj.Level + ")";
            }
            
            ObjectName.text = displayText;
            return true;
        }

        return false;
    }

    private void DisplaySelectedItem()
    {
        UIManager.Instance.CloseAll();

        CheckForRangeDisplay(true);

        gameObject.SetActive(true);
    }

    private void CheckForRangeDisplay(bool showRange)
    {
        if (itemSelected == null)
            return;

        BaseTower tower = itemSelected.GetComponent<BaseTower>();
        if (tower != null)
        {
            tower.ShowRange(showRange);
        }
    }

    public bool HasItemSelected()
    {
        return itemSelected != null;
    }

    public GameObject SelectedItem()
    {
        return itemSelected;
    }
}

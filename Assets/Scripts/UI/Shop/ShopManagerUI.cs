using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ShopManagerUI : MonoBehaviour
{
    public GridLayoutGroup Shop;
    public Button ShopItemPrefab;

    private void Start()
    {
        ShowShop();
    }

    public void ShowShop()
    {
        foreach (var key in UpgradeManager.Instance.Upgrades.Keys)
        {
            List<GameObject> upgrades = UpgradeManager.Instance.Upgrades[key];
            if (upgrades.Count <= 0)
                return;

            // get first upgrade
            BaseStructure obj = upgrades[0].GetComponent<BaseStructure>();
            if (obj == null)
                return;

            ShopItemUI item = ShopItemPrefab.GetComponent<ShopItemUI>();
            if (item == null)
                return;

            item.Item = obj;
            Instantiate(ShopItemPrefab, Shop.transform);
        }
    }
}

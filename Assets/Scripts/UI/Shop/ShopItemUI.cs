using UnityEngine.UI;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    public Button ItemButton;
    public TMPro.TMP_Text DisplayText;
    public BaseStructure Item;

    void Start()
    {
        DisplayText.text = Item.Name;
        ItemButton.onClick.AddListener(ItemSelected);
    }

    private void ItemSelected()
    {
        UpgradeManager.Instance.Purchase(Item.Type);
        UIEvents.Instance.ShowCanel();
    }
}

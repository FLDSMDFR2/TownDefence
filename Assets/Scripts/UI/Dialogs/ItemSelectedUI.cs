using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectedUI : MonoBehaviour
{

    public TMPro.TMP_Text NameText;

    // Start is called before the first frame update
    void Start()
    {
        UIEvents.Instance.OnShowItemInfo += ShowSelectedItemInfo;
        UIEvents.Instance.OnCloseItemInfo += CloseSelectedItemInfo;
    }

    private void ShowSelectedItemInfo()
    {
        if (!ItemSelectedManager.Instance.HasItemSelected())
            return;

        BaseSelectableObject obj = ItemSelectedManager.Instance.SelectedItem().GetComponent<BaseSelectableObject>();
        if (obj != null)
        {

            NameText.text = obj.Name;
            gameObject.SetActive(true);
        }
    }

    private void CloseSelectedItemInfo()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIEvents.Instance.OnShowItemInfo -= ShowSelectedItemInfo;
        UIEvents.Instance.OnCloseItemInfo-= CloseSelectedItemInfo;
    }
}

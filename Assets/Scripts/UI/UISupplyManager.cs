using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISupplyManager : MonoBehaviour
{
    public Image SupplyImage;

    public void UpdateSupplyDisplay(float Precent)
    {
        SupplyImage.fillAmount = Precent;
    }
}

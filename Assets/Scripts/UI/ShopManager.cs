using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public List<GameObject> ShopObjects;

    public void Purchase(int index)
    {
        if (ShopObjects == null || (ShopObjects.Count <= 0  || ShopObjects.Count <= index))
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "ShopObjects not supported index " + index.ToString());
            return;
        }

        BuildManager.Instance.SetStructureToBuild(ShopObjects[index]);
    }
}

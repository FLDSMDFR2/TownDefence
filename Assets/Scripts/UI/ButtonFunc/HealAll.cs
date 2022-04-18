using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAll : MonoBehaviour
{

    private void Start()
    {
        UIEvents.Instance.OnHealAll += HealAllStructures;
    } 

    public void HealAllStructures()
    {
        BaseStructure[] towers = FindObjectsOfType<BaseStructure>();

        if (towers == null)
            return;

        foreach (BaseStructure t in towers)
        {
            t.Heal(-1);
        }
    }

    private void OnDestroy()
    {
        UIEvents.Instance.OnHealAll -= HealAllStructures;
    }
}

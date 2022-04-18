using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    public GameObject RangeTrigger;
    public GameObject RangeDisplay;

    private List<BaseEnemy> targets;

    public void InitRange(float range)
    {
        if (RangeTrigger == null || RangeDisplay == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Range Display attached for obj");
            return;
        }

        targets = new List<BaseEnemy>();

        float rangeRadius = range * 2;
        RangeTrigger.transform.localScale = new Vector3(rangeRadius, rangeRadius, rangeRadius);
        RangeDisplay.transform.localScale = new Vector3(rangeRadius, 0.001f, rangeRadius);
    }

    public List<BaseEnemy> GetTarget()
    {
        if (targets == null || targets.Count <= 0)
            return null;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }

        if (targets == null || targets.Count <= 0)
            return null;

        return targets;
    }

    public void ToggleRangeDisplay(bool showRange)
    {
        if (RangeDisplay == null)
            return;

        RangeDisplay.SetActive(showRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy == null)
            return;

        targets.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy == null)
            return;

        targets.Remove(enemy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private void Awake()
    {
        StructureDataManager.Instance().LoadHelper();
    }

    private void OnApplicationQuit()
    {
        Object[] saveableObj = FindObjectsOfType(typeof(BaseSaveableObj));
        foreach (Object obj in saveableObj)
        {
            BaseSaveableObj saveObj = (BaseSaveableObj)obj;
            saveObj.Save();
        }

        StructureDataManager.Instance().Save();
    }
}

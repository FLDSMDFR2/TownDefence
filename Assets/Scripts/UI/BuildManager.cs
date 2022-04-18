using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager Instance;

    private GameObject StructureToBuild;
    private GameObject TempStructureToBuild;
    private BasePurchasableObject itemToPurchase;

    void Awake()
    {
        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi BuildManager");
            return;
        }
        Instance = this;
    }

    public bool CanBuild() 
    { 
        if (!BuildSelected())
        {
            return false;
        }

        itemToPurchase = StructureToBuild.GetComponent<BasePurchasableObject>();
        if (itemToPurchase == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
            return false;
        }

        return PlayerStats.data.Money >= itemToPurchase.Cost;
    }

    public bool BuildSelected()
    {
        return StructureToBuild != null;
    }

    public void BuildStructure(BaseFloor locationToBuild)
    {
        if (!CanBuild())
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Structrue to build");
            return;
        }

        TempBuildStructureRemove();

        PerformBuild(locationToBuild, null, true);

        CancelBuild();

        PlayerStats.data.Money -= itemToPurchase.Cost;
    }

    public void PlaceStructure(BaseFloor locationToBuild, StructureData data)
    {
        if (!BuildSelected())
        {
            return;
        }

        PerformBuild(locationToBuild, data, false);

        ClearBuildStructures();
    }

    private void PerformBuild(BaseFloor locationToBuild, StructureData data, bool buildEffect)
    {
        BaseStructure pObj = StructureToBuild.GetComponent<BaseStructure>();
        if (pObj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
            return;
        }

        GameObject obj = (GameObject)Instantiate(StructureToBuild, locationToBuild.transform.position + pObj.PositionOffset, Quaternion.identity);
        pObj = obj.GetComponent<BaseStructure>();
        if (pObj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
            return;
        }

        if (data != null) pObj.SetData(data.ID);
        // add date to manager class and have structure load its data
        pObj.SetData(StructureDataManager.Instance().AddData(pObj));

        pObj.Floor = locationToBuild;
        locationToBuild.SetStructure(pObj);

        obj.transform.parent = locationToBuild.transform;

        if (pObj.BuildEffect != null && buildEffect)
        {
            GameObject effect = (GameObject)Instantiate(pObj.BuildEffect, locationToBuild.transform.position + pObj.PositionOffset, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }


    public void TempBuildStructure(BaseFloor locationToBuild)
    {
        if (!CanBuild())
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Structrue to build");
            return;
        }

        PlaceableObject obj = StructureToBuild.GetComponent<PlaceableObject>();
        if (obj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Object can not be placed");
            return;
        }

        TempStructureToBuild = (GameObject)Instantiate(StructureToBuild, locationToBuild.transform.position + obj.PositionOffset, Quaternion.identity);

        BaseTower tower = TempStructureToBuild.GetComponent<BaseTower>();
        if (tower != null)
        {
            tower.ShowRange(true);
        }

        SetLayerRecursively (TempStructureToBuild, 2); // ignore raycast
    }

    public void TempBuildStructureRemove()
    {
        if (TempStructureToBuild == null)
            return;

        Destroy(TempStructureToBuild);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public void SetStructureToBuild(GameObject buildObj)
    {
        if (buildObj == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No  Structure to build");
            return;
        }

        StructureToBuild = buildObj;
    }

    public void ClearBuildStructures()
    {
        StructureToBuild = null;
        TempStructureToBuild = null;
    }

    public void CancelBuild()
    {
        ClearBuildStructures();
        UIManager.Instance.SetDefault();
    }
}

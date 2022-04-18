using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseFloor : BaseEnvironment
{
    public FloorData data;

    private BaseStructure structure;
    private Color startColor;
    private Color hoverColor;
    private Color errorColor;
    private Renderer rend;

    public Vector3 positionOffset;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        hoverColor = Color.gray;
        errorColor = Color.red;
    }

    public void SetData(ref FloorData d)
    {
        data = d;
    }

    public bool HasStructure()
    {
        return structure != null && structure.Type != GaneObjectType.None;
    }

    public bool HasActiveStructure()
    {
        return structure != null && !structure.IsDestroyed && structure.Type != GaneObjectType.None;
    }

    public void SetStructure(BaseStructure s)
    {
        structure = s;
        data.Structure.ID = s.ID;
        data.Structure.Type = s.Type;
    }

    public void ClearStructure()
    {
        structure = null;
        data.Structure = new StructureInfoData();
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    #region Selection
    protected override void Selected()
    {
        if (HasStructure())
        {
            base.Selected();
            return;
        }

        if (!BuildManager.Instance.CanBuild())
        {
            base.Selected();
            return;
        }

        BuildManager.Instance.BuildStructure(this);
    }

    protected override void MouseEnter()
    {
        if (BuildManager.Instance.BuildSelected())
        {
            if (!BuildManager.Instance.CanBuild())
            {
                rend.material.color = errorColor;
            }
            else
            {
                BuildManager.Instance.TempBuildStructure(this);
                rend.material.color = hoverColor;
            }
        }
    }

    protected override void MouseExit()
    {
        if (BuildManager.Instance.BuildSelected())
        {
            BuildManager.Instance.TempBuildStructureRemove();
        }
        ResetFloorColor();
    }

    public void ResetFloorColor()
    {
        rend.material.color = startColor;
    }
    #endregion
}

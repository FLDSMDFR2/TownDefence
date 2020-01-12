using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseFloor : BaseEnvironment
{
    private Color startColor;
    private Color hoverColor;
    private Renderer rend;

    public GameObject Structure;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        hoverColor = Color.gray;
    }

    public bool HasStructure()
    {
        return Structure != null;
    }

    void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (HasStructure())
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "TODO REMOVE /  UPDATE");
            return;
        }

        Structure = BuildManager.Instance.GetStructureToBuild();
        if (Structure == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Structrue to build");
            return;
        }
        Instantiate(Structure, transform.position + new Vector3(0f,.2f,0f), transform.rotation);
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "OnMouseEnter");
        if (BuildManager.Instance.GetStructureToBuild() == null)
            return;

        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        //TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "OnMouseExit");
        rend.material.color = startColor;
    }
}

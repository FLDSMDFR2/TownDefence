using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseSelectableObject : PlaceableObject
{
    [Header("Selectable Object")]
    public bool Selectable;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Selected();
    }

    protected virtual void Selected()
    {
        if (Selectable == false)
        {
            ItemSelectedManager.Instance.ClearSelectedItemAndUIDisplay();
            return;
        }

        ItemSelectedManager.Instance.SetSelectedItem(gameObject);
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        MouseEnter();
    }

    protected virtual void MouseEnter() { }

    void OnMouseExit()
    {
        MouseExit();
    }

    protected virtual void MouseExit() { }
}

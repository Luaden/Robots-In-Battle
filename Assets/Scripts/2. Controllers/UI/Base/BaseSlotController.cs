using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseSlotController<T> : MonoBehaviour, IDropHandler
{
    [SerializeField] protected BaseSlotManager<T> slotManager;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<T>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }

        slotManager.HandleDrop(eventData);
        //Sets game object as a child of the slot collection and tells the manager that the selected object belongs there.
    }

    public void HandleDrag(PointerEventData eventData)
    {
        slotManager.HandleDrag(eventData);
    }
}

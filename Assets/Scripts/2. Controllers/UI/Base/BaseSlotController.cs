using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BaseSlotController<T> : MonoBehaviour, IDropHandler
{
    [SerializeField] protected BaseSlotManager<T> slotManager;
    protected T currentSlottedItem;

    public BaseSlotManager<T> SlotManager { get => slotManager; set => slotManager = value; }
    public T CurrentSlottedItem { get => currentSlottedItem; set => currentSlottedItem = value; }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<T>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }
    }

    public void HandleDrag(PointerEventData eventData)
    {
        slotManager.HandleDrag(eventData);
    }
}

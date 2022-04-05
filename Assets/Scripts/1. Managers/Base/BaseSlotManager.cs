using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseSlotManager<T> : MonoBehaviour
{
    [SerializeField] protected Canvas mainCanvas;
    protected List<BaseSlotController<T>> slotList;

    public virtual void HandleDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public abstract void HandleDrop(PointerEventData eventData);

    public abstract void RemoveItemFromCollection(T item);
    public abstract void AddItemToCollection(T item);
    public abstract void AddSlotToList(BaseSlotController<T> newSlot);
}

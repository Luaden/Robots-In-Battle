using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseSlotManager<T> : MonoBehaviour
{
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected List<BaseSlotController<T>> slotList;

    public List<BaseSlotController<T>> SlotList { get => slotList; }
    public Canvas MainCanvas { get => mainCanvas; }

    public virtual void HandleDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public abstract void HandleDrop(PointerEventData eventData, T newData, BaseSlotController<T> slot);

    public abstract void RemoveItemFromCollection(T item);
    public abstract void AddItemToCollection(T item, BaseSlotController<T> slot);
    public abstract void AddSlotToList(BaseSlotController<T> newSlot);
}

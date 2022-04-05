using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseSlotManager<T> : MonoBehaviour
{
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected List<BaseSlotController<T>> slotList;

    public virtual void HandleDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public abstract void HandleDrop(PointerEventData eventData);
}

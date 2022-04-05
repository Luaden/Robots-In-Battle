using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseDraggableObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
                                                                     IBeginDragHandler, IEndDragHandler, IDragHandler 
{
    [Header("Required Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;

    private BaseSlotController<BaseDraggableObject> slotController;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //slotController.OnPickUp();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        slotController.HandleDrag(eventData);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    // List of items <IShoppable>
    // Shopping Cart
    // creates IShoppableSlots

    private bool isPickedUp = false;
    private Transform previousParentObject;

    private BaseSlotController<ShopItemUIController> shopItemSlotController;
    public BaseSlotController<ShopItemUIController> ShopItemSlotController { get => shopItemSlotController; set => UpdateItemSlot(value); }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position = eventData.position;
        Debug.Log("dragging");
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pressed");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("released");
    }
    private void UpdateItemSlot(BaseSlotController<ShopItemUIController> newSlot)
    {
        shopItemSlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }


}

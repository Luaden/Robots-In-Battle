using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    public bool isPickedUp = false;
    public Transform previousParentObject;
    public float travelSpeed = 450.0f;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;

    private BaseSlotController<ShopItemUIController> shopItemUISlotController;
    public BaseSlotController<ShopItemUIController> ShopItemUISlotController 
    { 
        get => shopItemUISlotController; 
        set => UpdateItemSlot(value); 
    }
    public Transform PreviousParentObject 
    { 
        get => previousParentObject; 
        set => previousParentObject = value; 
    }


    public void OnDrag(PointerEventData eventData)
    {
        shopItemUISlotController.HandleDrag(eventData);
        
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
        shopItemUISlotController.SlotManager.RemoveItemFromCollection(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(shopItemUISlotController.SlotManager.MainCanvas.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(previousParentObject);
    }

    private void Awake()
    {
        draggableRectTransform = GetComponent<RectTransform>();
        draggableCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        if (isPickedUp || shopItemUISlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        Debug.Log( this.name + "is moving to slot");
        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position, 
            ShopItemUISlotController.gameObject.GetComponent<RectTransform>().position, 
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<ShopItemUIController> newSlot)
    {
        shopItemUISlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}

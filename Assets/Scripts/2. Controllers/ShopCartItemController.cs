using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{

    private bool isPickedUp = false;
    private Transform previousParentObject;
    private float travelSpeed = 450.0f;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;

    private BaseSlotController<ShopCartItemController> shopCartItemSlotController;
    public BaseSlotController<ShopCartItemController> ShopCartItemSlotController
    {
        get => shopCartItemSlotController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }


    public void OnDrag(PointerEventData eventData)
    {
        shopCartItemSlotController.HandleDrag(eventData);
        Debug.Log("dragging");

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pressed");
        transform.SetParent(shopCartItemSlotController.SlotManager.MainCanvas.transform);
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
        transform.SetParent(previousParentObject);
        Debug.Log("released");
    }

    private void Awake()
    {
        draggableRectTransform = GetComponent<RectTransform>();
        draggableCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update() => MoveToSlot();

    private void MoveToSlot()
    {
        if (isPickedUp || shopCartItemSlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            ShopCartItemSlotController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }

    private void UpdateItemSlot(BaseSlotController<ShopCartItemController> newSlot)
    {
        shopCartItemSlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }

}

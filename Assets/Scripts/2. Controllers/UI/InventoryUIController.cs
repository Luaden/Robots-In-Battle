using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    [SerializeField] protected TMP_Text itemNameText;
    [SerializeField] protected TMP_Text itemDescriptionText;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TMP_Text timeCostText;
    [SerializeField] protected TMP_Text currencyCostText;


    public bool isPickedUp = false;
    public Transform previousParentObject;
    public float travelSpeed = 450.0f;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;

    private MechComponentUIObject itemUIObject;
    public MechComponentUIObject ItemUIObject { get => itemUIObject; }

    private BaseSlotController<InventoryUIController> inventorySlotController;
    public BaseSlotController<InventoryUIController> InventorySlotController
    {
        get => inventorySlotController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }

    public void InitUI(MechComponentUIObject mechComponent)
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        itemNameText = texts[0];
        itemDescriptionText = texts[1];
        itemImage = GetComponentInChildren<Image>(true);
        timeCostText = texts[2];
        currencyCostText = texts[3];


        itemNameText.text = mechComponent.ComponentName;
        itemDescriptionText.text = mechComponent.ComponentElement.ToString();
        itemImage.sprite = mechComponent.ComponentSprite;

        this.itemUIObject = mechComponent;
        mechComponent.MechComponentUIController = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventorySlotController.HandleDrag(eventData);

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
        transform.SetParent(inventorySlotController.SlotManager.MainCanvas.transform);
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
        if (isPickedUp || inventorySlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            InventorySlotController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<InventoryUIController> newSlot)
    {
        inventorySlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}


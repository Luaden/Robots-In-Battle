using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ComponentShopVendorUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
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

    private ShopItemUIObject shopItemUIObject;
    public ShopItemUIObject ShopItemUIObject { get => shopItemUIObject; }

    private BaseSlotController<ComponentShopVendorUIController> componentShopSlotUIController;
    public BaseSlotController<ComponentShopVendorUIController> ComponentShopSlotUIController
    {
        get => componentShopSlotUIController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }

    public void InitUI(ShopItemUIObject shopItemUIObject)
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        itemNameText = texts[0];
        itemDescriptionText = texts[1];
        itemImage = GetComponentInChildren<Image>(true);
        timeCostText = texts[2];
        currencyCostText = texts[3];

        itemNameText.text = shopItemUIObject.ItemName;
        itemDescriptionText.text = shopItemUIObject.ItemDescription;
        itemImage.sprite = shopItemUIObject.ItemImage;
        timeCostText.text = shopItemUIObject.TimeCost.ToString();
        currencyCostText.text = shopItemUIObject.CurrencyCost.ToString();

        this.shopItemUIObject = shopItemUIObject;
        shopItemUIObject.ShopItemUIController = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        componentShopSlotUIController.HandleDrag(eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
        componentShopSlotUIController.SlotManager.RemoveItemFromCollection(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(componentShopSlotUIController.SlotManager.MainCanvas.transform);
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
        if (isPickedUp || componentShopSlotUIController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            ComponentShopSlotUIController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<ComponentShopVendorUIController> newSlot)
    {
        componentShopSlotUIController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CardShopCartUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
                              IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                              IEndDragHandler, IDragHandler
{
    [SerializeField] protected TMP_Text itemNameText;
    [SerializeField] protected TMP_Text itemDescriptionText;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TMP_Text timeCostText;
    [SerializeField] protected TMP_Text currencyCost;


    public bool isPickedUp = false;
    public Transform previousParentObject;
    public float travelSpeed = 450.0f;

    private RectTransform draggableRectTransform;
    private CanvasGroup draggableCanvasGroup;

    private ShopItemUIObject shopItemUIObject;
    public ShopItemUIObject ShopItemUIObject { get => shopItemUIObject; }

    private BaseSlotController<CardShopCartUIController> cardShopCartSlotController;
    public BaseSlotController<CardShopCartUIController> CardShopCartSlotController
    {
        get => cardShopCartSlotController;
        set => UpdateItemSlot(value);
    }
    public Transform PreviousParentObject
    {
        get => previousParentObject;
        set => previousParentObject = value;
    }

    public void InitUI(ShopItemUIObject shopItemUIObject)
    {
        itemNameText.text = shopItemUIObject.ItemName;
        itemDescriptionText.text = shopItemUIObject.ItemDescription;
        itemImage.sprite = shopItemUIObject.ItemImage;
        timeCostText.text = shopItemUIObject.TimeCost.ToString();
        currencyCost.text = shopItemUIObject.CurrencyCost.ToString();

        this.shopItemUIObject = shopItemUIObject;
        shopItemUIObject.ShopItemUIController = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        cardShopCartSlotController.HandleDrag(eventData);

    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.blocksRaycasts = false;
        draggableCanvasGroup.alpha = .6f;
        cardShopCartSlotController.SlotManager.RemoveItemFromCollection(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.blocksRaycasts = true;
        draggableCanvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(cardShopCartSlotController.SlotManager.MainCanvas.transform);
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
        if (isPickedUp || cardShopCartSlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(previousParentObject);

        draggableRectTransform.position =
            Vector3.MoveTowards(draggableRectTransform.position,
            CardShopCartSlotController.gameObject.GetComponent<RectTransform>().position,
            travelSpeed * Time.deltaTime);
    }
    private void UpdateItemSlot(BaseSlotController<CardShopCartUIController> newSlot)
    {
        cardShopCartSlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}

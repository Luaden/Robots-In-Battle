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
    [Header("Card Attributes")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardName;

    [Header("Channel Icons")]
    [SerializeField] private GameObject highChannelIcon;
    [SerializeField] private GameObject midChannelIcon;
    [SerializeField] private GameObject lowChannelIcon;

    [Header("Required Interaction Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;

    [Header("Interaction Attributes")]
    [SerializeField] private float travelSpeed;
    private Transform previousParentObject;

    [Header("Card Frames")]
    [SerializeField] private Sprite attackFrame;
    [SerializeField] private Sprite defenseFrame;
    [SerializeField] private Sprite neutralFrame;

    [Header("Card Icons")]
    [SerializeField] private Sprite punchIcon;
    [SerializeField] private Sprite kickIcon;
    [SerializeField] private Sprite specialIcon;
    [SerializeField] private Sprite counterIcon;
    [SerializeField] private Sprite guardIcon;

    [SerializeField] private TMP_Text currencyCost;

    public bool isPickedUp = false;

    private ShopItemUIObject shopItemUIObject;
    public ShopItemUIObject ShopItemUIObject { get => shopItemUIObject; }

    private BaseSlotController<CardShopCartUIController> cardShopCartSlotController;
    public BaseSlotController<CardShopCartUIController> CardShopCartSlotController { get => cardShopCartSlotController; set => UpdateItemSlot(value); }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }

    public void InitUI(ShopItemUIObject shopItemUIObject)
    {
        cardName.text = shopItemUIObject.ItemName;
        this.shopItemUIObject = shopItemUIObject;
        shopItemUIObject.ShopItemUIController = this.gameObject;

        switch (shopItemUIObject.CardCategory)
        {
            case CardCategory.Punch:
                cardImage.sprite = punchIcon;
                cardImage.SetNativeSize();
                break;
            case CardCategory.Kick:
                cardImage.sprite = kickIcon;
                cardImage.SetNativeSize();
                break;
            case CardCategory.Special:
                cardImage.sprite = specialIcon;
                cardImage.SetNativeSize();
                break;
            case CardCategory.Guard:
                cardImage.sprite = guardIcon;
                cardImage.SetNativeSize();
                break;
            case CardCategory.Counter:
                cardImage.sprite = counterIcon;
                cardImage.SetNativeSize();
                break;
        }

        switch (shopItemUIObject.CardType)
        {
            case CardType.Attack:
                cardBackground.sprite = attackFrame;
                break;
            case CardType.Defense:
                cardBackground.sprite = defenseFrame;
                break;
            case CardType.Neutral:
                cardBackground.sprite = neutralFrame;
                break;
        }

        if (shopItemUIObject.PossibleChannels.HasFlag(Channels.High))
            highChannelIcon.SetActive(true);
        if (shopItemUIObject.PossibleChannels.HasFlag(Channels.Mid))
            midChannelIcon.SetActive(true);
        if (shopItemUIObject.PossibleChannels.HasFlag(Channels.Low))
            lowChannelIcon.SetActive(true);

        currencyCost.text = shopItemUIObject.CurrencyCost.ToString();


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

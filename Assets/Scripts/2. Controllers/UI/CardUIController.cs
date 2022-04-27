using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    private CardDataObject cardData;
    private BaseSlotController<CardUIController> cardSlotController;
    
    private bool isPickedUp = false;

    public CardDataObject CardData { get => cardData; }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }
    public BaseSlotController<CardUIController> CardSlotController { get => cardSlotController; set => UpdateCardSlot(value); }

    public void InitCardUI(CardDataObject newCardData)
    {
        cardName.text = newCardData.CardName;
        cardData = newCardData;
        newCardData.CardUIObject = this.gameObject;

        switch (newCardData.CardCategory)
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

        switch (newCardData.CardType)
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

        if(newCardData.PossibleChannels.HasFlag(Channels.High))
            highChannelIcon.SetActive(true);
        if (newCardData.PossibleChannels.HasFlag(Channels.Mid))
            midChannelIcon.SetActive(true);
        if (newCardData.PossibleChannels.HasFlag(Channels.Low))
            lowChannelIcon.SetActive(true);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(cardData, transform, eventData.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.InactivatePopup();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(cardSlotController.SlotManager.MainCanvas.transform);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(previousParentObject);
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

    public void OnDrag(PointerEventData eventData)
    {
        cardSlotController.HandleDrag(eventData);
    }

    private void Update()
    {
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        if (isPickedUp || cardSlotController == null)
            return;

        if (transform.parent == null)
            transform.SetParent(PreviousParentObject);

        draggableRectTransform.position = 
            Vector3.MoveTowards(draggableRectTransform.position, CardSlotController.gameObject.GetComponent<RectTransform>().position, travelSpeed * Time.deltaTime);
    }

    private void UpdateCardSlot(BaseSlotController<CardUIController> newSlot)
    {
        cardSlotController = newSlot;
        transform.SetParent(newSlot.transform);
        previousParentObject = newSlot.transform;
    }
}
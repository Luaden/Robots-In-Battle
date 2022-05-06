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
    [SerializeField] private Image highChannelIcon;
    [SerializeField] private Image midChannelIcon;
    [SerializeField] private Image lowChannelIcon;

    [Header("Required Interaction Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;

    [Header("Interaction Attributes")]
    [SerializeField] private float travelSpeed;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadeColor;
    private Transform previousParentObject;
    private bool isPlayer;

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
    private Animator cardAnimator;
    private BaseSlotController<CardUIController> cardSlotController;
    
    private bool isPickedUp = false;

    public delegate void onPickUp(Channels channel);
    public static event onPickUp OnPickUp;

    public CardDataObject CardData { get => cardData; }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }
    public Animator CardAnimator { get => cardAnimator; }
    public BaseSlotController<CardUIController> CardSlotController { get => cardSlotController; set => UpdateCardSlot(value); }

    public void InitCardUI(CardDataObject newCardData, CharacterSelect character)
    {
        cardName.text = newCardData.CardName;
        cardData = newCardData;
        newCardData.CardUIObject = this.gameObject;
        cardAnimator = GetComponent<Animator>();

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

        if (newCardData.PossibleChannels.HasFlag(Channels.High))
            highChannelIcon.color = fullColor;
        if (newCardData.PossibleChannels.HasFlag(Channels.Mid))
            midChannelIcon.color = fullColor;
        if (newCardData.PossibleChannels.HasFlag(Channels.Low))
            lowChannelIcon.color = fullColor;

        if (character == CharacterSelect.Player)
            isPlayer = true;
        if (character == CharacterSelect.Opponent)
            isPlayer = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(cardData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.InactivatePopup();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (isPlayer && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = true;
            transform.SetParent(cardSlotController.SlotManager.MainCanvas.transform);
            OnPickUp.Invoke(cardData.PossibleChannels);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isPlayer && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = false;
            transform.SetParent(previousParentObject);
            OnPickUp.Invoke(Channels.None);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(isPlayer && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = true;
            draggableCanvasGroup.blocksRaycasts = false;
            draggableCanvasGroup.alpha = .6f;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isPlayer && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = false;
            draggableCanvasGroup.blocksRaycasts = true;
            draggableCanvasGroup.alpha = 1f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isPlayer && CombatManager.instance.CanPlayCards)
            cardSlotController.HandleDrag(eventData);
    }

    public void UpdateSelectedChannel(Channels channel)
    {
        if(isPlayer || CombatManager.instance.DisplayAIDecisionIndicator)
        {
            if (channel.HasFlag(Channels.High))
                highChannelIcon.color = fullColor;
            else
                highChannelIcon.color = fadeColor;

            if (channel.HasFlag(Channels.Mid))
                midChannelIcon.color = fullColor;
            else
                midChannelIcon.color = fadeColor;

            if (channel.HasFlag(Channels.Low))
                lowChannelIcon.color = fullColor;
            else
                lowChannelIcon.color = fadeColor;
        }
    }

    private void Update()
    {
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        if (isPickedUp)
            return;

        if (transform.parent == null)
            transform.SetParent(PreviousParentObject);

        if (transform.parent != null)
        {
            draggableRectTransform.position =
                    Vector3.MoveTowards(draggableRectTransform.position, transform.parent.position, travelSpeed * FindObjectOfType<Canvas>().scaleFactor * Time.deltaTime);
        }
    }

    private void UpdateCardSlot(BaseSlotController<CardUIController> newSlot)
    {
        cardSlotController = newSlot;

        if (newSlot != null)
        {
            previousParentObject = newSlot.transform;
            transform.SetParent(newSlot.transform);
        }
    }

    private void DestroyCardUI()
    {
        Destroy(gameObject);
    }
}
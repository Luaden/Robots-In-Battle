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
    [SerializeField] private GameObject cardNameAttackObject;
    [SerializeField] private TMP_Text cardNameAttack;
    [SerializeField] private GameObject cardNameDefenseObject;
    [SerializeField] private TMP_Text cardNameDefense;
    [SerializeField] private GameObject cardNameNeutralObject;
    [SerializeField] private TMP_Text cardNameNeutral;

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
    [SerializeField] private bool isPlayerCard;

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

    public CardDataObject CardData { get => cardData; }
    public bool IsPlayerCard { get => isPlayerCard; }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }
    public Animator CardAnimator { get => cardAnimator; }
    public BaseSlotController<CardUIController> CardSlotController { get => cardSlotController; set => UpdateCardSlot(value); }

    public delegate void onPickUp(Channels destinationChannel, MechSelect destinationMech, Channels originChannel);
    public static event onPickUp OnPickUp;

    public void InitCardUI(CardDataObject newCardData, CharacterSelect character)
    {
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
                cardNameAttack.text = newCardData.CardName;
                cardNameAttackObject.SetActive(true);
                break;
            case CardType.Defense:
                cardBackground.sprite = defenseFrame;
                cardNameDefense.text = newCardData.CardName;
                cardNameDefenseObject.SetActive(true);
                break;
            case CardType.Neutral:
                cardBackground.sprite = neutralFrame;
                cardNameNeutral.text = newCardData.CardName;
                cardNameNeutralObject.SetActive(true);
                break;
        }

        if (newCardData.PossibleChannels.HasFlag(Channels.High))
            highChannelIcon.color = fullColor;
        if (newCardData.PossibleChannels.HasFlag(Channels.Mid))
            midChannelIcon.color = fullColor;
        if (newCardData.PossibleChannels.HasFlag(Channels.Low))
            lowChannelIcon.color = fullColor;

        if (character == CharacterSelect.Player)
            isPlayerCard = true;
        if (character == CharacterSelect.Opponent)
            isPlayerCard = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(cardData);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.ClearCardUIPopup();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (isPlayerCard && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = true;
            transform.SetParent(cardSlotController.SlotManager.MainCanvas.transform);

            if (CardCategory.Offensive.HasFlag(cardData.CardCategory))
            {
                switch (cardData.CardCategory)
                {
                    case CardCategory.Punch:
                        OnPickUp?.Invoke(cardData.PossibleChannels, MechSelect.Opponent, Channels.High);
                        break;
                    case CardCategory.Kick:
                        OnPickUp?.Invoke(cardData.PossibleChannels, MechSelect.Opponent, Channels.Low);
                        break;
                    case CardCategory.Special:
                        OnPickUp?.Invoke(cardData.PossibleChannels, MechSelect.Opponent, Channels.Mid);
                        break;
                }
            }
            else
                OnPickUp?.Invoke(cardData.PossibleChannels, MechSelect.Player, Channels.None);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isPlayerCard && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = false;
            transform.SetParent(previousParentObject);
            OnPickUp?.Invoke(Channels.None, MechSelect.None, Channels.None);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(isPlayerCard && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = true;
            draggableCanvasGroup.blocksRaycasts = false;
            draggableCanvasGroup.alpha = .6f;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isPlayerCard && CombatManager.instance.CanPlayCards)
        {
            isPickedUp = false;
            draggableCanvasGroup.blocksRaycasts = true;
            draggableCanvasGroup.alpha = 1f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isPlayerCard && CombatManager.instance.CanPlayCards)
            cardSlotController.HandleDrag(eventData);
    }

    public void UpdateSelectedChannel(Channels channel)
    {
        if(isPlayerCard || CombatManager.instance.DisplayAIDecisionIndicator)
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
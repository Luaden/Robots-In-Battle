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
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;
    [SerializeField] private Image highlightImage;

    [Header("Required Interaction Components")]
    [SerializeField] private RectTransform draggableRectTransform;
    [SerializeField] private CanvasGroup draggableCanvasGroup;

    [Header("Interaction Attributes")]
    [SerializeField] private float travelSpeed;
    private Transform previousParentObject;

    private CardDataObject cardData;
    private BaseSlotController<CardUIController> cardSlotController;
    
    private bool isPickedUp = false;
    private Vector2 originPosition;

    public CardDataObject CardData { get => cardData; }
    public Transform PreviousParentObject { get => previousParentObject; set => previousParentObject = value; }
    public BaseSlotController<CardUIController> CardSlotController { get => cardSlotController; set => UpdateCardSlot(value); }

    public void InitCardUI(CardDataObject newCardData)
    {
        cardBackground.sprite = newCardData.CardBackground;
        cardImage.sprite = newCardData.CardForeground;
        cardName.text = newCardData.CardName;
        cardDescription.text = newCardData.CardDescription;
        energyCostText.text = newCardData.EnergyCost.ToString();
        damageDealtText.text = newCardData.BaseDamage.ToString();

        //Need to somehow indicate possible channels and affected channels in here. Possibly description?

        cardData = newCardData;
        newCardData.CardUIObject = this.gameObject;
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
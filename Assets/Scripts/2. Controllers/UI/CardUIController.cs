using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUIController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    private CardDataObject cardData;
    private CardSlotController cardSlotController;
    private bool isPickedUp = false;

    public CardDataObject CardData { get => cardData; }
    public CardSlotController CardSlotController { get => cardSlotController; set => cardSlotController = value; }

    public void InitCardUI(CardDataObject newCardData)
    {
        cardBackground.sprite = newCardData.CardBackground;
        cardImage.sprite = newCardData.CardForeground;
        cardName.text = newCardData.CardName;
        cardDescription.text = newCardData.CardDescription;
        energyCostText.text = newCardData.EnergyCost.ToString();
        damageDealtText.text = newCardData.BaseDamage.ToString();

        cardData = newCardData;
        newCardData.CardUIObject = this.gameObject;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isPickedUp = true;
        draggableCanvasGroup.interactable = false;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isPickedUp = false;
        draggableCanvasGroup.interactable = true;
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

        draggableRectTransform.position = 
            Vector2.Lerp(draggableRectTransform.position, CardSlotController.gameObject.GetComponent<RectTransform>().position, travelSpeed * Time.deltaTime);
    }
}
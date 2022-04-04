using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIObject : MonoBehaviour
{
    [Header("Card Attributes")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;
    [SerializeField] private Image highlightImage;
    private CardDataObject cardData;

    private bool isHighlighted = true;
    public bool isPickedUp = false;

    public CardDataObject CardData { get => cardData; }

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
}
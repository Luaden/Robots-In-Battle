using System;
using UnityEngine;
using TMPro;

public class ShopCardUIPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("General Popup Attributes")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] protected GameObject cardPopupObject;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text cardEnergyCostText;
    [SerializeField] private TMP_Text cardDamageDealtText;
    [SerializeField] private TMP_Text cardCurrencyCostText;

    private void Awake()
    {
        mainCanvas = FindObjectOfType<Canvas>(true);
    }

    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        if (primaryData.ItemType == ItemType.Card)
        {
            cardNameText.text = primaryData.CardName;
            cardDescriptionText.text = primaryData.CardDescription;
            cardEnergyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
            cardDamageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = ("Price: ") + primaryData.CurrencyCost.ToString();
        }

        Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

        if (mousePosition.x + cardPopupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
        {
            mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - cardPopupObject.GetComponent<RectTransform>().rect.width;
        }

        if (mousePosition.y + cardPopupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
        {
            mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - cardPopupObject.GetComponent<RectTransform>().rect.height;
        }

        cardPopupObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
        cardPopupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(SOItemDataObject newData)
    {
        if (newData == null)
        {
            cardNameText.text = string.Empty;

            cardDescriptionText.text = string.Empty;
            cardEnergyCostText.text = string.Empty;
            cardDamageDealtText.text = string.Empty;

            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = string.Empty;

            cardPopupObject.SetActive(false);

            return true;
        }

        return false;
    }
}

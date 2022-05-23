using System;
using UnityEngine;
using TMPro;

public class ShopCardUIPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("General Popup Attributes")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text timeCostText;

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
            nameText.text = primaryData.CardName;
            descriptionText.text = primaryData.CardDescription;
            energyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
            damageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

            if (costText != null)
                costText.text = primaryData.CurrencyCost.ToString();
            if (timeCostText != null)
                timeCostText.text = primaryData.TimeCost.ToString();
        }

        Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

        if (mousePosition.x + popupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
        {
            mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - popupObject.GetComponent<RectTransform>().rect.width;
        }

        if (mousePosition.y + popupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
        {
            mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - popupObject.GetComponent<RectTransform>().rect.height;
        }

        popupObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
        popupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(SOItemDataObject newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;

            descriptionText.text = string.Empty;
            energyCostText.text = string.Empty;
            damageDealtText.text = string.Empty;

            if (costText != null)
                costText.text = string.Empty;
            if (timeCostText != null)
                timeCostText.text = string.Empty;

            popupObject.SetActive(false);

            return true;
        }

        return false;
    }
}

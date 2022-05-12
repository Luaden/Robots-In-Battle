using System;
using UnityEngine;
using TMPro;

public class ShopCardUIPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("General Popup Attributes")]
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;

    [Header("Card Popup Text")]
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;


    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        nameText.text = primaryData.ItemName;
        descriptionText.text = primaryData.ItemDescription;
        energyCostText.text = primaryData.EnergyCost.ToString();
        damageDealtText.text = primaryData.BaseDamage.ToString();

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

            popupObject.SetActive(false);
            return true;
        }

        return false;
    }
}

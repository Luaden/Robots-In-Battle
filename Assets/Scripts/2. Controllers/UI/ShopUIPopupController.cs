using System;
using UnityEngine;
using TMPro;

public class ShopUIPopupController : BaseUIElement<ShopItemUIController>
{
    [Header("General Popup Attributes")]
    [SerializeField] protected GameObject popupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject cardTextBlock;
    [SerializeField] private GameObject componentTextBlock;

    [Header("Card Popup Text")]
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text damageDealtText;

    [Header("Component Popup Text")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text cDMText;
    [SerializeField] private TMP_Text elementText;



    public override void UpdateUI(ShopItemUIController primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        //if(primaryData.ItemType == ItemType.Card)
        //{
        //    nameText.text = primaryData.ItemName;
        //    descriptionText.text = primaryData.ItemDescription;
        //    energyCostText.text = primaryData.EnergyCost.ToString();
        //    damageDealtText.text = primaryData.BaseDamage.ToString();
        //    cardTextBlock.SetActive(true);
        //}

        //if(primaryData.ItemType == ItemType.Component)
        //{
        //    healthText.text = primaryData.ComponentHP.ToString();
        //    energyText.text = primaryData.ComponentEnergy.ToString();
        //    cDMText.text = primaryData.CDMFromComponent.ToString();
        //    elementText.text = Enum.GetName(typeof(ElementType), primaryData.ComponentElement);
        //    componentTextBlock.SetActive(true);
        //}

        popupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(ShopItemUIController newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;

            descriptionText.text = string.Empty;
            energyCostText.text = string.Empty;
            damageDealtText.text = string.Empty;

            healthText.text = string.Empty;
            energyText.text = string.Empty;
            cDMText.text = string.Empty;
            elementText.text = string.Empty;

            popupObject.SetActive(false);
            cardTextBlock.SetActive(false);
            componentTextBlock.SetActive(false);
            return true;
        }

        return false;
    }
}

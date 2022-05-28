using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkshopItemPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("Popup Settings")]
    [SerializeField] private float currentItemPopupDelay;
    [Space]
    [Header("Shop Card Popup Attributes")]
    [SerializeField] protected GameObject cardPopupObject;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text cardEnergyCostText;
    [SerializeField] private TMP_Text cardDamageDealtText;
    [SerializeField] private TMP_Text cardCurrencyCostText;
    [Space]
    [Header("Shop New Component")]
    [SerializeField] protected GameObject componentPopupObject;
    [SerializeField] private TMP_Text componentNameText;
    [SerializeField] private TMP_Text componentHealthText;
    [SerializeField] private TMP_Text componentEnergyText;
    [SerializeField] private TMP_Text componentEnergyGainText;
    [SerializeField] private TMP_Text componentElementText;
    [SerializeField] private TMP_Text currencyCostText;
    [Space]
    [Header("Shop Current Equipped Component")]
    [SerializeField] private GameObject currentComponentPopupObject;
    [SerializeField] private TMP_Text currentComponentNameText;
    [SerializeField] private TMP_Text currentComponentHealthText;
    [SerializeField] private TMP_Text currentComponentEnergyText;
    [SerializeField] private TMP_Text currentComponentEnergyGainText;
    [SerializeField] private TMP_Text currentComponentElementText;
    [Space]
    [Header("Inventory Card Popup Attributes")]
    [SerializeField] protected GameObject inventoryCardPopupObject;
    [SerializeField] private TMP_Text inventoryCardNameText;
    [SerializeField] private TMP_Text inventoryCardDescriptionText;
    [SerializeField] private TMP_Text inventoryCardEnergyCostText;
    [SerializeField] private TMP_Text inventoryCardDamageDealtText;
    [Space]
    [Header("Inventory Component")]
    [SerializeField] protected GameObject inventoryComponentPopupObject;
    [SerializeField] private TMP_Text inventoryComponentNameText;
    [SerializeField] private TMP_Text inventoryComponentHealthText;
    [SerializeField] private TMP_Text inventoryComponentEnergyText;
    [SerializeField] private TMP_Text inventoryComponentEnergyGainText;
    [SerializeField] private TMP_Text inventoryComponentElementText;
    [Space]
    [Header("Inventory Equipped Component")]
    [SerializeField] private GameObject inventoryCurrentComponentPopupObject;
    [SerializeField] private TMP_Text inventoryCurrentComponentNameText;
    [SerializeField] private TMP_Text inventoryCurrentComponentHealthText;
    [SerializeField] private TMP_Text inventoryCurrentComponentEnergyText;
    [SerializeField] private TMP_Text inventoryCurrentComponentEnergyGainText;
    [SerializeField] private TMP_Text inventoryCurrentComponentElementText;


    private float currentTimer;
    private bool popupQueued;

    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        if (DowntimeManager.instance.CurrentLocation == WorkshopLocation.Shop)
        {
            HandleShopPopups(primaryData);
        }
        else
        {
            HandleInventoryPopups(primaryData);
        }
    }

    private void HandleShopPopups(SOItemDataObject primaryData)
    {
        if (primaryData.ItemType == ItemType.Component)
        {
            MechComponentDataObject currentItem = null;

            switch (primaryData.ComponentType)
            {
                case MechComponent.Head:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechHead;
                    break;
                case MechComponent.Torso:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechTorso;
                    break;
                case MechComponent.Arms:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechArms;
                    break;
                case MechComponent.Legs:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechLegs;
                    break;
            }

            componentNameText.text = primaryData.ItemName;
            componentHealthText.text = ("Health: ") + primaryData.ComponentHP.ToString();
            componentEnergyText.text = ("Energy: ") + primaryData.ComponentEnergy.ToString();
            componentEnergyGainText.text = ("Bonus Energy Gain: ") + primaryData.EnergyGainModifier.ToString();
            componentElementText.text = ("Element: ") + Enum.GetName(typeof(ElementType), primaryData.ComponentElement);
            currencyCostText.text = ("Price: ") + primaryData.CurrencyCost.ToString();

            currentComponentNameText.text = currentItem.ComponentName;
            currentComponentHealthText.text = ("Health: ") + currentItem.ComponentMaxHP.ToString();
            currentComponentEnergyText.text = ("Energy: ") + currentItem.ComponentMaxEnergy.ToString();
            currentComponentEnergyGainText.text = ("Bonus Energy Gain: ") + currentItem.EnergyGainModifier.ToString();
            currentComponentElementText.text = ("Element: ") + Enum.GetName(typeof(ElementType), currentItem.ComponentElement);

            popupQueued = true;
            componentPopupObject.SetActive(true);
            return;
        }

        if (primaryData.ItemType == ItemType.Card)
        {
            cardNameText.text = primaryData.CardName;
            cardDescriptionText.text = primaryData.CardDescription;
            cardEnergyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
            cardDamageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = ("Price: ") + primaryData.CurrencyCost.ToString();

            cardPopupObject.SetActive(true);
        }
    }

    private void HandleInventoryPopups(SOItemDataObject primaryData)
    {
        if (primaryData.ItemType == ItemType.Component)
        {
            MechComponentDataObject currentItem = null;

            switch (primaryData.ComponentType)
            {
                case MechComponent.Head:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechHead;
                    break;
                case MechComponent.Torso:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechTorso;
                    break;
                case MechComponent.Arms:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechArms;
                    break;
                case MechComponent.Legs:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechLegs;
                    break;
            }

            inventoryComponentNameText.text = primaryData.ItemName;
            inventoryComponentHealthText.text = ("Health: ") + primaryData.ComponentHP.ToString();
            inventoryComponentEnergyText.text = ("Energy: ") + primaryData.ComponentEnergy.ToString();
            inventoryComponentEnergyGainText.text = ("Bonus Energy Gain: ") + primaryData.EnergyGainModifier.ToString();
            inventoryComponentElementText.text = ("Element: ") + Enum.GetName(typeof(ElementType), primaryData.ComponentElement);

            inventoryCurrentComponentNameText.text = currentItem.ComponentName;
            inventoryCurrentComponentHealthText.text = ("Health: ") + currentItem.ComponentMaxHP.ToString();
            inventoryCurrentComponentEnergyText.text = ("Energy: ") + currentItem.ComponentMaxEnergy.ToString();
            inventoryCurrentComponentEnergyGainText.text = ("Bonus Energy Gain: ") + currentItem.EnergyGainModifier.ToString();
            inventoryCurrentComponentElementText.text = ("Element: ") + Enum.GetName(typeof(ElementType), currentItem.ComponentElement);

            popupQueued = true;

            inventoryComponentPopupObject.SetActive(true);
        }

        if (primaryData.ItemType == ItemType.Card)
        {
            inventoryCardNameText.text = primaryData.CardName;
            inventoryCardDescriptionText.text = primaryData.CardDescription;
            inventoryCardEnergyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
            inventoryCardDamageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

            inventoryCardPopupObject.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(SOItemDataObject newData)
    {
        if (newData == null)
        {
            componentNameText.text = string.Empty;
            componentHealthText.text = string.Empty;
            componentEnergyGainText.text = string.Empty;
            componentElementText.text = string.Empty;

            currentComponentNameText.text = string.Empty;
            currentComponentHealthText.text = string.Empty;
            currentComponentEnergyGainText.text = string.Empty;
            currentComponentElementText.text = string.Empty;

            cardNameText.text = string.Empty;
            cardDescriptionText.text = string.Empty;
            cardEnergyCostText.text = string.Empty;
            cardDamageDealtText.text = string.Empty;


            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = string.Empty;

            inventoryComponentNameText.text = string.Empty;
            inventoryComponentHealthText.text = string.Empty;
            inventoryComponentEnergyGainText.text = string.Empty;
            inventoryComponentElementText.text = string.Empty;

            inventoryCurrentComponentNameText.text = string.Empty;
            inventoryCurrentComponentHealthText.text = string.Empty;
            inventoryCurrentComponentEnergyGainText.text = string.Empty;
            inventoryCurrentComponentElementText.text = string.Empty;

            inventoryCardNameText.text = string.Empty;
            inventoryCardDescriptionText.text = string.Empty;
            inventoryCardEnergyCostText.text = string.Empty;
            inventoryCardDamageDealtText.text = string.Empty;

            inventoryCardPopupObject.SetActive(false);
            inventoryComponentPopupObject.SetActive(false);
            inventoryCurrentComponentPopupObject.SetActive(false);
            cardPopupObject.SetActive(false);
            componentPopupObject.SetActive(false);
            currentComponentPopupObject.SetActive(false);

            currentTimer = 0f;
            popupQueued = false;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (popupQueued)
        {
            CheckTimer();
        }
    }

    private void CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= currentItemPopupDelay)
        {
            if (DowntimeManager.instance.CurrentLocation == WorkshopLocation.Shop)
                currentComponentPopupObject.SetActive(true);
            else
                inventoryCurrentComponentPopupObject.SetActive(true);
            currentTimer = 0f;
            popupQueued = false;
        }
    }
}
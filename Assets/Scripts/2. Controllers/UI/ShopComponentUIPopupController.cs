using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopComponentUIPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("Card Popup Attributes")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] protected GameObject cardPopupObject;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text cardEnergyCostText;
    [SerializeField] private TMP_Text cardDamageDealtText;
    [SerializeField] private TMP_Text cardCurrencyCostText;
    [Space]
    [Header("New Component")]
    [SerializeField] protected GameObject componentPopupObject;
    [SerializeField] private TMP_Text componentNameText;
    [SerializeField] private TMP_Text componentHealthText;
    [SerializeField] private TMP_Text componentEnergyText;
    [SerializeField] private TMP_Text componentEnergyGainText;
    [SerializeField] private TMP_Text componentElementText;
    [SerializeField] private TMP_Text currencyCostText;
    [Space]
    [Header("Current Equipped Component")]
    [SerializeField] private GameObject currentComponentPopupObject;
    [SerializeField] private TMP_Text currentComponentNameText;
    [SerializeField] private TMP_Text currentComponentHealthText;
    [SerializeField] private TMP_Text currentComponentEnergyText;
    [SerializeField] private TMP_Text currentComponentEnergyGainText;
    [SerializeField] private TMP_Text currentComponentElementText;
    [Space]
    [Header("Popup Settings")]
    [SerializeField] private float currentItemPopupDelay;

    private float currentTimer;
    private bool popupQueued;

    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        if(primaryData.ItemType == ItemType.Component)
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
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechHead;
                    break;
                case MechComponent.Legs:
                    currentItem = GameManager.instance.PlayerMechController.PlayerMech.MechLegs;
                    break;
            }

            componentNameText.text = primaryData.ItemName;
            componentHealthText.text = ("Health: ") + primaryData.ComponentHP.ToString();
            componentEnergyGainText.text = ("Bonus Energy Gain: ") + primaryData.EnergyGainModifier.ToString();
            componentElementText.text = ("Element: ") + System.Enum.GetName(typeof(ElementType), primaryData.ComponentElement);
            currencyCostText.text = primaryData.CurrencyCost.ToString();

            currentComponentNameText.gameObject.SetActive(true);
            currentComponentNameText.text = currentItem.ComponentName;
            currentComponentHealthText.gameObject.SetActive(true);
            currentComponentHealthText.text = ("Health: ") + currentItem.ComponentMaxHP.ToString();
            currentComponentEnergyText.gameObject.SetActive(true);
            currentComponentEnergyText.text = ("Energy: ") + currentItem.ComponentMaxEnergy.ToString();
            currentComponentEnergyGainText.gameObject.SetActive(true);
            currentComponentEnergyGainText.text = ("Bonus Energy Gain: ") + currentItem.EnergyGainModifier.ToString();
            currentComponentElementText.gameObject.SetActive(true);
            currentComponentElementText.text = System.Enum.GetName(typeof(ElementType), currentItem.ComponentElement);

            popupQueued = true;

            //Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

            //if (mousePosition.x + cardPopupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
            //{
            //    mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - cardPopupObject.GetComponent<RectTransform>().rect.width;
            //}

            //if (mousePosition.y + cardPopupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
            //{
            //    mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - cardPopupObject.GetComponent<RectTransform>().rect.height;
            //}

            //componentPopupObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
            componentPopupObject.SetActive(true);
        }

        if (primaryData.ItemType == ItemType.Card)
        {
            cardNameText.text = primaryData.CardName;
            cardDescriptionText.text = primaryData.CardDescription;
            cardEnergyCostText.text = ("Energy: ") + primaryData.EnergyCost.ToString();
            cardDamageDealtText.text = ("Damage: ") + primaryData.BaseDamage.ToString();

            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = ("Price: ") + primaryData.CurrencyCost.ToString();

            //Vector3 mousePosition = Input.mousePosition / mainCanvas.scaleFactor;

            //if (mousePosition.x + cardPopupObject.GetComponent<RectTransform>().rect.width > mainCanvas.GetComponent<RectTransform>().rect.width)
            //{
            //    mousePosition.x = mainCanvas.GetComponent<RectTransform>().rect.width - cardPopupObject.GetComponent<RectTransform>().rect.width;
            //}

            //if (mousePosition.y + cardPopupObject.GetComponent<RectTransform>().rect.height > mainCanvas.GetComponent<RectTransform>().rect.height)
            //{
            //    mousePosition.y = mainCanvas.GetComponent<RectTransform>().rect.height - cardPopupObject.GetComponent<RectTransform>().rect.height;
            //}

            //cardPopupObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
            cardPopupObject.SetActive(true);
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
            cardNameText.text = string.Empty;
            cardDescriptionText.text = string.Empty;
            cardEnergyCostText.text = string.Empty;
            cardDamageDealtText.text = string.Empty;

            if (cardCurrencyCostText != null)
                cardCurrencyCostText.text = string.Empty;

            cardPopupObject.SetActive(false);

            currentComponentNameText.gameObject.SetActive(false);
            currentComponentHealthText.gameObject.SetActive(false);
            currentComponentEnergyText.gameObject.SetActive(false);
            currentComponentEnergyGainText.gameObject.SetActive(false);
            currentComponentElementText.gameObject.SetActive(false);

            componentPopupObject.SetActive(false);
            currentComponentPopupObject.SetActive(false);
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
            currentComponentPopupObject.SetActive(true);
            currentTimer = 0f;
            popupQueued = false;
        }
    }
}
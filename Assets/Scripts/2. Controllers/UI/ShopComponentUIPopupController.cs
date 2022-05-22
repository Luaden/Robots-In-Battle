using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopComponentUIPopupController : BaseUIElement<SOItemDataObject>
{
    [Header("New Item")]
    [SerializeField] protected GameObject newItemPopupObject;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text energyGain;
    [SerializeField] private TMP_Text elementText;
    [SerializeField] private TMP_Text currencyCostText;
    [Space]
    [Header("Current Equipped Item")]
    [SerializeField] private GameObject currentItemPopupObject;
    [SerializeField] private TMP_Text currentItemNameText;
    [SerializeField] private TMP_Text currentItemHealthText;
    [SerializeField] private TMP_Text currentItemEnergyText;
    [SerializeField] private TMP_Text currentItemEnergyGainText;
    [SerializeField] private TMP_Text currentItemElementText;
    [Space]
    [Header("Popup Settings")]
    [SerializeField] private float currentItemPopupDelay;

    private float currentTimer;
    private bool popupQueued;

    public override void UpdateUI(SOItemDataObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

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

        nameText.text = primaryData.ItemName;
        healthText.text = ("Health: ") + primaryData.ComponentHP.ToString();
        energyText.text = ("Energy: ") + primaryData.ComponentEnergy.ToString();
        energyGain.text = ("Bonus Energy Gain: ") + primaryData.EnergyGainModifier.ToString();
        elementText.text = ("Element: ") + System.Enum.GetName(typeof(ElementType), primaryData.ComponentElement);
        currencyCostText.text = primaryData.CurrencyCost.ToString();

        currentItemNameText.gameObject.SetActive(true);
        currentItemNameText.text = currentItem.ComponentName;
        currentItemHealthText.gameObject.SetActive(true);
        currentItemHealthText.text = ("Health: ") + currentItem.ComponentMaxHP.ToString();
        currentItemEnergyText.gameObject.SetActive(true);
        currentItemEnergyText.text = ("Energy: ") + currentItem.ComponentMaxEnergy.ToString();
        currentItemEnergyGainText.gameObject.SetActive(true);
        currentItemEnergyGainText.text = ("Bonus Energy Gain: ") + currentItem.EnergyGainModifier.ToString();
        currentItemElementText.gameObject.SetActive(true);
        currentItemElementText.text = System.Enum.GetName(typeof(ElementType), currentItem.ComponentElement);

        popupQueued = true;

        newItemPopupObject.SetActive(true);
    }

    protected override bool ClearedIfEmpty(SOItemDataObject newData)
    {
        if (newData == null)
        {
            nameText.text = string.Empty;
            healthText.text = string.Empty;
            energyText.text = string.Empty;
            energyGain.text = string.Empty;
            elementText.text = string.Empty;

            currentItemNameText.gameObject.SetActive(false);
            currentItemHealthText.gameObject.SetActive(false);
            currentItemEnergyText.gameObject.SetActive(false);
            currentItemEnergyGainText.gameObject.SetActive(false);
            currentItemElementText.gameObject.SetActive(false);

            newItemPopupObject.SetActive(false);
            currentItemPopupObject.SetActive(false);
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
            currentItemPopupObject.SetActive(true);
            currentTimer = 0f;
            popupQueued = false;
        }
    }
}
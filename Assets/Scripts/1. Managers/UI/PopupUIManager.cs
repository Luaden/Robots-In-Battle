using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private float textRate;
    [SerializeField] private float generalHUDPopupDelay;

    private CombatCardUIPopupController cardUIPopupController;
    private ShopComponentUIPopupController componentUIPopupController;
    private ShopCardUIPopupController shopUIPopupController;
    private HUDGeneralUIPopupController hudGeneralUIPopupController;
    private HUDBuffUIPopupController hudBuffUIPopupController;
    private AIDialoguePopupController aIDialoguePopupController;
    private EventDialoguePopupController eventDialoguePopupController;

    private bool popupsEnabled = false;
    public float TextPace { get => textRate; }
    public float GeneralHUDPopupDelay { get => generalHUDPopupDelay; }

    private void Awake()
    {
        cardUIPopupController = GetComponentInChildren<CombatCardUIPopupController>();
        componentUIPopupController = GetComponentInChildren<ShopComponentUIPopupController>();
        hudGeneralUIPopupController = GetComponentInChildren<HUDGeneralUIPopupController>();
        shopUIPopupController = GetComponentInChildren<ShopCardUIPopupController>();
        aIDialoguePopupController = GetComponentInChildren<AIDialoguePopupController>();
        eventDialoguePopupController = GetComponentInChildren<EventDialoguePopupController>();
        hudBuffUIPopupController = GetComponentInChildren<HUDBuffUIPopupController>();
        

        if (CombatManager.instance != null)
        {
            CombatSequenceManager.OnCombatStart += ClearPopups;
            CombatSequenceManager.OnCombatStart += DisablePopups;

            CombatSequenceManager.OnCombatComplete += ClearPopups;
            CombatSequenceManager.OnCombatComplete += EnablePopups;

            AIDialogueController.OnDialogueStarted += DisablePopups;
            AIDialogueController.OnDialogueComplete += EnablePopups;
        }
    }

    private void OnDestroy()
    {
        if (CombatManager.instance != null)
        {
            CombatSequenceManager.OnCombatStart -= ClearPopups;
            CombatSequenceManager.OnCombatStart -= DisablePopups;

            CombatSequenceManager.OnCombatComplete -= ClearPopups;
            CombatSequenceManager.OnCombatComplete -= EnablePopups;

            AIDialogueController.OnDialogueStarted -= DisablePopups;
            AIDialogueController.OnDialogueComplete -= EnablePopups;
        }
    }

    public void HandlePopup(CardDataObject cardDataObject)
    {
        if(popupsEnabled)
            cardUIPopupController.UpdateUI(cardDataObject);
    }

    public void HandlePopup(SOItemDataObject sOItemDataObject)
    {
        if(popupsEnabled)
        {
            if (sOItemDataObject.ItemType == ItemType.Component)
                componentUIPopupController.UpdateUI(sOItemDataObject);
            else
                cardUIPopupController.UpdateUI(new CardDataObject(sOItemDataObject));
        }
    }
    public void HandlePopup(string name, string dialogue)
    {
        aIDialoguePopupController.UpdateUI(name, dialogue);
    }

    public void HandlePopup(SOEventObject eventDialogue)
    {
        eventDialoguePopupController.UpdateUI(eventDialogue);
    }

    public void HandlePopup(HUDGeneralElement elementType)
    {
        if(popupsEnabled)
        {
            hudGeneralUIPopupController.UpdateUI(elementType);
        }
    }

    public void HandlePopup(HUDBuffElement elementType)
    {
        if(popupsEnabled)
        {
            hudBuffUIPopupController.UpdateUI(elementType);
        }
    }

    public void ClearPopups()
    {
        if(cardUIPopupController != null)
            cardUIPopupController.UpdateUI(null);
        if (componentUIPopupController != null)
            componentUIPopupController.UpdateUI(null);
        if(hudGeneralUIPopupController != null)
            hudGeneralUIPopupController.UpdateUI(HUDGeneralElement.None);
        if (hudBuffUIPopupController != null)
            hudBuffUIPopupController.UpdateUI(HUDBuffElement.None);
        if (shopUIPopupController != null)
            shopUIPopupController.UpdateUI(null);
    }

    private void DisablePopups()
    {
        popupsEnabled = false;
    }

    private void EnablePopups()
    {
        popupsEnabled = true;
    }
}

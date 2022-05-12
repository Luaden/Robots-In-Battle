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
    private GeneralHUDUIPopupController generalHUDUIPopupController;
    private HUDPopupController hudPopUpController;
    private AIDialoguePopupController aIDialoguePopupController;
    private EventDialoguePopupController eventDialoguePopupController;

    private bool popupsEnabled = false;
    public float TextPace { get => textRate; }
    public float GeneralHUDPopupDelay { get => generalHUDPopupDelay; }

    private void Awake()
    {
        cardUIPopupController = GetComponentInChildren<CombatCardUIPopupController>();
        componentUIPopupController = GetComponentInChildren<ShopComponentUIPopupController>();
        generalHUDUIPopupController = GetComponentInChildren<GeneralHUDUIPopupController>();
        hudPopUpController = GetComponentInChildren<HUDPopupController>();
        shopUIPopupController = GetComponentInChildren<ShopCardUIPopupController>();
        aIDialoguePopupController = GetComponentInChildren<AIDialoguePopupController>();
        eventDialoguePopupController = GetComponentInChildren<EventDialoguePopupController>();
        
        Debug.Log("Preparing to assign events.");

        if (CombatManager.instance != null)
        {
            Debug.Log("Assigning events.");
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

    public void HandlePopup(GeneralHUDElement elementType)
    {
        if(popupsEnabled)
        {
            generalHUDUIPopupController.UpdateUI(elementType);
        }
    }

    public void ClearPopups()
    {
        if(cardUIPopupController != null)
            cardUIPopupController.UpdateUI(null);
        if (componentUIPopupController != null)
            componentUIPopupController.UpdateUI(null);
        if(generalHUDUIPopupController != null)
            generalHUDUIPopupController.UpdateUI(GeneralHUDElement.None);
        if (shopUIPopupController != null)
            shopUIPopupController.UpdateUI(null);
    }

    private void DisablePopups()
    {
        Debug.Log("Disabling popups.");
        popupsEnabled = false;
    }

    private void EnablePopups()
    {
        Debug.Log("Popups enabled.");
        popupsEnabled = true;
    }
}

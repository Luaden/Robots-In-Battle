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

        if (CombatManager.instance != null)
            CombatSequenceManager.OnCombatComplete += InactivatePopup;
    }

    private void OnDestroy()
    {
        if (CombatManager.instance != null)
            CombatSequenceManager.OnCombatComplete -= InactivatePopup;
    }

    public void HandlePopup(CardDataObject cardDataObject)
    {
        cardUIPopupController.UpdateUI(cardDataObject);
    }

    public void HandlePopup(SOItemDataObject sOItemDataObject)
    {
        if (sOItemDataObject.ItemType == ItemType.Component)
            componentUIPopupController.UpdateUI(sOItemDataObject);
        else
            cardUIPopupController.UpdateUI(new CardDataObject(sOItemDataObject));
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
        generalHUDUIPopupController.UpdateUI(elementType);
    }

    public void InactivatePopup()
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private float textRate;

    private CardUIPopupController cardUIPopupController;
    private ComponentUIPopupController componentUIPopupController;
    private ShopUIPopupController shopUIPopupController;
    private MechUIPopupController mechUIPopupController;
    private HUDPopupController hudPopUpController;
    private AIDialoguePopupController aIDialoguePopupController;
    private EventDialoguePopupController eventDialoguePopupController;

    public float TextPace { get => textRate; }

    private void Awake()
    {
        cardUIPopupController = GetComponentInChildren<CardUIPopupController>();
        componentUIPopupController = GetComponentInChildren<ComponentUIPopupController>();
        mechUIPopupController = GetComponentInChildren<MechUIPopupController>();
        hudPopUpController = GetComponentInChildren<HUDPopupController>();
        shopUIPopupController = GetComponentInChildren<ShopUIPopupController>();
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

    public void InactivatePopup()
    {
        if(cardUIPopupController != null)
            cardUIPopupController.UpdateUI(null);
        if (componentUIPopupController != null)
            componentUIPopupController.UpdateUI(null);
        if(mechUIPopupController != null)
            mechUIPopupController.UpdateUI(Channels.None);
        if (shopUIPopupController != null)
            shopUIPopupController.UpdateUI(null);

        //if (hudPopUpController != null)
            //hudPopUpController.UpdateUI(null);
    }
}

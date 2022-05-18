using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private float textRate;
    [SerializeField] private float generalHUDPopupDelay;

    private CombatCardUIPopupController combatCardUIPopupController;
    private ShopComponentUIPopupController shopComponentUIPopupController;
    private ShopCardUIPopupController shopCardUIPopupController;
    private HUDGeneralUIPopupController hudGeneralUIPopupController;
    private HUDBuffUIPopupController hudBuffUIPopupController;
    private HUDMechComponentPopupController hudMechComponentPopupController;
    private AIDialoguePopupController aIDialoguePopupController;
    private AIConversationPopupController aIConversationPopupController;
    private EventDialoguePopupController eventDialoguePopupController;
    private HUDFloatingDamagePopupController floatingDamagePopupController;

    [SerializeField] private bool popupsEnabled = true;
    public float TextPace { get => textRate; }
    public float GeneralHUDPopupDelay { get => generalHUDPopupDelay; }

    private void Awake()
    {
        combatCardUIPopupController = GetComponentInChildren<CombatCardUIPopupController>();
        shopComponentUIPopupController = GetComponentInChildren<ShopComponentUIPopupController>();
        hudGeneralUIPopupController = GetComponentInChildren<HUDGeneralUIPopupController>();
        shopCardUIPopupController = GetComponentInChildren<ShopCardUIPopupController>();
        aIDialoguePopupController = GetComponentInChildren<AIDialoguePopupController>();
        aIConversationPopupController = GetComponentInChildren<AIConversationPopupController>();
        eventDialoguePopupController = GetComponentInChildren<EventDialoguePopupController>();
        hudBuffUIPopupController = GetComponentInChildren<HUDBuffUIPopupController>();
        hudMechComponentPopupController = GetComponentInChildren<HUDMechComponentPopupController>();
        floatingDamagePopupController = GetComponentInChildren<HUDFloatingDamagePopupController>();

        if (CombatManager.instance != null)
        {
            CombatSequenceManager.OnCombatStart += ClearAllPopups;
            CombatSequenceManager.OnCombatStart += DisablePopups;

            CombatSequenceManager.OnCombatComplete += ClearAllPopups;
            CombatSequenceManager.OnCombatComplete += EnablePopups;

            AIDialogueController.OnDialogueStarted += DisablePopups;
            AIDialogueController.OnDialogueComplete += EnablePopups;
        }
    }

    private void OnDestroy()
    {
        if (CombatManager.instance != null)
        {
            CombatSequenceManager.OnCombatStart -= ClearAllPopups;
            CombatSequenceManager.OnCombatStart -= DisablePopups;

            CombatSequenceManager.OnCombatComplete -= ClearAllPopups;
            CombatSequenceManager.OnCombatComplete -= EnablePopups;

            AIDialogueController.OnDialogueStarted -= DisablePopups;
            AIDialogueController.OnDialogueComplete -= EnablePopups;
        }
    }

    public void HandlePopup(CardDataObject cardDataObject)
    {
        if(popupsEnabled)
            combatCardUIPopupController.UpdateUI(cardDataObject);
    }

    public void ClearCardUIPopup()
    {
        combatCardUIPopupController.UpdateUI(null);
    }

    public void HandlePopup(SOItemDataObject sOItemDataObject)
    {
        if(popupsEnabled)
        {
            if (sOItemDataObject.ItemType == ItemType.Component)
                shopComponentUIPopupController.UpdateUI(sOItemDataObject);
            else
                shopCardUIPopupController.UpdateUI(sOItemDataObject);
        }
    }
    public void HandlePopup(string name, string dialogue)
    {
        ClearAllPopups();
        aIDialoguePopupController.UpdateUI(name, dialogue);
    }

    public void HandlePopup(ConversationObject conversationObject)
    {
        ClearAllPopups();
        aIConversationPopupController.UpdateUI(conversationObject);
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

    public void HandlePopup(MechSelect character)
    {
        if(popupsEnabled)
        {
            hudMechComponentPopupController.UpdateUI(character);
        }
    }

    public void HandlePopup(DamageMechPairObject damageObject)
    {
        floatingDamagePopupController.UpdateUI(damageObject);
    }

    public void ClearAllPopups()
    {
        if(combatCardUIPopupController != null)
            combatCardUIPopupController.UpdateUI(null);
        if (shopComponentUIPopupController != null)
            shopComponentUIPopupController.UpdateUI(null);
        if(hudGeneralUIPopupController != null)
            hudGeneralUIPopupController.UpdateUI(HUDGeneralElement.None);
        if (hudBuffUIPopupController != null)
            hudBuffUIPopupController.UpdateUI(HUDBuffElement.None);
        if (hudMechComponentPopupController != null)
            hudMechComponentPopupController.UpdateUI(MechSelect.None);
        if (shopCardUIPopupController != null)
            shopCardUIPopupController.UpdateUI(null);
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

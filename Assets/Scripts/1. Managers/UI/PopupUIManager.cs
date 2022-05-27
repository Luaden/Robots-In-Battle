using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private float textRate;
    [SerializeField] private float generalHUDPopupDelay;
    [SerializeField] private AIDialoguePopupController aIDialoguePopupController;
    [SerializeField] private EventDialoguePopupController eventDialoguePopupController;
    [SerializeField] private VFXPopupController vFXPopupController;
    private bool popupsEnabled = true;

    private CombatCardUIPopupController combatCardUIPopupController;
    private WorkshopItemPopupController workshopItemPopupController;
    private HUDGeneralUIPopupController hudGeneralUIPopupController;
    private HUDBuffUIPopupController hudBuffUIPopupController;
    private HUDMechComponentPopupController hudMechComponentPopupController;
    private AIConversationPopupController aIConversationPopupController;
    private HUDFloatingDamagePopupController floatingDamagePopupController;

    public float TextPace { get => textRate; }
    public float GeneralHUDPopupDelay { get => generalHUDPopupDelay; }

    private void Awake()
    {
        combatCardUIPopupController = GetComponentInChildren<CombatCardUIPopupController>();
        workshopItemPopupController = GetComponentInChildren<WorkshopItemPopupController>();
        hudGeneralUIPopupController = GetComponentInChildren<HUDGeneralUIPopupController>();
        aIConversationPopupController = GetComponentInChildren<AIConversationPopupController>();
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
        if(popupsEnabled && !GameManager.instance.isTrailerMaking)
            combatCardUIPopupController.UpdateUI(cardDataObject);
    }

    public void ClearCardUIPopup()
    {
        combatCardUIPopupController.UpdateUI(null);
    }

    public void HandlePopup(SOItemDataObject sOItemDataObject)
    {
        if (popupsEnabled && DowntimeManager.instance != null)
            workshopItemPopupController.UpdateUI(sOItemDataObject);
        else if (popupsEnabled && CombatManager.instance != null)
            combatCardUIPopupController.UpdateUI(sOItemDataObject);

    }
    public void HandlePopup(string name, string dialogue, CharacterSelect character)
    {
        //ClearAllPopups();
        aIDialoguePopupController.UpdateUI(name, dialogue, character);
    }

    public void HandlePopup(ConversationObject conversationObject)
    {
        //ClearAllPopups();
        aIConversationPopupController.UpdateUI(conversationObject);
    }

    public void HandlePopup(SOEventObject eventDialogue)
    {
        eventDialoguePopupController.UpdateUI(eventDialogue);
    }

    public void HandlePopup(HUDGeneralElement elementType)
    {
        if(popupsEnabled && !GameManager.instance.isTrailerMaking)
        {
            hudGeneralUIPopupController.UpdateUI(elementType);
        }
    }

    public void HandlePopup(HUDBuffElement elementType)
    {
        if(popupsEnabled && !GameManager.instance.isTrailerMaking)
        {
            hudBuffUIPopupController.UpdateUI(elementType);
        }
    }

    public void HandlePopup(MechSelect character)
    {
        if(popupsEnabled && !GameManager.instance.isTrailerMaking)
        {
            hudMechComponentPopupController.UpdateUI(character);
        }
    }

    public void HandlePopup(DamageMechPairObject damageObject)
    {
        floatingDamagePopupController.UpdateUI(damageObject);
    }

    public void HandlePopup(Channels channel)
    {
        vFXPopupController.UpdateUI(channel);
    }

    public void ClearAllPopups()
    {
        if(combatCardUIPopupController != null)
            combatCardUIPopupController.UpdateUI(null);
        if (workshopItemPopupController != null)
            workshopItemPopupController.UpdateUI(null);
        if(hudGeneralUIPopupController != null)
            hudGeneralUIPopupController.UpdateUI(HUDGeneralElement.None);
        if (hudBuffUIPopupController != null)
            hudBuffUIPopupController.UpdateUI(HUDBuffElement.None);
        if (hudMechComponentPopupController != null)
            hudMechComponentPopupController.UpdateUI(MechSelect.None);
    }

    private void DisablePopups()
    {
        //popupsEnabled = false;
    }

    private void EnablePopups()
    {
        //popupsEnabled = true;
    }
}

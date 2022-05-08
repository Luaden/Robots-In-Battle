using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private float textPace;
    [SerializeField] private string debugName;
    [TextArea(1, 5)]
    [SerializeField] private string debugDialogue;


    private CardUIPopupController cardUIPopupController;
    private ShopUIPopupController shopUIPopupController;
    private MechUIPopupController mechUIPopupController;
    private HUDPopupController hudPopUpController;
    private AIDialoguePopupController aIDialoguePopupController;

    public float TextPace { get => textPace; }

    private void Awake()
    {
        cardUIPopupController = GetComponentInChildren<CardUIPopupController>();
        mechUIPopupController = GetComponentInChildren<MechUIPopupController>();
        hudPopUpController = GetComponentInChildren<HUDPopupController>();
        shopUIPopupController = GetComponentInChildren<ShopUIPopupController>();
        aIDialoguePopupController = GetComponentInChildren<AIDialoguePopupController>();

        if (CombatManager.instance != null)
            CombatSequenceManager.OnCombatComplete += InactivatePopup;

        if (aIDialoguePopupController == null)
        {
            Debug.Log("Oopsie.");
        }
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

    public void HandlePopup(SOItemDataObject soItemDataObject)
    {
        //Handle generic popup.
    }

    public void HandlePopup(ShopItemUIController shopItem)
    {
        shopUIPopupController.UpdateUI(shopItem);
    }

    public void HandlePopup(string name, string dialogue)
    {
        aIDialoguePopupController.UpdateUI(name, dialogue);
    }

    public void InactivatePopup()
    {
        if(cardUIPopupController != null)
            cardUIPopupController.UpdateUI(null);
        if(mechUIPopupController != null)
            mechUIPopupController.UpdateUI(Channels.None);
        if (shopUIPopupController != null)
            shopUIPopupController.UpdateUI(null);

        //if (hudPopUpController != null)
            //hudPopUpController.UpdateUI(null);
    }
}

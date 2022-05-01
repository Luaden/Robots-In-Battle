using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    private CardUIPopupController cardUIPopupController;
    private ShopUIPopupController shopUIPopupController;
    private MechUIPopupController mechUIPopupController;
    private HUDPopupController hudPopUpController;


    private void Awake()
    {
        cardUIPopupController = GetComponentInChildren<CardUIPopupController>();
        mechUIPopupController = GetComponentInChildren<MechUIPopupController>();
        hudPopUpController = GetComponentInChildren<HUDPopupController>();
        shopUIPopupController = GetComponentInChildren<ShopUIPopupController>();
    }

    public void HandlePopup(CardDataObject cardDataObject)
    {
        cardUIPopupController.UpdateUI(cardDataObject);
    }

    //public void HandlePopup(List<SOCardEffectObject> cardEffects, Transform transform, Vector3 cursorPosition)
    //{
    //    hudPopUpController.HandlePopup(cardEffects, transform, cursorPosition);
    //}

    //public void HandlePopup(MechObject mechObject, Transform transform, Vector3 cursorPosition)
    //{
    //    mechUIPopupController.HandlePopup(mechObject, transform, cursorPosition);
    //}

    public void HandlePopup(ShopItemUIObject shopItem)
    {
        shopUIPopupController.UpdateUI(shopItem);
    }

    public void InactivatePopup()
    {
        if(cardUIPopupController != null)
            cardUIPopupController.UpdateUI(null);
        if(shopUIPopupController != null)
            shopUIPopupController.UpdateUI(null);
    }

}

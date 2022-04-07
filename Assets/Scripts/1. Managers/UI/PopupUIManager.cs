using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    private CardUIPopupController cardUIPopupController;
    private MechUIPopupController mechUIPopupController;
    private HUDPopupController hudPopUpController;

    // gets called from CardUIController
    public void HandlePopup(CardDataObject cardDataObject,
                            Transform transform)
    {
        //routes to CardUIPopupController
        cardUIPopupController.HandlePopup(cardDataObject, transform);
    }
    public void HandlePopup(List<CardEffectObject> cardEffects,
                            Transform transform)
    {
        //routes to HUDPopupController
        hudPopUpController.HandlePopup(cardEffects, transform);
    }
    public void HandlePopup(MechObject mechObject,
                            Transform transform)
    {
        //routes to MechUIPopupController
        mechUIPopupController.HandlePopup(mechObject, transform);
    }

}

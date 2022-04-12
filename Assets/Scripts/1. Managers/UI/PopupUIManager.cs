using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    static private PopupUIManager instance;
    public static PopupUIManager Instance { get { return instance; } }

    private CardUIPopupController cardUIPopupController;
    private MechUIPopupController mechUIPopupController;
    private HUDPopupController hudPopUpController;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        cardUIPopupController = GetComponentInChildren<CardUIPopupController>();
        mechUIPopupController = GetComponentInChildren<MechUIPopupController>();
        hudPopUpController = GetComponentInChildren<HUDPopupController>();
    }

    public void HandlePopup(CardDataObject cardDataObject,
                            Transform transform,
                            Vector3 cursorPosition)
    {
        cardUIPopupController.HandlePopup(cardDataObject,
                                          transform,
                                          cursorPosition);
    }
    public void HandlePopup(List<SOCardEffectObject> cardEffects,
                            Transform transform,
                            Vector3 cursorPosition)
    {
        hudPopUpController.HandlePopup(cardEffects,
                                       transform,
                                       cursorPosition);
    }
    public void HandlePopup(MechObject mechObject,
                            Transform transform,
                            Vector3 cursorPosition)
    {
        mechUIPopupController.HandlePopup(mechObject,
                                          transform,
                                          cursorPosition);
    }

    public void InactivatePopup()
    {
        cardUIPopupController.InactivatePopup();
    }

}

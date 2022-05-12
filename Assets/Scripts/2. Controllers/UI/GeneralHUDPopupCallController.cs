using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralHUDPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool canBeBlockedBySlotFill;

    [SerializeField] private HUDGeneralElement elementType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canBeBlockedBySlotFill)
        {
            if (GetComponent<BaseSlotController<CardUIController>>().CurrentSlottedItem != null)
                return;
 
            CombatManager.instance.PopupUIManager.HandlePopup(elementType);
            return;
        }

        CombatManager.instance.PopupUIManager.HandlePopup(elementType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(HUDGeneralElement.None);
    }
}

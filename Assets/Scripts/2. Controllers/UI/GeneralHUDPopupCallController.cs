using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralHUDPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool canBeBlockedBySlotFill;
    [SerializeField] private HUDGeneralElement elementType;

    private bool canPopup = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canPopup)
            return;

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
        if (!canPopup)
            return;

        CombatManager.instance.PopupUIManager.HandlePopup(HUDGeneralElement.None);
    }

    private void Start()
    {
        CardUIController.OnPickUp += IgnorePopups;
    }

    private void OnDestroy()
    {
        CardUIController.OnPickUp -= IgnorePopups;
    }

    private void IgnorePopups(Channels channel)
    {
        Debug.Log(channel);

        if (channel == Channels.None)
            canPopup = true;
        else
            canPopup = false;
    }
}

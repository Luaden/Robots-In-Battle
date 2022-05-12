using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralHUDPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private HUDGeneralElement elementType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(elementType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(HUDGeneralElement.None);
    }
}

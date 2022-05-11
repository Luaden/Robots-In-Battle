using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralHUDPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GeneralHUDElement elementType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(elementType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(GeneralHUDElement.None);
    }
}

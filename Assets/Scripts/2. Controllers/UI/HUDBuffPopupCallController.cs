using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDBuffPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private HUDBuffElement elementType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(elementType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(HUDBuffElement.None);
    }
}

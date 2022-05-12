using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDMechPopupCallController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MechSelect mechType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(mechType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.instance.PopupUIManager.HandlePopup(MechSelect.None);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickController : MonoBehaviour
{
    private CardUIController clickedCard;
    public void HandleClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerClick != null)
        {
            if (pointerEventData.pointerClick.GetComponent<CardUIController>() == null)
                return;
        }

        clickedCard = pointerEventData.pointerClick.GetComponent<CardUIController>();
    }

    public CardUIController GetCardFromClick()
    {
        return clickedCard;
    }

    public void ClearClickedCard()
    {
        clickedCard = null;
    }
}

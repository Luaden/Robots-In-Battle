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

            if (pointerEventData.pointerClick.GetComponent<CardUIController>() == clickedCard)
            {
                ClearClickedCard();
                return;
            }
        }

        if(clickedCard != null)
        {
            clickedCard.DeselectCard();
        }

        clickedCard = pointerEventData.pointerClick.GetComponent<CardUIController>();
        clickedCard.SelectCard();
    }

    public CardUIController GetCardFromClick()
    {
        return clickedCard;
    }

    public void ClearClickedCard()
    {
        if (clickedCard == null)
            return;

        clickedCard.DeselectCard();
        clickedCard = null;
    }
}

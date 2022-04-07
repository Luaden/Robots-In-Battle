using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIPopupController : MonoBehaviour
{
    // ref in inspector?
    [SerializeField] protected GameObject popupObject;
    RectTransform rectTransform;

    private int offsetX;
    private int offsetY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        popupObject.SetActive(false);
    }

    // create a popup anchored to transform location to display card details
    public void HandlePopup(CardDataObject cardDataObject,
                            Transform transform)
    {
        //create popupdata
        PopupData popupData = new PopupData(cardDataObject.CardName,
                                            cardDataObject.CardDescription);

        CardPopupObject cardPopupObject = popupObject.GetComponent<CardPopupObject>();
        //assign popupdata to cardpopup
        cardPopupObject.Assign(popupData);

        // put object at right location
        rectTransform.anchoredPosition = transform.position;
        // anchored to transform

        // display
        popupObject.SetActive(true);


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIPopupController : MonoBehaviour
{
    [SerializeField] protected GameObject popupObject;
    RectTransform rectTransform;

    private int cardWidth = 125;
    private int cardHeight = 160;
    private int offsetX = 25;
    private int offsetY = 25;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        popupObject.SetActive(false);
    }

    // create a popup anchored to transform location to display card details
    public void HandlePopup(CardDataObject cardDataObject, 
                            Transform transform,
                            Vector3 cursorPosition)
    {
        //create popupdata
        PopupData popupData = new PopupData(cardDataObject.CardName,
                                            cardDataObject.CardDescription);

        CardPopupObject cardPopupObject = popupObject.GetComponent<CardPopupObject>();
        //assign popupdata to cardpopup
        cardPopupObject.Assign(popupData);

        // put object at right location
        rectTransform = popupObject.GetComponent<RectTransform>();
        rectTransform.position = new Vector3(transform.position.x + cardWidth, cursorPosition.y);
        // display
        popupObject.SetActive(true);


    }

    public void ClearPopup()
    {
        popupObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIPopupController : MonoBehaviour
{
    [SerializeField] protected GameObject popupObject;

    private RectTransform rectTransform;
    private int cardWidth = 125;

    private void Awake()
    {
        popupObject.SetActive(false);
    }
    public void HandlePopup(CardDataObject cardDataObject, 
                            Transform transform,
                            Vector3 cursorPosition)
    {
        //create popupdata
        PopupData popupData = new PopupData(cardDataObject.CardName,
                                            cardDataObject.CardDescription);

        // get component from gameobject
        CardPopupObject cardPopupObject = popupObject.GetComponent<CardPopupObject>();

        //assign popupdata to cardpopup
        cardPopupObject.Assign(popupData);

        // reference the transform of the popup
        rectTransform = popupObject.GetComponent<RectTransform>();

        // put object at correct location
        rectTransform.position = new Vector3(transform.position.x + cardWidth, cursorPosition.y);

        // display
        popupObject.SetActive(true);


    }

    public void InactivatePopup()
    {
        popupObject.SetActive(false);
    }

}

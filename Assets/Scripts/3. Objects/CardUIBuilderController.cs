using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuilderController : MonoBehaviour
{
    [SerializeField] private GameObject CardUIPrefab;
    //Builds Card UIs, sets destination.

    public void BuildAndDrawCard(CardDataObject cardToDraw, Transform cardStartPoint, Transform cardHomePoint)
    {
        GameObject CardUIObject;
        CardUIObject = Instantiate(CardUIPrefab, cardHomePoint);
        CardUIObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = CardUIObject;
        CardUIObject.SetActive(true);
    }
}

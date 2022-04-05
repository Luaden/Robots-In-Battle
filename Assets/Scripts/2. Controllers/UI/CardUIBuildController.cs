using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCardPrefab;
    [SerializeField] private GameObject OpponentCardPrefab;
    //Builds Card UIs, sets destination.

    public void BuildAndDrawCard(CardDataObject cardToDraw, Transform cardStartPoint, Transform cardHomePoint)
    {
        GameObject CardUIObject;
        CardUIObject = Instantiate(PlayerCardPrefab, cardHomePoint);
        CardUIObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = CardUIObject;
        CardUIObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCardPrefab;
    [SerializeField] private GameObject OpponentCardPrefab;
    //Builds Card UIs, sets destination.

    public CardUIController BuildPlayerCard(CardDataObject cardToDraw, Transform cardStartPoint)
    {
        GameObject cardUIGameObject;
        cardUIGameObject = Instantiate(PlayerCardPrefab, transform);
        cardUIGameObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = cardUIGameObject;
        CardUIController cardUIObject = cardUIGameObject.GetComponent<CardUIController>();

        cardUIObject.InitCardUI(cardToDraw, CharacterSelect.Player);
        cardUIGameObject.SetActive(true);

        return cardUIObject;
    }

    public CardUIController BuildOpponentCard(CardDataObject cardToDraw, Transform cardStartPoint)
    {
        GameObject cardUIGameObject;
        cardUIGameObject = Instantiate(PlayerCardPrefab, transform);
        cardUIGameObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = cardUIGameObject;
        CardUIController cardUIObject = cardUIGameObject.GetComponent<CardUIController>();

        cardUIObject.InitCardUI(cardToDraw, CharacterSelect.Opponent);
        cardUIGameObject.SetActive(true);

        return cardUIObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    public CardUIController BuildPlayerCard(CardDataObject cardToDraw, Transform cardStartPoint)
    {
        GameObject cardUIGameObject;
        cardUIGameObject = Instantiate(cardPrefab, transform);
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
        cardUIGameObject = Instantiate(cardPrefab, transform);
        cardUIGameObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = cardUIGameObject;
        CardUIController cardUIObject = cardUIGameObject.GetComponent<CardUIController>();

        cardUIObject.InitCardUI(cardToDraw, CharacterSelect.Opponent);
        cardUIGameObject.SetActive(true);

        return cardUIObject;
    }
}

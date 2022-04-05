using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCardPrefab;
    [SerializeField] private GameObject OpponentCardPrefab;
    //Builds Card UIs, sets destination.

    public void BuildAndDrawPlayerCard(CardDataObject cardToDraw, Transform cardStartPoint)
    {
        GameObject cardUIGameObject;
        cardUIGameObject = Instantiate(PlayerCardPrefab);
        cardUIGameObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = cardUIGameObject;
        CardUIController cardUIObject = cardUIGameObject.GetComponent<CardUIController>();

        cardUIObject.InitCardUI(cardToDraw);

        CombatManager.instance.PlayerHandSlotManager.AddItemToCollection(cardUIObject);

        cardUIGameObject.SetActive(true);
    }

    public void BuildAndDrawOpponentCard(CardDataObject cardToDraw, Transform cardStartPoint)
    {
        GameObject cardUIGameObject;
        cardUIGameObject = Instantiate(PlayerCardPrefab);
        cardUIGameObject.transform.position = cardStartPoint.position;

        cardToDraw.CardUIObject = cardUIGameObject;
        CardUIController cardUIObject = cardUIGameObject.GetComponent<CardUIController>();

        cardUIObject.InitCardUI(cardToDraw);

        CombatManager.instance.OpponentHandSlotManager.AddItemToCollection(cardUIObject);

        cardUIGameObject.SetActive(true);
    }
}

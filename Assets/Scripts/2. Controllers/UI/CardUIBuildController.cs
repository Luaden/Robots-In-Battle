using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIBuildController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject itemPrefab;

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

    public ShopItemUIController BuildAndDisplayItemUI(SOItemDataObject shopItem, Transform startPoint,
    MechComponentDataObject oldMechComponentData = null)
    {
        GameObject shopItemUIGameObject;
        shopItemUIGameObject = Instantiate(itemPrefab, this.transform);
        shopItemUIGameObject.transform.position = startPoint.position;

        ShopItemUIController shopItemUIController = shopItemUIGameObject.GetComponent<ShopItemUIController>();
        shopItemUIController.InitUI(shopItem, oldMechComponentData);

        shopItemUIGameObject.SetActive(true);
        return shopItemUIController;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerDeckTransform;
    [SerializeField] private Transform opponentDeckTransform;
    [SerializeField] private PlayerHandUISlotManager playerHandSlotManager;
    [SerializeField] private Transform opponentHandTransform;
    [SerializeField] private Transform playerInventoryDeckTransform;
    [SerializeField] private Transform opponentInventoryDeckTransform;

    private CardUIBuildController cardUIBuildController;

    public void BuildAndDrawPlayerCard(CardDataObject cardToDraw)
    {
        CardUIController drawnCard = cardUIBuildController.BuildPlayerCard(cardToDraw, playerDeckTransform);
        CombatManager.instance.PlayerHandSlotManager.AddItemToCollection(drawnCard, null);
    }

    public void BuildAndDrawOpponentCard(CardDataObject cardToDraw)
    {
        CardUIController drawnCard = cardUIBuildController.BuildOpponentCard(cardToDraw, opponentDeckTransform);
        CombatManager.instance.OpponentHandSlotManager.AddItemToCollection(drawnCard, null);
    }

    public void BuildPlayerInventoryCard(SOItemDataObject sOItemDataObject)
    {
        ShopItemUIController drawnCard = cardUIBuildController.BuildAndDisplayItemUI(sOItemDataObject, playerInventoryDeckTransform);
        CombatManager.instance.PlayerInventoryCardDeckSlotManager.AddItemToCollection(drawnCard, null);
    }
    public void BuildOpponentInventoryCard(SOItemDataObject sOItemDataObject)
    {
        ShopItemUIController drawnCard = cardUIBuildController.BuildAndDisplayItemUI(sOItemDataObject, opponentInventoryDeckTransform);
        CombatManager.instance.OpponentInventoryCardDeckSlotManager.AddItemToCollection(drawnCard, null);
    }


    public void DestroyCardUI(CardDataObject cardToReturn)
    {
        Destroy(cardToReturn.CardUIObject);
    }

    private void Awake()
    {
        cardUIBuildController = FindObjectOfType<CardUIBuildController>(true);
    }
}

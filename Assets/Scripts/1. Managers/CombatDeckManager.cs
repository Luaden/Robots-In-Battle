using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDeckManager : MonoBehaviour
{
    private CombatDeckController playerDeck;
    private CombatDeckController opponentDeck;

    public CombatDeckManager()
    {
        playerDeck = new CombatDeckController();
        opponentDeck = new CombatDeckController();
    }

    public void SetPlayerDeck(List<CardDataObject> playerCardSOs)
    {
        playerDeck.InitDeckList(playerCardSOs);
        RandomizeCardDeck(playerDeck);
    }

    public void SetOpponentDeck(List<CardDataObject> opponentCardSOs)
    {
        opponentDeck.InitDeckList(opponentCardSOs);
        RandomizeCardDeck(opponentDeck);
    }

    public void DrawPlayerCard(int amountToDraw = 1)
    {
        for(int i = 0; i < amountToDraw; i++)
        {
            CardDataObject drawnCard = playerDeck.DrawCard();

            if (drawnCard == null)
            {
                Debug.Log("No more cards to draw in deck.");
                return;
            }

            CombatManager.instance.CardUIManager.BuildAndDrawPlayerCard(drawnCard);
        }
    }

    public void DrawOpponentCard(int amountToDraw = 1)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            CardDataObject drawnCard = opponentDeck.DrawCard();

            if (drawnCard == null)
            {
                Debug.Log("No more cards to draw in player deck.");
                return;
            }

            CombatManager.instance.CardUIManager.BuildAndDrawOpponentCard(drawnCard);
        }
    }

    public void ReturnCardToPlayerDeck(CardDataObject cardToReturn)
    {
        if(cardToReturn.CardUIController.CardSlotController != null)
        {
            BaseSlotController<CardUIController> slotController = cardToReturn.CardUIController.CardSlotController;
            slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIController);
        }

        playerDeck.AddCardToBottom(cardToReturn);
        cardToReturn.CardUIController.DissolveCardUI();
    }

    public void ReturnCardToOpponentDeck(CardDataObject cardToReturn)
    {
        if (cardToReturn.CardUIController.CardSlotController != null)
        {
            BaseSlotController<CardUIController> slotController = cardToReturn.CardUIController.CardSlotController;
            slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIController);
        }

        opponentDeck.AddCardToBottom(cardToReturn);
        cardToReturn.CardUIController.DissolveCardUI();
    }

    private void RandomizeCardDeck(CombatDeckController destinationDeck)
    {
        List<CardDataObject> newDeckOrder = new List<CardDataObject>();
        int deckCount = destinationDeck.CardDeck.Count;


        for (int i = 0; i < deckCount; i++)
        {
            int randomInt = Random.Range(0, destinationDeck.CardDeck.Count);

            newDeckOrder.Add(destinationDeck.CardDeck[randomInt]);
            destinationDeck.RemoveCard(destinationDeck.CardDeck[randomInt]);
        }

        destinationDeck.CardDeck = newDeckOrder;
    }
}

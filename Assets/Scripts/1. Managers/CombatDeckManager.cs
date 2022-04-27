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

    public void SetPlayerDeck(List<SOItemDataObject> playerCardSOs)
    {
        playerDeck.InitDeckList(playerCardSOs);
        RandomizeCardDeck(playerDeck);
    }

    public void SetOpponentDeck(List<SOItemDataObject> opponentCardSOs)
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
        BaseSlotController<CardUIController> slotController = cardToReturn.CardUIObject.GetComponent<CardUIController>().CardSlotController;
        slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIObject.GetComponent<CardUIController>());

        CombatManager.instance.CardUIManager.DestroyCardUI(cardToReturn);

        cardToReturn.SelectedChannels = Channels.None;

        playerDeck.AddCardToBottom(cardToReturn);
    }

    public void ReturnCardToOpponentDeck(CardDataObject cardToReturn)
    {
        BaseSlotController<CardUIController> slotController = cardToReturn.CardUIObject.GetComponent<CardUIController>().CardSlotController;
        slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIObject.GetComponent<CardUIController>());
        
        CombatManager.instance.CardUIManager.DestroyCardUI(cardToReturn);

        cardToReturn.SelectedChannels = Channels.None;
        
        opponentDeck.AddCardToBottom(cardToReturn);
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

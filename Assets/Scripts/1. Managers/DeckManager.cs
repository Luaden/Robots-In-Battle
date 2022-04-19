using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private DeckController playerDeck;
    private DeckController opponentDeck;

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
                Debug.Log("No more cards to draw in player deck.");
                return;
            }

            CombatManager.instance.HandManager.AddCardToPlayerHand(drawnCard);
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

            CombatManager.instance.HandManager.AddCardToOpponentHand(drawnCard);
            CombatManager.instance.CardUIManager.BuildAndDrawOpponentCard(drawnCard);
        }
    }

    public void ReturnCardToPlayerDeck(CardDataObject cardToReturn)
    {
        BaseSlotController<CardUIController> slotController = cardToReturn.CardUIObject.GetComponent<CardUIController>().CardSlotController;

        cardToReturn.SelectedChannels = Channels.None;
        slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIObject.GetComponent<CardUIController>());

        playerDeck.AddCardToBottom(cardToReturn);
        CombatManager.instance.CardUIManager.DestroyCardUI(cardToReturn);
    }

    public void ReturnCardToOpponentDeck(CardDataObject cardToReturn)
    {
        BaseSlotController<CardUIController> slotController = cardToReturn.CardUIObject.GetComponent<CardUIController>().CardSlotController;
        
        cardToReturn.SelectedChannels = Channels.None;
        slotController.SlotManager.RemoveItemFromCollection(cardToReturn.CardUIObject.GetComponent<CardUIController>());
        
        opponentDeck.AddCardToBottom(cardToReturn);
        CombatManager.instance.CardUIManager.DestroyCardUI(cardToReturn);
    }

    private void Awake()
    {
        playerDeck = new DeckController();
        opponentDeck = new DeckController();
    }

    private void RandomizeCardDeck(DeckController destinationDeck)
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

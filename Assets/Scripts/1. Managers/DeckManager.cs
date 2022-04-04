using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Transform playerDeckTransform;
    [SerializeField] private Transform opponentDeckTransform;
    [SerializeField] private Transform playerHandTransform;
    [SerializeField] private Transform opponentHandTransform;

    private DeckObject playerDeck;
    private DeckObject opponentDeck;

    private CardUIBuilderController cardUIBuilderController;


    public void SetPlayerHand(List<SOCardDataObject> playerCardSOs)
    {
        playerDeck.InitDeckList(playerCardSOs);
        RandomizeCardDeck(playerDeck);
    }

    public void SetOpponentHand(List<SOCardDataObject> opponentCardSOs)
    {
        opponentDeck.InitDeckList(opponentCardSOs);
        RandomizeCardDeck(opponentDeck);
    }

    public void DrawPlayerCard()
    {
        CardDataObject drawnCard;

        if (playerDeck.CardDeck.Count <= 0)
        {
            Debug.Log("Not enough utility cards to draw more!");
            return;
        }

        drawnCard = playerDeck.CardDeck[0];

        CombatManager.instance.CardPlayManager.AddCardToPlayerHand(drawnCard);
        playerDeck.CardDeck.Remove(playerDeck.CardDeck[0]);

        cardUIBuilderController.BuildAndDrawCard(drawnCard, playerDeckTransform, playerHandTransform);
    }

    public void DrawOpponentCard()
    {
        CardDataObject drawnCard;

        if (opponentDeck.CardDeck.Count <= 0)
        {
            Debug.Log("Not enough utility cards to draw more!");
            return;
        }

        drawnCard = opponentDeck.CardDeck[0];

        CombatManager.instance.CardPlayManager.AddCardToOpponentHand(drawnCard);
        opponentDeck.CardDeck.Remove(opponentDeck.CardDeck[0]);

        cardUIBuilderController.BuildAndDrawCard(drawnCard, opponentDeckTransform, opponentHandTransform);
    }

    public void ReturnCardToPlayerDeck(CardDataObject cardToReturn)
    {
        playerDeck.AddCardToBottom(cardToReturn);
        Destroy(cardToReturn.CardUIObject);
    }

    public void ReturnCardToOpponentDeck(CardDataObject cardToReturn)
    {
        opponentDeck.AddCardToBottom(cardToReturn);
        Destroy(cardToReturn.CardUIObject);
    }

    private void Awake()
    {
        playerDeck = new DeckObject();
        opponentDeck = new DeckObject();

        cardUIBuilderController = FindObjectOfType<CardUIBuilderController>();
    }

    private void RandomizeCardDeck(DeckObject destinationDeck)
    {
        List<CardDataObject> newDeckOrder = new List<CardDataObject>();
        int deckCount = newDeckOrder.Count;


        for (int i = 0; i < deckCount; i++)
        {
            int randomInt = Random.Range(0, destinationDeck.CardDeck.Count);

            newDeckOrder.Add(destinationDeck.CardDeck[randomInt]);
            destinationDeck.RemoveCard(destinationDeck.CardDeck[randomInt]);
        }

        destinationDeck.CardDeck = newDeckOrder;
    }
}

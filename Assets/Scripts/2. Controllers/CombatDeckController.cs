using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatDeckController
{
    [SerializeField] private List<CardDataObject> cardDeck;

    public List<CardDataObject> CardDeck { get => cardDeck; set => cardDeck = value; }

    public void InitDeckList(List<SOItemDataObject> newDeckSO)
    {
        cardDeck = new List<CardDataObject>();

        CardDataObject newCard;

        foreach (SOItemDataObject newCardSO in newDeckSO)
        {
            if(newCardSO.ItemType != ItemType.Card)
            {
                Debug.Log(newCardSO.ItemName + " was found in the deck, but it is not a Card ItemType. Was this a mistake or is this item classified incorrectly?");
                continue;
            }

            newCard = new CardDataObject(newCardSO);
            AddCardToBottom(newCard);
        }
    }

    public void AddCardToBottom(CardDataObject card) => cardDeck.Add(card);
    public void AddCardToTop(CardDataObject card) => cardDeck.Insert(0, card);

    public void RemoveCard(CardDataObject card)
    {
        if (!cardDeck.Contains(card))
            return;

        cardDeck.Remove(card);
    }

    public CardDataObject DrawCard()
    {
        CardDataObject drawnCard;

        if (cardDeck.Count <= 0)
        {
            Debug.Log("Not enough utility cards to draw more!");
            return null;
        }

        drawnCard = cardDeck[0];
        RemoveCard(drawnCard);

        return drawnCard;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckController
{
    [SerializeField] private List<CardDataObject> cardDeck;

    public List<CardDataObject> CardDeck { get => cardDeck; set => cardDeck = value; }

    public void RemoveCard(CardDataObject card)
    {
        if (!cardDeck.Contains(card))
            return;

        cardDeck.Remove(card);
    }

    public void AddCardToBottom(CardDataObject card) => cardDeck.Add(card);
    public void AddCardToTop(CardDataObject card) => cardDeck.Insert(0, card);

    public void InitDeckList(List<SOCardDataObject> newDeckSO)
    {
        cardDeck = new List<CardDataObject>();

        CardDataObject newCard;

        foreach (SOCardDataObject newCardSO in newDeckSO)
        {
            newCard = new CardDataObject(newCardSO);
            AddCardToBottom(newCard);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatDeckController
{
    [SerializeField] private List<CardDataObject> cardDeck;

    public List<CardDataObject> CardDeck { get => cardDeck; set => cardDeck = value; }

    public CombatDeckController() => cardDeck = new List<CardDataObject>();

    public void InitDeckList(List<CardDataObject> newDeck)
    {
        cardDeck = new List<CardDataObject>(newDeck);
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
            Debug.Log("Not enough cards to draw more!");
            return null;
        }

        drawnCard = cardDeck[0];
        RemoveCard(drawnCard);

        return drawnCard;
    }
}

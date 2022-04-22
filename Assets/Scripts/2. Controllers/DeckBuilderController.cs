using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderController
{
    public List<CardDataObject> BuildDeck(List<SOItemDataObject> newSODeck)
    {
        List<CardDataObject> newDeck = new List<CardDataObject>();

        foreach(SOItemDataObject soItem in newSODeck)
        {
            if(soItem.ItemType != ItemType.Card)
            {
                Debug.Log("The deckbuilder was sent a mech component. Like, why dude?");
                continue;
            }

            CardDataObject newCard = new CardDataObject(soItem);
            newDeck.Add(newCard);
        }

        return newDeck;
    }
}

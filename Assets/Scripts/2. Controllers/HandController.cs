using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController
{
    [SerializeField] private List<CardDataObject> characterHandHand;

    public List<CardDataObject> PlayerHand { get => characterHandHand; }

    public void AddCardToHand(CardDataObject cardToAdd)
    {
        characterHandHand.Add(cardToAdd);
    }


    public void RemoveCardFromHand(CardDataObject cardToRemove)
    {
        if (!characterHandHand.Contains(cardToRemove))
        {
            Debug.Log(cardToRemove.CardName + " was not found in the player hand!");
            return;
        }

        characterHandHand.Remove(cardToRemove);
    }
}

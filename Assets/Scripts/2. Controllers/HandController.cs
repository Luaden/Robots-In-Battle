using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController
{
    [SerializeField] private List<CardDataObject> characterHand;

    public List<CardDataObject> CharacterHand { get => characterHand; }

    public HandController() => characterHand = new List<CardDataObject>();

    public void AddCardToHand(CardDataObject cardToAdd)
    {
        if (characterHand == null)
            characterHand = new List<CardDataObject>();

        characterHand.Add(cardToAdd);
    }


    public void RemoveCardFromHand(CardDataObject cardToRemove)
    {
        if (!characterHand.Contains(cardToRemove))
        {
            Debug.Log(cardToRemove.CardName + " was not found in the hand!");
            return;
        }

        characterHand.Remove(cardToRemove);
    }

    private void Awake()
    {
        characterHand = new List<CardDataObject>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    [SerializeField] private List<CardDataObject> playerHand;
    [SerializeField] private List<CardDataObject> opponentHand;

    public List<CardDataObject> PlayerHand { get => playerHand; }
    public List<CardDataObject> OpponentHand { get => opponentHand;}

    public void AddCardToPlayerHand(CardDataObject cardToAdd)
    {
        playerHand.Add(cardToAdd);
    }

    public void AddCardToOpponentHand(CardDataObject cardToAdd)
    {
        opponentHand.Add(cardToAdd);
    }

    public void RemoveCardFromPlayerHand(CardDataObject cardToRemove)
    {
        if(!playerHand.Contains(cardToRemove))
        {
            Debug.Log(cardToRemove.CardName + " was not found in the player hand!");
            return;
        }

        playerHand.Remove(cardToRemove);
    }

    public void RemoveCardFromOpponentHand(CardDataObject cardToRemove)
    {
        if (!opponentHand.Contains(cardToRemove))
        {
            Debug.Log(cardToRemove.CardName + " was not found in the opponent hand!");
            return;
        }

        opponentHand.Remove(cardToRemove);
    }
}

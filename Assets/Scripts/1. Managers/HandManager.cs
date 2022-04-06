using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private HandController playerHand;
    [SerializeField] private HandController opponentHand;

    public int PlayerHandSize { get => playerHand.CharacterHand.Count; }
    public int OpponentHandSize { get => opponentHand.CharacterHand.Count; }

    public void AddCardToPlayerHand(CardDataObject cardToAdd)
    {
        playerHand.AddCardToHand(cardToAdd);
    }

    public void AddCardToOpponentHand(CardDataObject cardToAdd)
    {
        opponentHand.AddCardToHand(cardToAdd);
    }

    public void RemoveCardFromPlayerHand(CardDataObject cardToRemove)
    {
        playerHand.RemoveCardFromHand(cardToRemove);
    }

    public void RemoveCardFromOpponentHand(CardDataObject cardToRemove)
    {
        opponentHand.RemoveCardFromHand(cardToRemove);
    }

    private void Start()
    {
        playerHand = new HandController();
        opponentHand = new HandController();
    }
}

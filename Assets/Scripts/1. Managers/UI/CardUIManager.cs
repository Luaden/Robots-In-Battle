using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerDeckTransform;
    [SerializeField] private Transform opponentDeckTransform;
    [SerializeField] private PlayerHandUISlotManager playerHandSlotManager;
    [SerializeField] private Transform opponentHandTransform;

    private CardUIBuildController cardUIBuildController;

    public void BuildAndDrawPlayerCard(CardDataObject cardToDraw)
    {
        CardUIController drawnCard = cardUIBuildController.BuildPlayerCard(cardToDraw, playerDeckTransform);
        CombatManager.instance.PlayerHandSlotManager.AddItemToCollection(drawnCard, null);
    }

    public void BuildAndDrawOpponentCard(CardDataObject cardToDraw)
    {
        CardUIController drawnCard = cardUIBuildController.BuildOpponentCard(cardToDraw, opponentDeckTransform);
        CombatManager.instance.OpponentHandSlotManager.AddItemToCollection(drawnCard, null);
    }

    public void DestroyCardUI(CardDataObject cardToReturn)
    {
        Destroy(cardToReturn.CardUIObject);
    }

    private void Awake()
    {
        cardUIBuildController = FindObjectOfType<CardUIBuildController>(true);
    }
}

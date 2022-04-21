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
        cardUIBuildController.BuildAndDrawPlayerCard(cardToDraw, playerDeckTransform);
    }

    public void BuildAndDrawOpponentCard(CardDataObject cardToDraw)
    {
        cardUIBuildController.BuildAndDrawOpponentCard(cardToDraw, opponentDeckTransform);
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

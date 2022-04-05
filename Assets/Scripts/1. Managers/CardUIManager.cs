using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerDeckTransform;
    [SerializeField] private Transform opponentDeckTransform;
    [SerializeField] private Transform playerHandTransform;
    [SerializeField] private Transform opponentHandTransform;

    private CardUIBuildController cardUIBuildController;

    public void BuildAndDrawPlayerCard(CardDataObject cardToDraw)
    {
        cardUIBuildController.BuildAndDrawCard(cardToDraw, playerDeckTransform, playerHandTransform);
    }

    public void BuildAndDrawOpponentCard(CardDataObject cardToDraw)
    {
        cardUIBuildController.BuildAndDrawCard(cardToDraw, opponentDeckTransform, opponentHandTransform);
    }

    private void Awake()
    {
        cardUIBuildController = FindObjectOfType<CardUIBuildController>(true);
    }
}

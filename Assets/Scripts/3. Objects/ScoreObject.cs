using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject
{
    public float playerScorePercentile;
    public int turnLimitMaxPoints;
    public int playerTurnLimitPoints;

    public int winPoints;
    public int playerWinPoints;

    public int pointsGainedForNoHealthLoss;
    public int playerPointsGainedForHealthLoss;
    public bool hasWon;

    public ScoreObject(float playerScorePercentile, int turnLimitMaxPoints, int playerTurnLimitPoints, int winPoints, 
        int playerWinPoints, int pointsGainedForNoHealthLoss, int playerPointsGainedForHealthLoss, bool hasWon)
    {
        this.playerScorePercentile = playerScorePercentile;
        this.turnLimitMaxPoints = turnLimitMaxPoints;
        this.playerTurnLimitPoints = playerTurnLimitPoints;
        this.winPoints = winPoints;
        this.playerWinPoints = playerWinPoints;
        this.pointsGainedForNoHealthLoss = pointsGainedForNoHealthLoss;
        this.playerPointsGainedForHealthLoss = playerPointsGainedForHealthLoss;
        this.hasWon = hasWon;
    }
}

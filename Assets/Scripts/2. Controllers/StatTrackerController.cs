using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTrackerController : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int turnLimitForMaxPoints;
    [SerializeField] private int pointsGainedForWinUnderTurnLimit;
    [SerializeField] private int pointLossPerHealthLoss;
    [SerializeField] private int pointsGainedForNoHealthLoss;
    [SerializeField] private int pointsGainedForWin;

    private int playerStartingHealth;

    private int turnsTaken = 0;

    private int pointsForPlayerHP;
    private int pointsForTurnLimit;
    private int pointsForWin;
    private float playerTotalScore;

    public void InitializeStatTracking()
    {
        playerStartingHealth = CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
        turnsTaken = 0;
    }

    public void GameOver(bool playerWon)
    {
        int maxPoints = pointsGainedForNoHealthLoss + pointsGainedForWin + pointsGainedForWinUnderTurnLimit;

        if(CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP == playerStartingHealth)
        {
            pointsForPlayerHP = pointsGainedForNoHealthLoss;
        }
        else
        {
            int healthLost = playerStartingHealth - CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
            int pointLoss = pointLossPerHealthLoss * healthLost;
            pointsForPlayerHP = pointsGainedForNoHealthLoss - pointLoss;

        }

        if (turnsTaken <= turnLimitForMaxPoints)
            pointsForTurnLimit = pointsGainedForWinUnderTurnLimit;
        else
            pointsForTurnLimit = Mathf.RoundToInt(pointsGainedForWinUnderTurnLimit * ((float)turnLimitForMaxPoints / (float)turnsTaken));

        if (playerWon)
            pointsForWin = pointsGainedForWin;
        else
            pointsForWin = 0;

        playerTotalScore = ((float)(pointsForPlayerHP + pointsForTurnLimit + pointsForWin) / (float)maxPoints);
    }

    private void Start()
    {
        CombatSequenceManager.OnCombatComplete += OnCombatEnded;
    }
    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= OnCombatEnded;
    }

    private void OnCombatEnded()
    {
        turnsTaken += 1;
    }
}

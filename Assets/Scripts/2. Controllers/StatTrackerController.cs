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
    private int enemyStartingHealth;
    private int turnsTaken = 0;

    private int pointsForPlayerHP;
    private int pointsForTurnLimit;
    private int pointsForWin;
    private float playerTotalScore;

    public void InitializeStatTracking()
    {
        playerStartingHealth = CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
        enemyStartingHealth = CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP;
        turnsTaken = 0;
    }

    public void GameOver(bool playerWon)
    {
        int maxPoints = pointsGainedForNoHealthLoss + pointsGainedForWin + pointsGainedForWinUnderTurnLimit;

        pointsForPlayerHP = playerStartingHealth - CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
        pointsForPlayerHP *= pointLossPerHealthLoss;
        pointsForPlayerHP = pointsGainedForNoHealthLoss - pointsForPlayerHP;

        if (turnsTaken <= turnLimitForMaxPoints)
            pointsForTurnLimit = pointsGainedForWinUnderTurnLimit;
        else
        {
            pointsForTurnLimit = Mathf.RoundToInt(pointsGainedForWinUnderTurnLimit * (turnLimitForMaxPoints / turnsTaken));
        }

        if (playerWon)
            pointsForWin = pointsGainedForWin;
        else
            pointsForWin = 0;

        playerTotalScore = (pointsForPlayerHP + pointsForTurnLimit + pointsForWin) / maxPoints;
        Debug.Log("Player Score: " + playerTotalScore * 100);
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

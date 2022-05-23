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

    [ContextMenu("Test")]
    public void GameOver(bool playerWon)
    {
        int maxPoints = pointsGainedForNoHealthLoss + pointsGainedForWin + pointsGainedForWinUnderTurnLimit;
        Debug.Log("Max possible points: " + maxPoints);

        if(CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP == playerStartingHealth)
        {
            pointsForPlayerHP = pointsGainedForNoHealthLoss;
            Debug.Log("Points for Player HP: " + pointsForPlayerHP);
        }
        else
        {
            int healthLost = playerStartingHealth - CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
            int pointLoss = pointLossPerHealthLoss * healthLost;
            pointsForPlayerHP = pointsGainedForNoHealthLoss - pointLoss;
            Debug.Log("Points lost due to health loss: " + pointLoss);
            Debug.Log("Points for Player HP: " + pointsForPlayerHP);
        }

        if (turnsTaken <= turnLimitForMaxPoints)
        {
            pointsForTurnLimit = pointsGainedForWinUnderTurnLimit;
            Debug.Log("Points for Turn Limit: " + pointsForTurnLimit);
        }
        else
        {
            Debug.Log("Turn limit: " + turnLimitForMaxPoints);
            Debug.Log("Turns taken: " + turnsTaken);
            pointsForTurnLimit = Mathf.RoundToInt(pointsGainedForWinUnderTurnLimit * ((float)turnLimitForMaxPoints / (float)turnsTaken));
            Debug.Log("Points for Turn Limit: " + pointsForTurnLimit);
        }

        if (playerWon)
            pointsForWin = pointsGainedForWin;
        else
            pointsForWin = 0;

        playerTotalScore = ((float)(pointsForPlayerHP + pointsForTurnLimit + pointsForWin) / (float)maxPoints);
        Debug.Log("Player score percentile: " + (playerTotalScore * 100));
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

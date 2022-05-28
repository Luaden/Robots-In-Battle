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
    [Space]
    [Header("Debug")]
    [SerializeField] private bool testWin = true;
    [SerializeField] private bool bossKilled = true;

    private int playerStartingHealth;

    private int turnsTaken = 0;

    private int pointsForPlayerHP;
    private int pointsForTurnLimit;
    private int pointsForWin;
    private float playerPercentile;

    public void InitializeStatTracking()
    {
        playerStartingHealth = CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP;
        turnsTaken = 0;
    }

    public void GameOver(bool playerWon, bool bossKilled)
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
            pointsForPlayerHP = Mathf.Clamp(pointsGainedForNoHealthLoss - pointLoss, 0, int.MaxValue);

        }

        if (turnsTaken <= turnLimitForMaxPoints)
            pointsForTurnLimit = pointsGainedForWinUnderTurnLimit;
        else
            pointsForTurnLimit = Mathf.RoundToInt(pointsGainedForWinUnderTurnLimit * ((float)turnLimitForMaxPoints / (float)turnsTaken));

        if (playerWon)
            pointsForWin = pointsGainedForWin;
        else
            pointsForWin = 0;

        playerPercentile = ((float)(pointsForPlayerHP + pointsForTurnLimit + pointsForWin) / (float)maxPoints);

        ScoreObject newScoreObject = new ScoreObject(playerPercentile, pointsGainedForWinUnderTurnLimit, pointsForTurnLimit, pointsGainedForWin,
                                             pointsForWin, pointsGainedForNoHealthLoss, pointsForPlayerHP, playerWon);

        CombatManager.instance.WinLossPanelController.UpdateUI(newScoreObject, bossKilled);
    }

    [ContextMenu("Game Over")]
    private void GameOver()
    {
        GameOver(testWin, bossKilled);
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

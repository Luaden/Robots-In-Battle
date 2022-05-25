using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLossPanelController : BaseUIElement<ScoreObject>
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject starsBackground;
    [SerializeField] private GameObject starsForeground;
    [SerializeField] private Image starsForegroundFillImage;
    [SerializeField] private TMP_Text maxWinPointsText;
    [SerializeField] private TMP_Text maxHealthLossPointsText;
    [SerializeField] private TMP_Text maxTurnLimitPointsText;
    [SerializeField] private TMP_Text playerWinPointsText;
    [SerializeField] private TMP_Text playerHealthLossPointsText;
    [SerializeField] private TMP_Text playerTurnLimitPointsText;

    private ScoreObject currentScoreObject;
    private float playerPercentile;
    private int playerWinPoints;
    private int playerHealthLossPoints;
    private int playerTurnLimitPoints;

    private int playerCurrentWinPoints = 0;
    private int playerCurrentHealthLossPoints = 0;
    private int playerCurrentTurnLimitPoints = 0;
    private float playerCurrentStarPoints = 0f;

    private bool playerStatsSet = false;
    private bool playerHealthLossStatsComplete = false;
    private bool playerTurnLimitStatsComplete = false;
    private bool playerWinStatsComplete = false;
    private bool playerStarStatsComplete = false;

    private float currentTimer;

    private void Update()
    {
        UpdateStats();
    }

    private void SetGameOverText(ScoreObject newScoreObject)
    {
        currentScoreObject = newScoreObject;
        maxHealthLossPointsText.text = newScoreObject.pointsGainedForNoHealthLoss.ToString();
        maxTurnLimitPointsText.text = newScoreObject.turnLimitMaxPoints.ToString();
        maxWinPointsText.text = newScoreObject.winPoints.ToString();

        playerPercentile = newScoreObject.playerScorePercentile;
        playerWinPoints = newScoreObject.playerWinPoints;
        playerHealthLossPoints = newScoreObject.playerPointsGainedForHealthLoss;
        playerTurnLimitPoints = newScoreObject.playerTurnLimitPoints;

        playerStatsSet = true;
    }

    public override void UpdateUI(ScoreObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        if(primaryData.hasWon)
        {
            SetGameOverText(primaryData);

            winPanel.SetActive(true);
            starsBackground.SetActive(true);
            starsForeground.SetActive(true);
        }
        else
        {
            SetGameOverText(primaryData);

            losePanel.SetActive(true);
            starsBackground.SetActive(true);
            starsForeground.SetActive(true);
        }

    }

    protected override bool ClearedIfEmpty(ScoreObject newData)
    {
        if (newData == null)
            return true;

        return false;
    }

    private bool CheckTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= CombatManager.instance.PopupUIManager.TextPace)
        {
            currentTimer = 0f;
            return true;
        }

        return false;
    }

    private void UpdateStats()
    {
        if (!playerStatsSet)
            return;

        if(!playerHealthLossStatsComplete)
        {
            if(CheckTimer())
            {
                playerHealthLossPointsText.text = Mathf.RoundToInt(playerCurrentHealthLossPoints + Time.deltaTime).ToString();

                if(playerCurrentHealthLossPoints >= playerHealthLossPoints)
                {
                    playerCurrentHealthLossPoints = playerHealthLossPoints;
                    playerHealthLossPointsText.text = playerCurrentHealthLossPoints.ToString();
                    playerHealthLossStatsComplete = true;
                    return;
                }
            }
        }

        if (!playerTurnLimitStatsComplete)
        {
            if (CheckTimer())
            {
                playerTurnLimitPointsText.text = Mathf.RoundToInt(playerCurrentTurnLimitPoints + Time.deltaTime).ToString();

                if (playerCurrentTurnLimitPoints >= playerTurnLimitPoints)
                {
                    playerCurrentTurnLimitPoints = playerTurnLimitPoints;
                    playerTurnLimitPointsText.text = playerCurrentTurnLimitPoints.ToString();
                    playerTurnLimitStatsComplete = true;
                    return;
                }
            }
        }

        if (!playerWinStatsComplete)
        {
            if (CheckTimer())
            {
                playerWinPointsText.text = Mathf.RoundToInt(playerCurrentWinPoints + Time.deltaTime).ToString();

                if (playerCurrentWinPoints >= playerWinPoints)
                {
                    playerCurrentWinPoints = playerWinPoints;
                    playerWinPointsText.text = playerWinPoints.ToString();
                    playerWinStatsComplete = true;
                    return;
                }
            }
        }

        if (!playerStarStatsComplete)
        {
            if (CheckTimer())
            {
                starsForegroundFillImage.fillAmount = (playerCurrentStarPoints + Time.deltaTime);

                if (playerCurrentStarPoints >= playerPercentile)
                {
                    playerCurrentStarPoints = playerPercentile;
                    starsForegroundFillImage.fillAmount = playerPercentile;
                    playerStarStatsComplete = true;
                    return;
                }
            }
        }
    }
    //starsForeground.GetComponent<Image>().fillAmount = primaryData.playerScorePercentile;
}

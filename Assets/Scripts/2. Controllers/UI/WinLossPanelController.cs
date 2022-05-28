using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLossPanelController : BaseUIElement<ScoreObject, bool>
{
    [SerializeField] private float statCrawlSpeedModifier;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject creditsSceneButton;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject starsBackground;
    [SerializeField] private GameObject starsForeground;
    [SerializeField] private GameObject textHolder;
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

    private float playerCurrentWinPoints = 0;
    private float playerCurrentHealthLossPoints = 0;
    private float playerCurrentTurnLimitPoints = 0;
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
        maxHealthLossPointsText.text = "/ " + newScoreObject.pointsGainedForNoHealthLoss.ToString();
        maxTurnLimitPointsText.text = "/ " + newScoreObject.turnLimitMaxPoints.ToString();
        maxWinPointsText.text = "/ " + newScoreObject.winPoints.ToString();

        playerPercentile = newScoreObject.playerScorePercentile;
        playerWinPoints = newScoreObject.playerWinPoints;
        playerHealthLossPoints = newScoreObject.playerPointsGainedForHealthLoss;
        playerTurnLimitPoints = newScoreObject.playerTurnLimitPoints;

        starsBackground.SetActive(true);
        starsForeground.SetActive(true);
        textHolder.SetActive(true);
        playerStatsSet = true;
    }

    public override void UpdateUI(ScoreObject primaryData, bool bossKilled = false)
    {
        if (ClearedIfEmpty(primaryData, bossKilled))
            return;

        if(primaryData.hasWon)
        {
            AudioController.instance.PlayMusic(ThemeType.Win);
            SetGameOverText(primaryData);
            winPanel.SetActive(true);

            if(bossKilled)
                creditsSceneButton.SetActive(true);
        }
        else
        {
            AudioController.instance.PlayMusic(ThemeType.Loss);
            SetGameOverText(primaryData);
            losePanel.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(ScoreObject newData, bool bossKilled)
    {
        if (newData == null)
            return true;

        return false;
    }

    private void UpdateStats()
    {
        if (!playerStatsSet)
            return;

        if (!playerHealthLossStatsComplete)
        {
            playerCurrentHealthLossPoints += Time.deltaTime * statCrawlSpeedModifier;
            playerHealthLossPointsText.text = Mathf.RoundToInt(playerCurrentHealthLossPoints).ToString();

            if (playerCurrentHealthLossPoints >= playerHealthLossPoints)
            {
                playerCurrentHealthLossPoints = playerHealthLossPoints;
                playerHealthLossPointsText.text = playerCurrentHealthLossPoints.ToString();
                playerHealthLossStatsComplete = true;
                return;
            }

            return;
        }

        if (!playerTurnLimitStatsComplete)
        {
            playerCurrentTurnLimitPoints += Time.deltaTime * statCrawlSpeedModifier;
            playerTurnLimitPointsText.text = Mathf.RoundToInt(playerCurrentTurnLimitPoints).ToString();

            if (playerCurrentTurnLimitPoints >= playerTurnLimitPoints)
            {
                playerCurrentTurnLimitPoints = playerTurnLimitPoints;
                playerTurnLimitPointsText.text = playerCurrentTurnLimitPoints.ToString();
                playerTurnLimitStatsComplete = true;
                return;
            }

            return;
        }

        if (!playerWinStatsComplete)
        {
            playerCurrentWinPoints += Time.deltaTime * statCrawlSpeedModifier;
            playerWinPointsText.text = Mathf.RoundToInt(playerCurrentWinPoints).ToString();

            if (playerCurrentWinPoints >= playerWinPoints)
            {
                Debug.Log(playerWinPoints);
                playerCurrentWinPoints = playerWinPoints;
                playerWinPointsText.text = playerWinPoints.ToString();
                playerWinStatsComplete = true;
                return;
            }

            return;
        }

        if (!playerStarStatsComplete)
        {
            playerCurrentStarPoints += Time.deltaTime;
            starsForegroundFillImage.fillAmount = playerCurrentStarPoints;

            if (playerCurrentStarPoints >= playerPercentile)
            {
                playerCurrentStarPoints = playerPercentile;
                starsForegroundFillImage.fillAmount = playerPercentile;
                playerStarStatsComplete = true;
                playerStatsSet = false;
                return;
            }

            return;
        }
    }
}

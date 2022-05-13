using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHUDManager : MonoBehaviour
{
    [SerializeField] private HUDBarController playerHudBarController;
    [SerializeField] private HUDBarController opponentHudBarController;
    [SerializeField] private Transform playerPilotSpriteObjectPosition;
    [SerializeField] private Transform opponentPilotSpriteObjectPosition;

    public void UpdatePlayerHP(int playerHP)
    {
        playerHudBarController.UpdateHealthBar(playerHP);
    }

    public void UpdatePlayerPilotImage(GameObject playerSpriteObject)
    {
        Instantiate(playerSpriteObject, playerPilotSpriteObjectPosition);
    }

    public void UpdateOpponentPilotImage(GameObject opponentSpriteObject)
    {
        Instantiate(opponentSpriteObject, opponentPilotSpriteObjectPosition);
    }

    public void UpdatePlayerEnergy(int currentPlayerEnergy, int playerQueuedEnergy, bool queuedEnergy = false)
    {
        if(!queuedEnergy)
            playerHudBarController.UpdateEnergyBar(currentPlayerEnergy);
        else
        {
            playerHudBarController.UpdateEnergyQueueBar(currentPlayerEnergy, playerQueuedEnergy);
        }
    }

    public void UpdateOpponentHP(int opponentHP)
    {
        opponentHudBarController.UpdateHealthBar(opponentHP);
    }

    public void UpdateOpponentEnergy(int opponentEnergy, int opponentQueuedEnergy, bool queuedEnergy = false)
    {
        if(!queuedEnergy)
            opponentHudBarController.UpdateEnergyBar(opponentEnergy);
        else
            opponentHudBarController.UpdateEnergyQueueBar(opponentEnergy, opponentQueuedEnergy);
    }

    public void SetPlayerMaxStats(int playerHealth, int playerEnergy)
    {
        playerHudBarController.SetHealthBarMax(playerHealth);
        playerHudBarController.SetEnergyBarMax(playerEnergy);
        UpdatePlayerEnergy(playerEnergy, playerEnergy, false);
        UpdatePlayerHP(playerHealth);
    }

    public void SetOpponentMaxStats(int opponentHealth, int opponentEnergy)
    {
        opponentHudBarController.SetHealthBarMax(opponentHealth);
        opponentHudBarController.SetEnergyBarMax(opponentEnergy);
        UpdateOpponentEnergy(opponentEnergy, opponentEnergy, false);
        UpdateOpponentHP(opponentHealth);
    }
}

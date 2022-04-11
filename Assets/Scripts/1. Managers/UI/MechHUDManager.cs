using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHUDManager : MonoBehaviour
{
    [SerializeField] private HUDBarController playerHudBarController;
    [SerializeField] private HUDBarController opponentHudBarController;
    //private HUDBuffController hudBuffController;

    public void UpdatePlayerHP(int playerHP)
    {
        playerHudBarController.UpdateHealthBar(playerHP);
    }

    public void UpdatePlayerEnergy(int playerEnergy)
    {
        playerHudBarController.UpdateEnergyBar(playerEnergy);
    }

    public void UpdateOpponentHP(int opponentHP)
    {
        opponentHudBarController.UpdateHealthBar(opponentHP);
    }

    public void UpdateOpponentEnergy(int opponentEnergy)
    {
        opponentHudBarController.UpdateEnergyBar(opponentEnergy);
    }

    public void SetPlayerMaxStats(int playerHealth, int playerEnergy)
    {
        playerHudBarController.SetHealthBarMax(playerHealth);
        playerHudBarController.SetEnergyBarMax(playerEnergy);
    }

    public void SetOpponentMaxStats(int opponentHealth, int opponentEnergy)
    {
        opponentHudBarController.SetHealthBarMax(opponentHealth);
        opponentHudBarController.SetEnergyBarMax(opponentEnergy);
    }
}

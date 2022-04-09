using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHUDManager : MonoBehaviour
{
    [SerializeField] private HUDBarController playerHudBarController;
    [SerializeField] private HUDBarController opponentHudBarController;
    //private HUDBuffController hudBuffController;

    private void Awake()
    {
        //playerHudBarController = FindObjectOfType<HUDBarController>();
        //opponentHudBarController = FindObjectOfType<HUDBarController>();

        //hudBuffController = GetComponentInChildren<HUDBuffController>();
    }

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
}

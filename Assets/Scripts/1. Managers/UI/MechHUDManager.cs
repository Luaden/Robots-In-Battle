using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHUDManager : MonoBehaviour
{
    private HUDBarController playerHudBarController;
    private HUDBarController opponentHudBarController;
    //private HUDBuffController hudBuffController;

    public HUDBarController PlayerHUDBarController { get => playerHudBarController; }
    public HUDBarController OpponentHUDBarController { get => opponentHudBarController; }
    private void Awake()
    {
        playerHudBarController = transform.Find("Player").GetComponent<HUDBarController>();
        opponentHudBarController = transform.Find("Opponent").GetComponent<HUDBarController>();

        //hudBuffController = GetComponentInChildren<HUDBuffController>();
    }
}

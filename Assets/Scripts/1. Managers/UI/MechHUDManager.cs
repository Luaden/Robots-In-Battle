using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHUDManager : MonoBehaviour
{
    private HUDBarController playerHudBarController;
    private HUDBarController opponentHudBarController;
    private HUDBuffController hudBuffController;
    float elapseTime = 0.0f;


    private void Awake()
    {
        playerHudBarController = transform.Find("Player").GetComponent<HUDBarController>();
        opponentHudBarController = transform.Find("Opponent").GetComponent<HUDBarController>();
        hudBuffController = GetComponentInChildren<HUDBuffController>();
    }
    public void UpdateHUD(int value, CharacterSelect target) //, FigherObject fighterData
    {
        if (target == CharacterSelect.Player)
            playerHudBarController.UpdateUI(value);
        else
            opponentHudBarController.UpdateUI(value);
    }
    public void Update()
    {
        elapseTime += Time.deltaTime;
        if (elapseTime >= 0.25f)
        {
            UpdateHUD(10, CharacterSelect.Player);
            UpdateHUD(10, CharacterSelect.Opponent);
            elapseTime = 0;
        }
    }

}

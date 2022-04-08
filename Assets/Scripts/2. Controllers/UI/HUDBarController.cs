using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBarController : MonoBehaviour
{
    [SerializeField] protected HUDBar currentHealthBar;
    [SerializeField] protected HUDBar currentEnergyBar;


    public void UpdateHealthBar(int health) { currentHealthBar.UpdateUI(health); }
    public void UpdateEnergyBar(int energy) { currentEnergyBar.UpdateUI(energy); }

}

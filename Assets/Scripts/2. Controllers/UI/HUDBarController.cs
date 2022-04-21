using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBarController : MonoBehaviour
{
    [SerializeField] protected HUDBarObject currentHealthBar;
    [SerializeField] protected HUDBarObject currentEnergyBar;

    public void SetHealthBarMax(int maxHealth) { currentHealthBar.BarMax = maxHealth; }
    public void SetEnergyBarMax(int maxEnergy) { currentEnergyBar.BarMax = maxEnergy; }
    public void UpdateHealthBar(int health) { currentHealthBar.UpdateUI(health); }
    public void UpdateEnergyBar(int energy) { currentEnergyBar.UpdateUI(energy); }
}

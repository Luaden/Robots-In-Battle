using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBarController : MonoBehaviour
{
    [SerializeField] protected HUDBarObject currentHealthBar;
    [SerializeField] protected HUDBarObject queuedEnergyBar;
    [SerializeField] protected HUDBarSecondaryObject currentEnergyBar;

    public void SetHealthBarMax(int maxHealth) { currentHealthBar.BarMax = maxHealth; }
    public void UpdateHealthBar(int health) => currentHealthBar.UpdateUI(health);
    public void SetEnergyBarMax(int maxEnergy)
    {
        currentEnergyBar.BarMaxValue = maxEnergy;
        queuedEnergyBar.BarMax = maxEnergy;
    }

    public void UpdateEnergyBar(int energy) 
    { 
        currentEnergyBar.UpdateUI(energy, string.Empty);
        queuedEnergyBar.UpdateUI(energy);
    }

    public void UpdateEnergyQueueBar(int queuedEnergyTotal, int currentEnergy)
    {
        int queuedValue = currentEnergy - queuedEnergyTotal;
        string queuedString = "(" + queuedValue.ToString() + ")";
        currentEnergyBar.UpdateUI(currentEnergyBar.BarCurretValue, queuedString);
        queuedEnergyBar.UpdateUI(queuedEnergyTotal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBarController : BaseUIElement<int>
{
    [SerializeField] protected Image currentHealthBar;
    [SerializeField] protected Image currentEnergyBar;

    public override void UpdateUI(int primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        // fillAmount is between 0 - 1
        // so we have to convert the primarydata to become a floating point
        // and make it between 0 - 1 so it fills in correctly
        currentHealthBar.fillAmount -= (float)primaryData / 100; //fighterMaxHealth;
        currentEnergyBar.fillAmount -= (float)primaryData / 100; //figherMaxEnergy;
    }

    protected override bool ClearedIfEmpty(int newData)
    {
        if (newData == 0)
            return true;
        return false;
    }
}

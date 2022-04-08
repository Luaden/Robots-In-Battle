using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBar : BaseUIElement<int>
{
    private Image currentBar;

    private void Awake()
    {
        currentBar = GetComponent<Image>();
    }
    public override void UpdateUI(int primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        currentBar.fillAmount -= (float)primaryData / 100;
    }

    protected override bool ClearedIfEmpty(int newData)
    {
        if (newData == 0)
            return true;
        return false;
    }
}

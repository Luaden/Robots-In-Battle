using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBarObject : BaseUIElement<int>
{
    private Image currentBar;
    private int barMax;
    public int BarMax { get => barMax; set => barMax = value; }

    private void Awake()
    {
        currentBar = GetComponent<Image>();
    }

    public override void UpdateUI(int primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        currentBar.fillAmount = (float)primaryData / barMax;
    }

    protected override bool ClearedIfEmpty(int newData)
    {
        if (newData == 0)
            return true;
        return false;
    }
}

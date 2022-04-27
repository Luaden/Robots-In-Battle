using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDBarSecondaryObject : BaseUIElement<int, string>
{
    private TMP_Text text;
    private Image barImage;
    private int barMaxValue;
    private int barCurrentValue;
    public int BarMaxValue { get => barMaxValue; set => barMaxValue = value; }
    public int BarCurretValue { get => barCurrentValue; set => barCurrentValue = value; }

    private void Awake()
    {
        barImage = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public override void UpdateUI(int primaryData, string secondaryData)
    {
        if (ClearedIfEmpty(primaryData, secondaryData))
            return;

        barCurrentValue = primaryData;
        barImage.fillAmount = (float)primaryData / barMaxValue;
        text.text = secondaryData;
    }

    protected override bool ClearedIfEmpty(int newData, string secondaryData)
    {
        if (newData < 0)
            return true;
        return false;
    }
}

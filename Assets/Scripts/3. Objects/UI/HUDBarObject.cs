using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDBarObject : BaseUIElement<int, bool>
{
    private TMP_Text text;
    private Image currentBar;
    private int barMax;
    public int BarMax { get => barMax; set => barMax = value; }

    private void Awake()
    {
        currentBar = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public override void UpdateUI(int primaryData, bool updateText = true)
    {
        if (ClearedIfEmpty(primaryData, updateText))
        {
            currentBar.fillAmount = 0 / barMax;
            text.text = "0";
            return;
        }

        currentBar.fillAmount = (float)primaryData / barMax;

        if(updateText)
            text.text = primaryData.ToString();
    }

    protected override bool ClearedIfEmpty(int newData, bool updateText)
    {
        if (newData < 0)
            return true;
        return false;
    }
}


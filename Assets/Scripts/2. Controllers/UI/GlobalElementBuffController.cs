using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalElementBuffController : BaseUIElement<Dictionary<ElementType, int>>
{
    [SerializeField] private GameObject fireBuff;
    [SerializeField] private GameObject plasmaBuff;
    [SerializeField] private TMP_Text fireBuffText;
    [SerializeField] private TMP_Text plasmaBuffText;


    public override void UpdateUI(Dictionary<ElementType, int> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        int checkValue;

        if (primaryData.TryGetValue(ElementType.Fire, out checkValue))
        {
            fireBuff.SetActive(true);
            fireBuffText.text = checkValue.ToString();
        }

        if (primaryData.TryGetValue(ElementType.Plasma, out checkValue))
        {
            plasmaBuff.SetActive(true);
            plasmaBuffText.text = checkValue.ToString();
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<ElementType, int> newData)
    {
        int checkValue;

        if (newData.Keys.Count == 0)
        {
            fireBuffText.text = null;
            plasmaBuffText.text = null;

            fireBuff.SetActive(false);
            plasmaBuff.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(ElementType.Fire, out checkValue))
        {
            fireBuffText.text = null;
            fireBuff.SetActive(false);
        }

        if (!newData.TryGetValue(ElementType.Plasma, out checkValue))
        {
            plasmaBuffText.text = null;
            plasmaBuff.SetActive(false);
        }

        return false;
    }
}

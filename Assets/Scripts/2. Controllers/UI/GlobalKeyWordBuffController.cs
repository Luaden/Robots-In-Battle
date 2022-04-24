using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalKeyWordBuffController : BaseUIElement<Dictionary<CardKeyWord, List<CardEffectObject>>>
{
    [SerializeField] private GameObject flurryBuff;
    [SerializeField] private TMP_Text flurryBuffText;


    public override void UpdateUI(Dictionary<CardKeyWord, List<CardEffectObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if (primaryData.TryGetValue(CardKeyWord.Flurry, out checkValue))
        {
            int categoryTurnCount = 0;

            foreach (CardEffectObject cardEffect in checkValue)
                categoryTurnCount += cardEffect.EffectMagnitude;

            flurryBuffText.text = categoryTurnCount.ToString();
            flurryBuff.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<CardKeyWord, List<CardEffectObject>> newData)
    {
        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if (newData.Keys.Count == 0)
        {
            flurryBuffText.text = null;
            flurryBuff.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(CardKeyWord.Flurry, out checkValue))
        {
            flurryBuffText.text = null;
            flurryBuff.SetActive(false);

            return true;
        }

        if(checkValue == null || checkValue.Count == 0)
        {
            flurryBuffText.text = null;
            flurryBuff.SetActive(false);

            return true;
        }

        return false;
    }
}

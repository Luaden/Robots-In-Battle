using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalCategoryBuffController : BaseUIElement<Dictionary<CardCategory, List<CardEffectObject>>>
{
    [SerializeField] private GameObject punchBuff;
    [SerializeField] private GameObject kickBuff;
    [SerializeField] private GameObject specialBuff;
    [SerializeField] private TMP_Text punchBuffText;
    [SerializeField] private TMP_Text kickBuffText;
    [SerializeField] private TMP_Text specialBuffText;

    public override void UpdateUI(Dictionary<CardCategory, List<CardEffectObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if (primaryData.TryGetValue(CardCategory.Punch, out checkValue))
        {
            int categoryTurnCount = 0;
            
            foreach(CardEffectObject cardEffect in checkValue)
            {
                if(cardEffect.EffectDuration > categoryTurnCount)
                    categoryTurnCount = cardEffect.EffectDuration;
            }

            punchBuffText.text = categoryTurnCount.ToString();
            punchBuff.SetActive(true);
        }

        if (primaryData.TryGetValue(CardCategory.Kick, out checkValue))
        {
            int categoryTurnCount = 0;

            foreach (CardEffectObject cardEffect in checkValue)
            {
                if (cardEffect.EffectDuration > categoryTurnCount)
                    categoryTurnCount = cardEffect.EffectDuration;
            }

            kickBuffText.text = categoryTurnCount.ToString();
            kickBuff.SetActive(true);
        }

        if (primaryData.TryGetValue(CardCategory.Special, out checkValue))
        {
            int categoryTurnCount = 0;

            foreach (CardEffectObject cardEffect in checkValue)
            {
                if (cardEffect.EffectDuration > categoryTurnCount)
                    categoryTurnCount = cardEffect.EffectDuration;
            }

            specialBuffText.text = categoryTurnCount.ToString();
            specialBuff.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<CardCategory, List<CardEffectObject>> newData)
    {
        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if (newData.Keys.Count == 0)
        {
            punchBuffText.text = null;
            kickBuffText.text = null;
            specialBuffText.text = null;

            punchBuff.SetActive(false);
            kickBuff.SetActive(false);
            specialBuff.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(CardCategory.Punch, out checkValue))
        {
            punchBuffText.text = null;
            punchBuff.SetActive(false);
        }

        if (!newData.TryGetValue(CardCategory.Kick, out checkValue))
        {
            kickBuffText.text = null;
            kickBuff.SetActive(false);
        }

        if (!newData.TryGetValue(CardCategory.Special, out checkValue))
        {
            specialBuffText.text = null;
            specialBuff.SetActive(false);
        }

        return false;
    }
}

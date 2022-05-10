using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalCategoryBuffController : BaseUIElement<Dictionary<CardCategory, List<CardEffectObject>>>
{
    [SerializeField] private GameObject punchBuff;
    [SerializeField] private GameObject kickBuff;
    [SerializeField] private GameObject specialBuff;
    [SerializeField] private GameObject jazzersizeBuff;
    [SerializeField] private TMP_Text punchBuffText;
    [SerializeField] private TMP_Text kickBuffText;
    [SerializeField] private TMP_Text specialBuffText;
    [SerializeField] private TMP_Text jazzersizeBuffText;

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

    public void UpdateUI(Dictionary<ActiveEffects, int> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        int checkValue;

        if (primaryData.TryGetValue(ActiveEffects.Jazzersize, out checkValue))
        {
            jazzersizeBuffText.text = checkValue.ToString();
            jazzersizeBuff.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<CardCategory, List<CardEffectObject>> newData)
    {
        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if (newData.Keys.Count == 0)
        {
            punchBuffText.text = string.Empty;
            kickBuffText.text = string.Empty;
            specialBuffText.text = string.Empty;

            punchBuff.SetActive(false);
            kickBuff.SetActive(false);
            specialBuff.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(CardCategory.Punch, out checkValue))
        {
            punchBuffText.text = string.Empty;
            punchBuff.SetActive(false);
        }

        if (!newData.TryGetValue(CardCategory.Kick, out checkValue))
        {
            kickBuffText.text = string.Empty;
            kickBuff.SetActive(false);
        }

        if (!newData.TryGetValue(CardCategory.Special, out checkValue))
        {
            specialBuffText.text = string.Empty;
            specialBuff.SetActive(false);
        }

        return false;
    }

    protected bool ClearedIfEmpty(Dictionary<ActiveEffects, int> newData)
    {
        if (newData.Keys.Count == 0)
        {
            jazzersizeBuffText.text = string.Empty;
            jazzersizeBuff.SetActive(false);
            return true;
        }

        return false;
    }
}

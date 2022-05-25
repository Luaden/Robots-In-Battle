using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChannelDamageBuffController : BaseUIElement<Dictionary<Channels, List<CardEffectObject>>>
{
    [SerializeField] private GameObject highDamageUpIndicator;
    [SerializeField] private TMP_Text highDamageUpIndicatorText;
    [SerializeField] private GameObject highDamageDownIndicator;
    [SerializeField] private TMP_Text highDamageDownIndicatorText;

    [SerializeField] private GameObject midDamageUpIndicator;
    [SerializeField] private TMP_Text midDamageUpIndicatorText;
    [SerializeField] private GameObject midDamageDownIndicator;
    [SerializeField] private TMP_Text midDamageDownIndicatorText;

    [SerializeField] private GameObject lowDamageUpIndicator;
    [SerializeField] private TMP_Text lowDamageUpIndicatorText;
    [SerializeField] private GameObject lowDamageDownIndicator;
    [SerializeField] private TMP_Text lowDamageDownIndicatorText;

    public override void UpdateUI(Dictionary<Channels, List<CardEffectObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<CardEffectObject> checkValue = new List<CardEffectObject>();
        int damageModifier = 0;

        if (primaryData.TryGetValue(Channels.High, out checkValue))
        {
            foreach(CardEffectObject effect in checkValue)
            {
                if (effect.EffectType == CardEffectTypes.IncreaseOutgoingChannelDamage)
                    damageModifier += effect.EffectMagnitude;
                if (effect.EffectType == CardEffectTypes.ReduceOutgoingChannelDamage)
                    damageModifier -= effect.EffectMagnitude;
            }

            if(damageModifier > 0)
            {
                highDamageUpIndicatorText.text = damageModifier.ToString();
                highDamageUpIndicator.SetActive(true);
            }
            else if(damageModifier < 0)
            {
                highDamageDownIndicatorText.text = damageModifier.ToString();
                highDamageDownIndicator.SetActive(true);
            }
            else
            {
                highDamageDownIndicator.SetActive(false);
                highDamageUpIndicator.SetActive(false);
            }

        }

        if (primaryData.TryGetValue(Channels.Mid, out checkValue))
        {
            foreach (CardEffectObject effect in checkValue)
            {
                if (effect.EffectType == CardEffectTypes.IncreaseOutgoingChannelDamage)
                    damageModifier += effect.EffectMagnitude;
                if (effect.EffectType == CardEffectTypes.ReduceOutgoingChannelDamage)
                    damageModifier -= effect.EffectMagnitude;
            }

            if (damageModifier > 0)
            {
                midDamageUpIndicatorText.text = damageModifier.ToString();
                midDamageUpIndicator.SetActive(true);
            }
            else if (damageModifier < 0)
            {
                midDamageDownIndicatorText.text = damageModifier.ToString();
                midDamageDownIndicator.SetActive(true);
            }
            else
            {
                midDamageDownIndicator.SetActive(false);
                midDamageUpIndicator.SetActive(false);
            }
        }

        if (primaryData.TryGetValue(Channels.Low, out checkValue))
        {
            foreach (CardEffectObject effect in checkValue)
            {
                if (effect.EffectType == CardEffectTypes.IncreaseOutgoingChannelDamage)
                    damageModifier += effect.EffectMagnitude;
                if (effect.EffectType == CardEffectTypes.ReduceOutgoingChannelDamage)
                    damageModifier -= effect.EffectMagnitude;
            }

            if (damageModifier > 0)
            {
                lowDamageUpIndicatorText.text = damageModifier.ToString();
                lowDamageUpIndicator.SetActive(true);
            }
            else if (damageModifier < 0)
            {
                lowDamageDownIndicatorText.text = damageModifier.ToString();
                lowDamageUpIndicator.SetActive(true);
            }
            else
            {
                lowDamageUpIndicator.SetActive(false);
                lowDamageDownIndicator.SetActive(false);
            }
        }
    }

    protected override bool ClearedIfEmpty(Dictionary<Channels, List<CardEffectObject>> newData)
    {
        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if(newData.Keys.Count == 0)
        {
            highDamageUpIndicatorText.text = string.Empty;
            highDamageDownIndicatorText.text = string.Empty;
            midDamageUpIndicatorText.text = string.Empty;
            midDamageDownIndicatorText.text = string.Empty;
            lowDamageUpIndicatorText.text = string.Empty;
            lowDamageDownIndicatorText.text = string.Empty;

            highDamageUpIndicator.SetActive(false);
            highDamageDownIndicator.SetActive(false);
            midDamageUpIndicator.SetActive(false);
            midDamageDownIndicator.SetActive(false);
            lowDamageUpIndicator.SetActive(false);
            lowDamageDownIndicator.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(Channels.High, out checkValue))
        {
            highDamageUpIndicatorText.text = string.Empty;
            highDamageDownIndicatorText.text = string.Empty;

            highDamageUpIndicator.SetActive(false);
            highDamageDownIndicator.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Mid, out checkValue))
        {
            midDamageUpIndicatorText.text = string.Empty;
            midDamageDownIndicatorText.text = string.Empty;

            midDamageUpIndicator.SetActive(false);
            midDamageDownIndicator.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Low, out checkValue))
        {
            lowDamageUpIndicatorText.text = string.Empty;
            lowDamageDownIndicatorText.text = string.Empty;

            lowDamageUpIndicator.SetActive(false);
            lowDamageDownIndicator.SetActive(false);
        }

        return false;
    }
}

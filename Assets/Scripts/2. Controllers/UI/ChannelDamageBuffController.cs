using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChannelDamageBuffController : BaseUIElement<Dictionary<Channels, List<CardEffectObject>>>
{
    [SerializeField] private GameObject highDamageIndicator;
    [SerializeField] private TMP_Text highDamageIndicatorText;
    [SerializeField] private GameObject midDamageIndicator;
    [SerializeField] private TMP_Text midDamageIndicatorText;
    [SerializeField] private GameObject lowDamageIndicator;
    [SerializeField] private TMP_Text lowDamageIndicatorText;

    public override void UpdateUI(Dictionary<Channels, List<CardEffectObject>> primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        List<CardEffectObject> checkValue = new List<CardEffectObject>();
        int damageModifier = 0;
        //int damageModifierTurnCount = 0;

        if (primaryData.TryGetValue(Channels.High, out checkValue))
        {
            foreach(CardEffectObject effect in checkValue)
            {
                if (effect.EffectType == CardEffectTypes.IncreaseOutgoingChannelDamage)
                    damageModifier += effect.EffectMagnitude;
                if (effect.EffectType == CardEffectTypes.ReduceOutgoingChannelDamage)
                    damageModifier -= effect.EffectMagnitude;
            }

            highDamageIndicatorText.text = damageModifier.ToString();
            highDamageIndicator.SetActive(true);
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

            midDamageIndicatorText.text = damageModifier.ToString();
            midDamageIndicator.SetActive(true);
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

            lowDamageIndicatorText.text = damageModifier.ToString();
            lowDamageIndicator.SetActive(true);
        }

        //Change damage indicator to reflect net effect.
    }

    protected override bool ClearedIfEmpty(Dictionary<Channels, List<CardEffectObject>> newData)
    {
        List<CardEffectObject> checkValue = new List<CardEffectObject>();

        if(newData.Keys.Count == 0)
        {
            highDamageIndicatorText.text = null;
            midDamageIndicatorText.text = null;
            lowDamageIndicatorText.text = null;

            highDamageIndicator.SetActive(false);
            midDamageIndicator.SetActive(false);
            lowDamageIndicator.SetActive(false);

            return true;
        }

        if (!newData.TryGetValue(Channels.High, out checkValue))
        {
            highDamageIndicatorText.text = null;
            highDamageIndicator.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Mid, out checkValue))
        {
            midDamageIndicatorText.text = null;
            midDamageIndicator.SetActive(false);
        }

        if (!newData.TryGetValue(Channels.Low, out checkValue))
        {
            lowDamageIndicatorText.text = null;
            lowDamageIndicator.SetActive(false);
        }

        return false;
    }
}

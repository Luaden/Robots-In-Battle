using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffectObject
{
    private Dictionary<ElementType, int> elementStacks = new Dictionary<ElementType, int>();
    private Dictionary<CardCategory, List<CardEffectObject>> cardCategoryDamageBonus = new Dictionary<CardCategory, List<CardEffectObject>>();
    private Dictionary<Channels, List<CardEffectObject>> channelDamageBonus = new Dictionary<Channels, List<CardEffectObject>>();
    private Dictionary<Channels, List<CardEffectObject>> channelDamagePercentBonus = new Dictionary<Channels, List<CardEffectObject>>();
    private Dictionary<Channels, List<CardEffectObject>> channelDamageReduction = new Dictionary<Channels, List<CardEffectObject>>();
    private Dictionary<Channels, List<CardEffectObject>> channelDamagePercentReduction = new Dictionary<Channels, List<CardEffectObject>>();
    private Dictionary<Channels, int> channelShields = new Dictionary<Channels, int>();
    private Dictionary<CardKeyWord, List<CardEffectObject>> keywordDuration = new Dictionary<CardKeyWord, List<CardEffectObject>>();

    public Dictionary<ElementType,int> ElementStacks { get => elementStacks; }
    public Dictionary<CardCategory, List<CardEffectObject>> CardCategoryDamageBonus { get => cardCategoryDamageBonus; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamageBonus { get => channelDamageBonus; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamagePercentBonus { get => channelDamagePercentBonus; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamageReduction { get => channelDamageReduction; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamagePercentReduction { get => channelDamagePercentReduction; }
    public Dictionary<Channels, int> ChannelShields { get => channelShields; }
    public Dictionary<CardKeyWord, List<CardEffectObject>> KeyWordDuration { get => keywordDuration; }

    public void IncrementFighterEffects()
    {
        ReduceElementStacks(ElementStacks);
        ReduceCategoryDamageBonusEffects(CardCategoryDamageBonus);
        ReduceChannelEffects(ChannelDamageBonus);
        ReduceKeyWordEffects(KeyWordDuration);
        ReduceChannelEffects(ChannelDamageReduction);
    }

    private void ReduceElementStacks(Dictionary<ElementType, int> elementDict)
    {
        List<KeyValuePair<ElementType, int>> elementKVPairList = new List<KeyValuePair<ElementType, int>>();

        foreach (KeyValuePair<ElementType, int> pair in elementDict)
        {
            //Deal damage
            elementDict[pair.Key] = pair.Value - 1;

            if (elementDict[pair.Key] == 0)
                elementKVPairList.Add(pair);
        }

        foreach (KeyValuePair<ElementType, int> pair in elementKVPairList)
            elementDict.Remove(pair.Key);

        //Update buff popups
    }

    private void ReduceCategoryDamageBonusEffects(Dictionary<CardCategory, List<CardEffectObject>> cardCategoryDict)
    {
        List<KeyValuePair<CardCategory, List<CardEffectObject>>> categoryKVPairList = new List<KeyValuePair<CardCategory, List<CardEffectObject>>>();
        List<CardEffectObject> effectList = new List<CardEffectObject>();

        foreach (KeyValuePair<CardCategory, List<CardEffectObject>> pair in cardCategoryDict)
        {
            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    effectList.Add(effect);
            }

            foreach (CardEffectObject effect in effectList)
            {
                Debug.Log("Removing effect from dictionary list.");
                pair.Value.Remove(effect);
            }

            if (pair.Value.Count == 0)
            {
                Debug.Log("Removing KV pair from dictionary.");
                categoryKVPairList.Add(pair);
            }
        }

        foreach (KeyValuePair<CardCategory, List<CardEffectObject>> pair in categoryKVPairList)
            cardCategoryDict.Remove(pair.Key);

        //Update buff popups

    }

    private void ReduceChannelEffects(Dictionary<Channels, List<CardEffectObject>> channelDamageDict)
    {
        List<KeyValuePair<Channels, List<CardEffectObject>>> channelsKVPairList = new List<KeyValuePair<Channels, List<CardEffectObject>>>();
        List<CardEffectObject> channelEffectList = new List<CardEffectObject>();

        foreach (KeyValuePair<Channels, List<CardEffectObject>> pair in channelDamageDict)
        {
            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    channelEffectList.Add(effect);
            }

            foreach (CardEffectObject effect in channelEffectList)
                pair.Value.Remove(effect);

            if (pair.Value.Count == 0)
                channelsKVPairList.Add(pair);
        }

        foreach (KeyValuePair<Channels, List<CardEffectObject>> pair in channelsKVPairList)
            channelDamageDict.Remove(pair.Key);

        //Update buff popups

    }

    private void ReduceKeyWordEffects(Dictionary<CardKeyWord, List<CardEffectObject>> keyWordDict)
    {
        List<KeyValuePair<CardKeyWord, List<CardEffectObject>>> keyWordKVPair = new List<KeyValuePair<CardKeyWord, List<CardEffectObject>>>();
        List<CardEffectObject> keyWordEffectList = new List<CardEffectObject>();

        foreach (KeyValuePair<CardKeyWord, List<CardEffectObject>> pair in keyWordDict)
        {
            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    keyWordEffectList.Add(effect);
            }

            foreach (CardEffectObject effect in keyWordEffectList)
                pair.Value.Remove(effect);

            if (pair.Value.Count == 0)
                keyWordKVPair.Add(pair);
        }

        foreach (KeyValuePair<CardKeyWord, List<CardEffectObject>> pair in keyWordKVPair)
            keyWordDict.Remove(pair.Key);
    }

}

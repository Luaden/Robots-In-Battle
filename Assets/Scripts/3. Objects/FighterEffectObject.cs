using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffectObject
{
    private Dictionary<ElementType, int> elementStacks = new Dictionary<ElementType, int>();
    private Dictionary<CardCategory, CardEffectObject> cardCategoryDamageBonus = new Dictionary<CardCategory, CardEffectObject>();
    private Dictionary<Channels, CardEffectObject> channelDamageBonus = new Dictionary<Channels, CardEffectObject>();
    private Dictionary<Channels, CardEffectObject> channelDamagePercentBonus = new Dictionary<Channels, CardEffectObject>();
    private Dictionary<Channels, CardEffectObject> channelDamageReduction = new Dictionary<Channels, CardEffectObject>();
    private Dictionary<Channels, CardEffectObject> channelDamagePercentReduction = new Dictionary<Channels, CardEffectObject>();
    private Dictionary<Channels, int> channelShields = new Dictionary<Channels, int>();
    private Dictionary<CardKeyWord, CardEffectObject> keywordDuration = new Dictionary<CardKeyWord, CardEffectObject>();

    public Dictionary<ElementType,int> ElementStacks { get => elementStacks; }
    public Dictionary<CardCategory, CardEffectObject> CardCategoryDamageBonus { get => cardCategoryDamageBonus; }
    public Dictionary<Channels, CardEffectObject> ChannelDamageBonus { get => channelDamageBonus; }
    public Dictionary<Channels, CardEffectObject> ChannelDamagePercentBonus { get => channelDamagePercentBonus; }
    public Dictionary<Channels, CardEffectObject> ChannelDamageReduction { get => channelDamageReduction; }
    public Dictionary<Channels, CardEffectObject> ChannelDamagePercentReduction { get => channelDamagePercentReduction; }
    public Dictionary<Channels, int> ChannelShields { get => channelShields; }
    public Dictionary<CardKeyWord, CardEffectObject> KeyWordDuration { get => keywordDuration; }

}

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

}

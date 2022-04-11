using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffectObject
{
    private Dictionary<ElementType, int> elementStacks = new Dictionary<ElementType, int>();
    private Dictionary<CardCategory, int> cardCategoryDamageBonus = new Dictionary<CardCategory, int>();
    private Dictionary<Channels, int> channelDamageBonus = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamagePercentBonus = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamageReduction = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamagePercentReduction = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelShields = new Dictionary<Channels, int>();

    public Dictionary<ElementType,int> ElementStacks { get => elementStacks; }
    public Dictionary<CardCategory, int> CardCategoryDamageBonus { get => cardCategoryDamageBonus; }
    public Dictionary<Channels, int> ChannelDamageBonus { get => channelDamageBonus; }
    public Dictionary<Channels, int> ChannelDamagePercentBonus { get => channelDamagePercentBonus; }
    public Dictionary<Channels, int> ChannelDamageReduction { get => channelDamageReduction; }
    public Dictionary<Channels, int> ChannelDamagePercentReduction { get => channelDamagePercentReduction; }
    public Dictionary<Channels, int> ChannelShields { get => channelShields; }
}

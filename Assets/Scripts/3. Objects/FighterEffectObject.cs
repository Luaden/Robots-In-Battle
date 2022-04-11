using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffectObject
{
    private Dictionary<ElementType, int> elementsStacked = new Dictionary<ElementType, int>();
    private Dictionary<CardCategory, int> cardCategoryDamageBonus = new Dictionary<CardCategory, int>();
    private Dictionary<Channels, int> channelDamageBonus = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamagePercentBonus = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamageReduction = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelDamagePercentReduction = new Dictionary<Channels, int>();
    private Dictionary<Channels, int> channelShields = new Dictionary<Channels, int>();
}

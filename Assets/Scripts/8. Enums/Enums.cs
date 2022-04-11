using System;
using UnityEngine;

public enum ItemType
{
    Card,
    Component
}

public enum CardType
{
    Attack,
    Defense,
    Neutral
}

[Flags]
public enum CardCategory
{
    None = 0,
    Punch = 1 << 0,
    Kick = 1 << 1,
    Special = 1 << 2,

    Guard = 1 << 3,
    Counter = 1 << 4, 

    [InspectorName(null)]
    Offensive = Punch | Kick | Special,
    [InspectorName(null)]
    Defensive = Guard | Counter,
    [InspectorName(null)]
    All = ~0
}

[Flags]
public enum Channels
{
    None = 0,
    High = 1 << 0,
    Mid = 1 << 1,
    Low = 1 << 2,
    All = ~0,
}

public enum AffectedChannels
{
    SelectedChannel,
    AllPossibleChannels
}

public enum CardEffectTypes
{
    None = 0,
    PlayMultipleTimes = 1,
    AdditionalElementStacks = 2,
    IncreaseOutgoingChannelDamage = 3,
    IncreaseOutgoingCardTypeDamage = 4,
    ReduceIncomingChannelDamage = 5, 
    GainShields = 6,
    MultiplyShield = 7,
    KeyWordInitialize = 8,
    KeyWordExecute = 9
}

public enum CardKeyWord
{
    Flurry = 1,
}

public enum ElementType
{
    None = 0,
    Fire = 1, // Stacks, damage at end of turn
    Ice = 2, // Stacks, raises energy cost in channel
    Plasma = 3, // Stacks, siphons energy at end of turn
    Acid = 4, // Increases damage dealt to components.
}

[Flags]
public enum PilotEffects
{
    None = 0,
    BonusMoney = 1 << 0,
    EnergyGain = 1 << 1,
    SelfRepair = 1 << 2,
    ShopRarity = 1 << 3
}

public enum EffectTarget
{
    Player,
    Opponent
}

public enum MechComponent
{
    Head,
    Torso,
    Arms,
    Legs,
    Back
}

public enum CharacterSelect
{
    Player,
    Opponent
}
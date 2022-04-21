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

public enum AnimationType
{
    Idle,
    Punch,
    Kick,
    Special,
    Guard,
    Counter,
    Damaged,
    Win,
    Lose,
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
    IncreaseOutgoingChannelDamage = 3, //Channel buff
    IncreaseOutgoingCardTypeDamage = 4, //Global buff
    ReduceIncomingChannelDamage = 5,  //Channel buff
    GainShields = 6,  //Channel buff
    MultiplyShield = 7, 
    KeyWordInitialize = 8, //Global buff
    KeyWordExecute = 9,
    GainShieldWithFalloff = 10, //Channel buff
    //EnergyDestroy = 11,
    [InspectorName(null)]
    Fire = 11, // Global buff
    [InspectorName(null)]
    Plasma = 12, // Global buff
    [InspectorName(null)]
    Ice = 13, // Channel buff
    [InspectorName(null)]
    Acid = 14, // Channel buff
}

public enum CardKeyWord
{
    None = 0,
    Flurry = 1,
}

public enum ElementType
{
    None = 0,
    Fire = 1, // Stacks, damage at end of turn 
    Ice = 2, // Stacks, raises energy cost in channel
    Plasma = 3, // Stacks, siphons energy at end of turn
    Acid = 4, // Increases damage dealt to components. 
    //Void = 5, //Stacks,  nullifies elements
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
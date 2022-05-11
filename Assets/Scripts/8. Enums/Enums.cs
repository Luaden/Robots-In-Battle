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
    All = High | Mid | Low,
    [InspectorName(null)]
    HighMid = High | Mid,
    [InspectorName(null)]
    LowMid = Low | Mid,
}

public enum AffectedChannels
{
    SelectedChannel,
    AllPossibleChannels
}

[Flags]
public enum CardEffectTypes
{
    None = 0,
    PlayMultipleTimes = 1 << 0,
    AdditionalElementStacks = 1 << 1, 
    IncreaseOutgoingChannelDamage = 1 << 2, 
    IncreaseOutgoingCardTypeDamage = 1 << 3,
    ReduceOutgoingChannelDamage = 1 << 4,
    GainShields = 1 << 5,
    MultiplyShield =  1 << 6, 
    KeyWord = 1 << 7,
    GainShieldWithFalloff = 1 << 8,
    EnergyDestroy = 1 << 9,
    ShieldDestroy = 1 << 10,

    [InspectorName(null)]
    Defensive = GainShields | MultiplyShield | ReduceOutgoingChannelDamage | GainShieldWithFalloff,
    [InspectorName(null)]
    Offensive = PlayMultipleTimes | AdditionalElementStacks | IncreaseOutgoingChannelDamage | 
        IncreaseOutgoingCardTypeDamage | KeyWord | EnergyDestroy | ShieldDestroy
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
    Void = 5, //Stacks,  nullifies elements
}

[Flags]
public enum PassiveEffects
{
    None = 0,
    BonusMoney = 1 << 0,
    EnergyGain = 1 << 1,
    SelfRepair = 1 << 2,
    ShopRarity = 1 << 3
}

[Flags]
public enum ActiveEffects
{
    None = 0,
    Jazzersize = 1 << 0,
}

public enum EffectTarget
{
    Player,
    Opponent
}

public enum MechComponent
{
    None,
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

public enum PilotType
{
    Unique,
    Generic
}

public enum EventRequirement
{
    HasEnoughMoney,
}

public enum EventOutcome
{
    DamageNextEnemy,
    DamagePlayer,
    GainOrLoseMoney,
    GainOrLoseTime,
    TempWinningsModifier,
    AddItemToPlayer,

    ////SpecificEvents that will need a different system.
    //Investment,
    //ANightOut,
    //ExperimentalFuel,
    //BetOnOtherFighter,
    //BadLuck
}

public enum EventObjective
{
    EndFightBelowHPPercent,
    EndFightWithEquippedItem,
}

public enum GeneralHUDElement
{
    None,

    //General HUD
    HighChannel,
    MidChannel,
    LowChannel,

    Health,
    Energy,

    AttackSlot,
    DefenseSlot,
}

public enum MechHUDElement
{
    //MechComponents
    PlayerMech,
    OpponentMech,

    OpponentMechHighChannel,
    OpponentMechMidChannel,
    OpponentMechLowChannel,
}

public enum BuffHUDElement
{
    //Buffs
    Shield,
    DamageModifier,
    Flurry,
    Ice,
    Acid,
    Fire,
    Plasma,
    Jazzersize
}
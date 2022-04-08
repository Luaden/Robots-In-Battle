using System;
using UnityEngine;

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
    High = 1,
    Mid = 2,
    Low = 4,

    //HighMid = High | Mid,
    //HighLow = High | Low,
    //MidLow = High | Low,
    All = ~0,
}

public enum AffectedChannels
{
    SelectedChannel,
    AllPossibleChannels
}

public enum CardEffectTypes
{
    
}

public enum ActiveFighterEffectTypes
{

}

public enum PassiveFighterEffectTypes
{

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
    Legs
}

public enum CharacterSelect
{
    Player,
    Opponent
}
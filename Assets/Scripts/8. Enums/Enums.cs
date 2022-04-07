using System;
public enum CardType
{
    Attack,
    Defense,
    Neutral
}

public enum CardCategory
{
    None,
    Punch,
    Kick,
    Special,
    Guard,
    Counter
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
    All = High | Mid | Low
}

public enum AffectedChannels
{
    SelectedChannel,
    AllPossibleChannels
}

public enum CardEffectTypes
{
    
}

public enum ComponentEffectTypes
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
    Body,
    Arms,
    Legs
}

public enum CharacterSelect
{
    Player,
    Opponent
}
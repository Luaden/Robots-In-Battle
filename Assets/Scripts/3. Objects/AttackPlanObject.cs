using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Containing information about the planned attack
public class AttackPlanObject
{
    private CardChannelPairObject cardChannelPairObjectA;
    private CardChannelPairObject cardChannelPairObjectB;
    private CharacterSelect characterOrigin;
    private CharacterSelect characterDestination;

    public CardChannelPairObject cardChannelPairA { get => cardChannelPairA; }
    public CardChannelPairObject cardChannelPairB { get => cardChannelPairB; }
    public CharacterSelect CharacterOrigin { get => characterOrigin; }
    public CharacterSelect CharacterDestination { get => characterDestination; }

    public AttackPlanObject(CardChannelPairObject cardChannelPairA,
                            CardChannelPairObject cardChannelPairB,
                            CharacterSelect origin, 
                            CharacterSelect destination)
    {
        cardChannelPairObjectA = cardChannelPairA;
        cardChannelPairObjectB = cardChannelPairB;
        characterOrigin = origin;
        characterDestination = destination;
    }

}

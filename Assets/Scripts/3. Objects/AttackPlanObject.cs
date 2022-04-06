using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Containing information about the planned attack
public class AttackPlanObject
{
    protected List<CardChannelPairObject> cardChannelPairObjects;
    protected CharacterOrigin characterOrigin;
    protected CharacterDestination characterDestination;

    public AttackPlanObject(List<CardChannelPairObject> cardChannelPairObjects, 
                            CharacterOrigin characterOrigin, 
                            CharacterDestination characterDestination)
    {
        this.cardChannelPairObjects = cardChannelPairObjects;
        this.characterOrigin = characterOrigin;
        this.characterDestination = characterDestination;
    }

}

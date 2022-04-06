using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Containing information about the planned attack
public class AttackPlanObject
{
    protected List<CardChannelPairObject> cardChannelPairObjects;
    protected CharacterSelect characterOrigin;
    protected CharacterSelect characterDestination;

    public AttackPlanObject(List<CardChannelPairObject> cardChannelPairObjects, 
                            CharacterSelect characterOrigin, 
                            CharacterSelect characterDestination)
    {
        this.cardChannelPairObjects = cardChannelPairObjects;
        this.characterOrigin = characterOrigin;
        this.characterDestination = characterDestination;
    }

}

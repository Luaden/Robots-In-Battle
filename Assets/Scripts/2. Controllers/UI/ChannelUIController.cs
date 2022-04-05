using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelUIController : MonoBehaviour
{
    //Card UI placements
    private List<CardChannelPairObject> cardChannelPairObjects;

    public AttackPlanObject BuildAttackPlanObject()
    {
        // testing
        CharacterDestination destination = CharacterDestination.Player;
        CharacterOrigin origin = CharacterOrigin.Opponent;

        // create object
        AttackPlanObject attackPlanObject = new AttackPlanObject(cardChannelPairObjects, origin, destination);

        // ends turn

        //send object to CardPlayManager
        return attackPlanObject;
    }
}

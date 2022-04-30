using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMechPair
{
    public int damageToDeal;
    public Channels damageChannels;
    public CharacterSelect characterTakingDamage;

    public DamageMechPair(int damage, Channels channel, CharacterSelect character)
    {
        damageToDeal = damage;
        damageChannels = channel;
        characterTakingDamage = character;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMechPairObject
{
    private CardChannelPairObject attack;
    private bool counterDamage;
    private bool guardDamage;
    private CharacterSelect characterTakingDamage;

    public CharacterSelect CharacterTakingDamage { get => characterTakingDamage; }

    public DamageMechPairObject(CardChannelPairObject attack, CharacterSelect characterTakingDamage, bool counter, bool guard)
    {
        this.attack = attack;
        this.characterTakingDamage = characterTakingDamage;
        counterDamage = counter;
        guardDamage = guard;
    }

    public int GetDamageToDeal()
    {
        int damageToReturn = 
            CombatManager.instance.CardPlayManager.EffectController.GetMechDamageWithAndConsumeModifiers(attack, characterTakingDamage);

        if (counterDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.CounterDamageMultiplier);
        if (guardDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.GuardDamageMultiplier);

        return damageToReturn;
    }

    public Channels GetDamageChannels()
    {
        if (attack.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            return attack.CardData.PossibleChannels;
        else
            return attack.CardChannel;
    }
}
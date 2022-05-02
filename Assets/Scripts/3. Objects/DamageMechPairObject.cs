using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMechPairObject
{
    private CardChannelPairObject cardChannelPair;
    private bool counterDamage;
    private bool guardDamage;
    private CharacterSelect characterTakingDamage;

    public CharacterSelect CharacterTakingDamage { get => characterTakingDamage; }
    public CardChannelPairObject CardChannelPair { get => cardChannelPair; }

    public DamageMechPairObject(CardChannelPairObject attack, CharacterSelect characterTakingDamage, bool counter, bool guard)
    {
        this.cardChannelPair = attack;
        this.characterTakingDamage = characterTakingDamage;
        counterDamage = counter;
        guardDamage = guard;
    }

    public int GetDamageToDeal()
    {
        int damageToReturn = 
            CombatManager.instance.EffectManager.GetMechDamageWithAndConsumeModifiers(cardChannelPair, characterTakingDamage);

        if (counterDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.CounterDamageMultiplier);
        if (guardDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.GuardDamageMultiplier);

        return damageToReturn;
    }

    public Channels GetDamageChannels()
    {
        if (cardChannelPair.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            return cardChannelPair.CardData.PossibleChannels;
        else
            return cardChannelPair.CardChannel;
    }
}
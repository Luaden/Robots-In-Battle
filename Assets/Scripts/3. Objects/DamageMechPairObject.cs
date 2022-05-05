using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMechPairObject
{
    private CardCharacterPairObject cardCharacterPairA;
    private CardCharacterPairObject cardCharacterPairB;
    private bool counterDamage;
    private bool guardDamage;
    private string combatLog;

    public CharacterSelect CharacterTakingDamage { get => cardCharacterPairA.character; }
    public CardCharacterPairObject CardCharacterPairA { get => cardCharacterPairA; }
    public CardCharacterPairObject CardCharacterPairB { get => cardCharacterPairB; }
    public DamageMechPairObject(CardCharacterPairObject attackA, CardCharacterPairObject attackB, bool counter, bool guard, string combatLog = null)
    {
        cardCharacterPairA = attackA;
        cardCharacterPairB = attackB;
        counterDamage = counter;
        guardDamage = guard;
        this.combatLog = combatLog;
    }

    public int GetDamageToDeal()
    {
        int damageToReturn = 
            CombatManager.instance.EffectManager.GetMechDamageWithAndConsumeModifiers(cardCharacterPairA.cardChannelPair, cardCharacterPairA.character);

        if (counterDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.CounterDamageMultiplier);
        if (guardDamage)
            return Mathf.RoundToInt(damageToReturn * CombatManager.instance.GuardDamageMultiplier);

        if (CombatManager.instance.NarrateCombat)
            Debug.Log(combatLog);

        return damageToReturn;
    }

    public Channels GetDamageChannels()
    {
        if (cardCharacterPairA.cardChannelPair.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            return cardCharacterPairA.cardChannelPair.CardData.PossibleChannels;
        else
            return cardCharacterPairA.cardChannelPair.CardChannel;
    }
}
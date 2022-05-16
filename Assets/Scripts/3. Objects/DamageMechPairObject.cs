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
    public bool CounterDamage { get => counterDamage; }
    public bool GuardDamage { get => guardDamage; }
    public bool DenyOffensiveEffects { get => counterDamage; }
    
    public DamageMechPairObject(CardCharacterPairObject attackA, CardCharacterPairObject attackB, bool counter, bool guard, string combatLog = null)
    {
        cardCharacterPairA = attackA;
        cardCharacterPairB = attackB;
        counterDamage = counter;
        guardDamage = guard;
        this.combatLog = combatLog;
    }

    public int GetDamageWithAndConsumeModifiers()
    {
        return CombatManager.instance.CombatEffectManager.GetDamageWithAndConsumeModifiers(cardCharacterPairA.cardChannelPair,
            cardCharacterPairA.character, counterDamage, guardDamage);
    }

    public Vector2Int GetDamageAndShieldWithModifiers()
    {
        return CombatManager.instance.CombatEffectManager.GetDamageWithModifiersAndShield(cardCharacterPairA.cardChannelPair,
            cardCharacterPairA.character, counterDamage, guardDamage);
    }

    public Channels GetDamageChannels()
    {
        if (cardCharacterPairA.cardChannelPair.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            return cardCharacterPairA.cardChannelPair.CardData.PossibleChannels;
        else
            return cardCharacterPairA.cardChannelPair.CardChannel;
    }
}
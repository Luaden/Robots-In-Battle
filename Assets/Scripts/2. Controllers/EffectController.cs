using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private FighterEffectObject playerFighterEffectObject;
    private FighterEffectObject opponentFighterEffectObject;

    //Get defensive reductions with channel
    //Get offensive boosts with channel

    public void LogNewEffect(CardEffectObject cardEffect)
    {
        switch(cardEffect.EffectType)
        {
            case CardEffectTypes.PlayMultipleTimes:
                break;
            case CardEffectTypes.AdditionalElementStacks:
                break;
            case CardEffectTypes.GainShields:
                break;
            case CardEffectTypes.IncreaseOutgoingCardTypeDamage:
                break;
            case CardEffectTypes.IncreaseOutgoingChannelDamage:
                break;
            case CardEffectTypes.ReduceIncomingChannelDamage:
                break;
            case CardEffectTypes.KeyWordBoost:
                break;
        }

        //Check if this is new or if this is an addition
        //Log incoming effect
        //Create or update popup or icon for buff
    }

    public int GetDamageWithReductions(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        return 0;
    }

    public int GetDamageWithBoost(CardChannelPairObject attack, CharacterSelect offensiveCharacter)
    {
        return 0;
    }

    public void CheckEffectsAtTurnEnd()
    {
        //Iterate through elements
        //Clean all other effects out
    }

    private void Start()
    {
        CardPlayManager.OnCombatComplete += CheckEffectsAtTurnEnd;

        playerFighterEffectObject = new FighterEffectObject();
        opponentFighterEffectObject = new FighterEffectObject();
    }

    private void OnDestroy()
    {
        CardPlayManager.OnCombatComplete -= CheckEffectsAtTurnEnd;
    }
}

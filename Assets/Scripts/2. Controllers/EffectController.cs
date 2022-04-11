using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private FighterEffectObject playerFighterEffectObject;
    private FighterEffectObject opponentFighterEffectObject;

    public void EnableEffects(CardChannelPairObject cardChannelPair, CharacterSelect destinationMech)
    {
        int repeatPlay = 1;

        foreach (SOCardEffectObject effect in cardChannelPair.CardData.CardEffects)
            if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                repeatPlay = effect.EffectMagnitude;

        for (int i = 0; i < repeatPlay; i++)
            foreach (SOCardEffectObject effect in cardChannelPair.CardData.CardEffects)
            {
                switch (effect.EffectType)
                {
                    case CardEffectTypes.PlayMultipleTimes:
                        break;

                    case CardEffectTypes.AdditionalElementStacks:
                        switch(cardChannelPair.CardData.CardCategory)
                        {
                            case CardCategory.Punch:
                                AddElementalStacks(effect, MechComponent.Arms, destinationMech);
                                break;
                            case CardCategory.Kick:
                                AddElementalStacks(effect, MechComponent.Legs, destinationMech);
                                break;
                            case CardCategory.Special:
                                AddElementalStacks(effect, MechComponent.Torso, destinationMech);
                                break;
                        }
                        break;

                    case CardEffectTypes.GainShields:
                        GainShields(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.MultiplyShield:
                        MultiplyShields(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.IncreaseOutgoingCardTypeDamage:
                        BoostCardTypeDamage(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.IncreaseOutgoingChannelDamage:
                        BoostChannelDamage(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.ReduceIncomingChannelDamage:
                        ReduceChannelDamage(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.KeyWordInitialize:
                        KeyWordInitialize(effect, cardChannelPair.CardChannel, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;
                    case CardEffectTypes.KeyWordExecute:
                        break;
                }
            }

        //Create or update popup or icon for buff
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

    private int GetDamageWithReductions(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        return 0;
    }

    private int GetDamageWithBoost(CardChannelPairObject attack, CharacterSelect offensiveCharacter)
    {
        return 0;
    }

    private void AddElementalStacks(SOCardEffectObject effect, MechComponent component, CharacterSelect characterAdding)
    {
        CardEffectObject newEffect = new CardEffectObject(effect);
        int currentStacks;
        int newStacks = effect.EffectMagnitude;

        switch (component)
        {
            case MechComponent.Head:
                break;

            case MechComponent.Torso:
                break;

            case MechComponent.Arms:

                if (characterAdding == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechArms.ComponentElement;

                    if (playerFighterEffectObject.ElementStacks.TryGetValue(currentElement, out currentStacks))
                    {
                        newStacks += currentStacks;
                        newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechArms.ExtraElementStacks;

                        playerFighterEffectObject.ElementStacks[currentElement] = newStacks;
                    }
                    else
                        playerFighterEffectObject.ElementStacks.Add(currentElement, newStacks);
                }

                if (characterAdding == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechArms.ComponentElement;

                    if (opponentFighterEffectObject.ElementStacks.TryGetValue(currentElement, out currentStacks))
                    {
                        newStacks += currentStacks;
                        newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechArms.ExtraElementStacks;

                        opponentFighterEffectObject.ElementStacks[currentElement] = newStacks;
                    }
                    else
                        opponentFighterEffectObject.ElementStacks.Add(currentElement, newStacks);
                }
                break;

            case MechComponent.Legs:
                if (characterAdding == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechLegs.ComponentElement;

                    if (playerFighterEffectObject.ElementStacks.TryGetValue(currentElement, out currentStacks))
                    {
                        newStacks += currentStacks;
                        newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechLegs.ExtraElementStacks;

                        playerFighterEffectObject.ElementStacks[currentElement] = newStacks;
                    }
                    else
                        playerFighterEffectObject.ElementStacks.Add(currentElement, newStacks);
                }

                if (characterAdding == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechLegs.ComponentElement;

                    if (opponentFighterEffectObject.ElementStacks.TryGetValue(currentElement, out currentStacks))
                    {
                        newStacks += currentStacks;
                        newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.ExtraElementStacks;

                        opponentFighterEffectObject.ElementStacks[currentElement] = newStacks;
                    }
                    else
                        opponentFighterEffectObject.ElementStacks.Add(currentElement, newStacks);
                }
                break;
        }
    }

    private void GainShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        int shieldAmount;
        if (characterGaining == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.ChannelShields.TryGetValue(channel, out shieldAmount))
                playerFighterEffectObject.ChannelShields[channel] =
                    playerFighterEffectObject.ChannelShields[channel] + effect.EffectMagnitude;
            else
                playerFighterEffectObject.ChannelShields.Add(channel, effect.EffectMagnitude);
        }

        if (characterGaining == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelShields.TryGetValue(channel, out shieldAmount))
                opponentFighterEffectObject.ChannelShields[channel] =
                    opponentFighterEffectObject.ChannelShields[channel] + effect.EffectMagnitude;
            else
                opponentFighterEffectObject.ChannelShields.Add(channel, effect.EffectMagnitude);
        }
    }

    private void MultiplyShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        if (characterGaining == CharacterSelect.Player)
        {
            int shieldAmount;

            if (playerFighterEffectObject.ChannelShields.TryGetValue(channel, out shieldAmount))
                playerFighterEffectObject.ChannelShields[channel] =
                    playerFighterEffectObject.ChannelShields[channel] * effect.EffectMagnitude;
            else
                return;
        }

        if (characterGaining == CharacterSelect.Opponent)
        {
            int shieldAmount;

            if (opponentFighterEffectObject.ChannelShields.TryGetValue(channel, out shieldAmount))
                opponentFighterEffectObject.ChannelShields[channel] =
                    opponentFighterEffectObject.ChannelShields[channel] * effect.EffectMagnitude;
            else
                return;
        }
    }

    private void BoostCardTypeDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterBoosting)
    {
        CardEffectObject previousBoost;
        CardEffectObject newBoost = new CardEffectObject(effect);

        if (characterBoosting == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.CardCategoryDamageBonus.TryGetValue(effect.CardTypeToBoost, out previousBoost))
            {
                newBoost.EffectMagnitude += effect.EffectMagnitude;
                newBoost.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                playerFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] = newBoost;
            }
            else
                playerFighterEffectObject.CardCategoryDamageBonus.Add(effect.CardTypeToBoost, newBoost);
        }

        if (characterBoosting == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.CardCategoryDamageBonus.TryGetValue(effect.CardTypeToBoost, out previousBoost))
            {
                newBoost.EffectMagnitude += effect.EffectMagnitude;
                newBoost.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                opponentFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] = newBoost;
            }
            else
                opponentFighterEffectObject.CardCategoryDamageBonus.Add(effect.CardTypeToBoost, newBoost);
        }
    }

    private void BoostChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterBoosting)
    {
        CardEffectObject previousBoost;
        CardEffectObject newBoost = new CardEffectObject(effect);

        if (characterBoosting == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousBoost))
            {
                newBoost.EffectMagnitude += effect.EffectMagnitude;
                newBoost.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                playerFighterEffectObject.ChannelDamageBonus[channel] = newBoost;
            }
            else
                playerFighterEffectObject.ChannelDamageBonus.Add(channel, newBoost);
        }

        if (characterBoosting == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousBoost))
            {
                newBoost.EffectMagnitude += effect.EffectMagnitude;
                newBoost.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                opponentFighterEffectObject.ChannelDamageBonus[channel] = newBoost;
            }
            else
                opponentFighterEffectObject.ChannelDamageBonus.Add(channel, newBoost);
        }
    }

    private void ReduceChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterReducing)
    {
        CardEffectObject previousReduction;
        CardEffectObject newReduction = new CardEffectObject(effect);

        if (characterReducing == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.ChannelDamageReduction.TryGetValue(channel, out previousReduction))
            {
                newReduction.EffectMagnitude += effect.EffectMagnitude;
                newReduction.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                playerFighterEffectObject.ChannelDamageReduction[channel] = newReduction;
            }
            else
                playerFighterEffectObject.ChannelDamageReduction.Add(channel, newReduction);
        }

        if (characterReducing == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousReduction))
            {
                newReduction.EffectMagnitude += effect.EffectMagnitude;
                newReduction.EffectDuration += effect.EffectDuration;
                //We could assign the current turn here the same way to not refresh the duration.
                //We could also make them a list of effects and track them additively / individually.

                opponentFighterEffectObject.ChannelDamageBonus[channel] = newReduction;
            }
            else
                opponentFighterEffectObject.ChannelDamageBonus.Add(channel, newReduction);
        }
    }

    private void KeyWordInitialize(SOCardEffectObject effect, Channels channel, CharacterSelect characterPriming)
    {
        CardEffectObject currentKeyWordEffect;
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (characterPriming == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.KeyWordDuration.TryGetValue(effect.CardKeyWord, out currentKeyWordEffect))
            {
                newKeyWordEffect.EffectDuration += currentKeyWordEffect.EffectDuration;
                playerFighterEffectObject.KeyWordDuration[effect.CardKeyWord] = newKeyWordEffect;
            }
            else
                playerFighterEffectObject.KeyWordDuration.Add(effect.CardKeyWord, newKeyWordEffect);
        }

        if (characterPriming == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(effect.CardKeyWord, out currentKeyWordEffect))
            {
                newKeyWordEffect.EffectDuration += currentKeyWordEffect.EffectDuration;
                opponentFighterEffectObject.KeyWordDuration[effect.CardKeyWord] = newKeyWordEffect;
            }
            else
                opponentFighterEffectObject.KeyWordDuration.Add(effect.CardKeyWord, newKeyWordEffect);
        }
    }

    private void CheckEffectsAtTurnEnd()
    {
        //Iterate through elements
        //Clean all other effects out
    }
}

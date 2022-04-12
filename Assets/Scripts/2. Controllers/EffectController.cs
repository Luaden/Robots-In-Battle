using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectController
{
    private FighterEffectObject playerFighterEffectObject;
    private FighterEffectObject opponentFighterEffectObject;

    public void EnableEffects(CardChannelPairObject cardChannelPair, CharacterSelect destinationMech)
    {
        if (cardChannelPair.CardData.CardEffects == null)
            return;

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
                                AddElementalStacks(effect, MechComponent.Head, destinationMech);
                                break;
                        }
                        break;

                    case CardEffectTypes.GainShields:
                        GainShields(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels, 
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.MultiplyShield:
                        MultiplyShields(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.IncreaseOutgoingCardTypeDamage:
                        BoostCardTypeDamage(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.IncreaseOutgoingChannelDamage:
                        BoostChannelDamage(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.ReduceIncomingChannelDamage:
                        ReduceChannelDamage(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.KeyWordInitialize:
                        KeyWordInitialize(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;
                    case CardEffectTypes.KeyWordExecute:
                        break;
                }
            }

        //Create or update popup or icon for buff
    }

    public int GetMechDamageWithModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        int damageToReturn = attack.CardData.BaseDamage;

        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            //Get Damage Boosts and Modifiers
            damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);
            damageToReturn = GetComponentDamageBonus(attack, damageToReturn, defensiveCharacter);

            //Get Damage Reductions and Modifiers
            damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);
        }

        if (defensiveCharacter == CharacterSelect.Player)
        {
            //Get Damage Boosts and Modifiers
            damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);
            damageToReturn = GetComponentDamageBonus(attack, damageToReturn, defensiveCharacter);

            //Get Damage Reductions and Modifiers
            damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
            damageToReturn = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);
        }


        return damageToReturn;
    }

    public int GetComponentDamageWithModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        int damageToReturn = attack.CardData.BaseDamage;
        //Check for CardType boost.
        //Check for ChannelType boost.
        //Check for KeywordExecute boost.
        //Check for component damage bonus.

        //Check for channel reductions
        //Check for shields
        //Check for component damage reduction.
        //Return damage.

        return damageToReturn;
    }
    
    private void IncrementEffectsAtTurnEnd()
    {
        playerFighterEffectObject.IncrementFighterEffects();
        opponentFighterEffectObject.IncrementFighterEffects();
    }

    public EffectController()
    {
        CardPlayManager.OnCombatComplete += IncrementEffectsAtTurnEnd;
        CombatManager.OnDestroyScene += OnDestroy;

        playerFighterEffectObject = new FighterEffectObject();
        opponentFighterEffectObject = new FighterEffectObject();
    }

    private void OnDestroy()
    {
        CardPlayManager.OnCombatComplete -= IncrementEffectsAtTurnEnd;
        CombatManager.OnDestroyScene -= OnDestroy;
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

    private int GetCardCategoryDamageBonus(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        List<CardEffectObject> previousCardCategoryEffects = new List<CardEffectObject>();

        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    if (playerFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Punch, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Kick:
                    if (playerFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Kick, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Special:
                    if (playerFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Special, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    if (opponentFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Punch, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Kick:
                    if (opponentFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Kick, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Special:
                    if (opponentFighterEffectObject.CardCategoryDamageBonus.TryGetValue(CardCategory.Special, out previousCardCategoryEffects))
                        foreach (CardEffectObject effect in previousCardCategoryEffects)
                            damageToReturn += effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
    }

    private int GetCardChannelDamageBonus(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        List<CardEffectObject> previousChannelEffects = new List<CardEffectObject>();

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach(Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (playerFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn += effect.EffectMagnitude;
            }

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (opponentFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn += effect.EffectMagnitude;
            }

            return damageToReturn;
        }
    }

    private int GetKeyWordDamageBonus(CardChannelPairObject attack, ref int damageToReturn, CharacterSelect defensiveCharacter)
    {
        CardKeyWord keyWord = CardKeyWord.None;
        List<CardEffectObject> previousKeyWordEffects = new List<CardEffectObject>();

        if (attack.CardData.CardEffects.Select(x => x.EffectType).Contains(CardEffectTypes.KeyWordExecute))
        {
            foreach (SOCardEffectObject effect in attack.CardData.CardEffects)
                if (effect.EffectType == CardEffectTypes.KeyWordExecute)
                    keyWord = effect.CardKeyWord;

            if(defensiveCharacter == CharacterSelect.Opponent)
            {
                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                        damageToReturn += effect.EffectMagnitude;
            }
            else
            {
                if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                        damageToReturn += effect.EffectMagnitude;
            }
        }

        return damageToReturn;
    }

    private int GetComponentDamageBonus(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    if (CombatManager.instance.PlayerFighter.FighterMech.MechArms.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.PlayerFighter.FighterMech.MechArms.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.PlayerFighter.FighterMech.MechArms.BonusDamageFromComponent;
                    break;
                case CardCategory.Kick:
                    if (CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    break;
                case CardCategory.Special:
                    if (CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.PlayerFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    if (CombatManager.instance.OpponentFighter.FighterMech.MechArms.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.OpponentFighter.FighterMech.MechArms.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.OpponentFighter.FighterMech.MechArms.BonusDamageFromComponent;
                    break;
                case CardCategory.Kick:
                    if (CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    break;
                case CardCategory.Special:
                    if (CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageAsPercent)
                        damageToReturn *= CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    else
                        damageToReturn += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    break;
            }

            return damageToReturn;
        }
    }

    private int GetCardChannelDamageReduction(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        List<CardEffectObject> previousChannelEffects = new List<CardEffectObject>();

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (opponentFighterEffectObject.ChannelDamageReduction.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn -= effect.EffectMagnitude;
            }

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (playerFighterEffectObject.ChannelDamageReduction.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn -= effect.EffectMagnitude;
            }

            return damageToReturn;
        }
    }
    
    private int GetDamageReducedByShield(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        int initialShield;

        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(channel, out initialShield))
                {
                    int shieldAmount = initialShield;

                    shieldAmount -= damageToReturn;
                    damageToReturn = Mathf.RoundToInt(Mathf.Clamp(damageToReturn - initialShield, 0, Mathf.Infinity));


                    if (shieldAmount <= 0)
                        opponentFighterEffectObject.ChannelShields.Remove(channel);
                    else
                        opponentFighterEffectObject.ChannelShields[attack.CardChannel] = shieldAmount;
                }
            }

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in GetChannelListFromFlags(attack.CardChannel))
            {
                if (playerFighterEffectObject.ChannelShields.TryGetValue(channel, out initialShield))
                {
                    int shieldAmount = initialShield;

                    shieldAmount -= damageToReturn;
                    damageToReturn = Mathf.RoundToInt(Mathf.Clamp(damageToReturn - initialShield, 0, Mathf.Infinity));


                    if (shieldAmount <= 0)
                        playerFighterEffectObject.ChannelShields.Remove(channel);
                    else
                        playerFighterEffectObject.ChannelShields[attack.CardChannel] = shieldAmount;
                }
            }                

            return damageToReturn;
        }
    }

    private void GainShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        int shieldAmount;

        if (characterGaining == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (playerFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    playerFighterEffectObject.ChannelShields[returnedChannel] =
                        playerFighterEffectObject.ChannelShields[returnedChannel] + effect.EffectMagnitude;
                else
                    playerFighterEffectObject.ChannelShields.Add(returnedChannel, effect.EffectMagnitude);
            }
                
        }

        if (characterGaining == CharacterSelect.Opponent)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    opponentFighterEffectObject.ChannelShields[returnedChannel] =
                        opponentFighterEffectObject.ChannelShields[returnedChannel] + effect.EffectMagnitude;
                else
                    opponentFighterEffectObject.ChannelShields.Add(returnedChannel, effect.EffectMagnitude);
            }
        }
    }

    private void MultiplyShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        int shieldAmount;

        if (characterGaining == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (playerFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    playerFighterEffectObject.ChannelShields[returnedChannel] =
                        playerFighterEffectObject.ChannelShields[returnedChannel] * effect.EffectMagnitude;
                else
                    return;
            }
        }

        if (characterGaining == CharacterSelect.Opponent)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    opponentFighterEffectObject.ChannelShields[returnedChannel] =
                        opponentFighterEffectObject.ChannelShields[returnedChannel] * effect.EffectMagnitude;
                else
                    return;
            }
        }
    }

    private void BoostCardTypeDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterBoosting)
    {
        List<CardEffectObject> previousBoostList = new List<CardEffectObject>();
        CardEffectObject newBoost = new CardEffectObject(effect);

        if (characterBoosting == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.CardCategoryDamageBonus.TryGetValue(effect.CardTypeToBoost, out previousBoostList))
            {
                previousBoostList.Add(newBoost);
                playerFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] = previousBoostList;
            }
            else
            {
                List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                newBoostList.Add(newBoost);
                playerFighterEffectObject.CardCategoryDamageBonus.Add(effect.CardTypeToBoost, newBoostList);
            }
                
        }

        if (characterBoosting == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.CardCategoryDamageBonus.TryGetValue(effect.CardTypeToBoost, out previousBoostList))
            {
                previousBoostList.Add(newBoost);
                opponentFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] = previousBoostList;
            }
            else
            {
                List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                newBoostList.Add(newBoost);
                opponentFighterEffectObject.CardCategoryDamageBonus.Add(effect.CardTypeToBoost, newBoostList);
            }
        }
    }

    private void BoostChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterBoosting)
    {
        List<CardEffectObject> previousBoostList = new List<CardEffectObject>();
        CardEffectObject newBoost = new CardEffectObject(effect);

        if (characterBoosting == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (playerFighterEffectObject.ChannelDamageBonus.TryGetValue(returnedChannel, out previousBoostList))
                {
                    previousBoostList.Add(newBoost);
                    playerFighterEffectObject.ChannelDamageBonus[returnedChannel] = previousBoostList;
                }
                else
                {
                    List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                    newBoostList.Add(newBoost);
                    playerFighterEffectObject.ChannelDamageBonus.Add(returnedChannel, newBoostList);
                }
            }
        }

        if (characterBoosting == CharacterSelect.Opponent)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelDamageBonus.TryGetValue(returnedChannel, out previousBoostList))
                {
                    previousBoostList.Add(newBoost);
                    opponentFighterEffectObject.ChannelDamageBonus[returnedChannel] = previousBoostList;
                }
                else
                {
                    List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                    newBoostList.Add(newBoost);
                    opponentFighterEffectObject.ChannelDamageBonus.Add(returnedChannel, newBoostList);
                }
            }
        }
    }

    private void ReduceChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterReducing)
    {
        List<CardEffectObject> previousReductionList = new List<CardEffectObject>();
        CardEffectObject newReduction = new CardEffectObject(effect);

        if (characterReducing == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (playerFighterEffectObject.ChannelDamageReduction.TryGetValue(returnedChannel, out previousReductionList))
                {
                    previousReductionList.Add(newReduction);
                    playerFighterEffectObject.ChannelDamageReduction[returnedChannel] = previousReductionList;
                }
                else
                {
                    List<CardEffectObject> newReductionList = new List<CardEffectObject>();
                    newReductionList.Add(newReduction);
                    playerFighterEffectObject.ChannelDamageReduction.Add(returnedChannel, newReductionList);
                }
            }
        }

        if (characterReducing == CharacterSelect.Opponent)
        {
            foreach (Channels returnedChannel in GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelDamageReduction.TryGetValue(returnedChannel, out previousReductionList))
                {
                    previousReductionList.Add(newReduction);
                    opponentFighterEffectObject.ChannelDamageReduction[returnedChannel] = previousReductionList;
                }
                else
                {
                    List<CardEffectObject> newReductionList = new List<CardEffectObject>();
                    newReductionList.Add(newReduction);
                    opponentFighterEffectObject.ChannelDamageReduction.Add(returnedChannel, newReductionList);
                }
            }
        }
    }

    private void KeyWordInitialize(SOCardEffectObject effect, Channels channel, CharacterSelect characterPriming)
    {
        List<CardEffectObject> currentKeyWordEffectList = new List<CardEffectObject>();
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (characterPriming == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.KeyWordDuration.TryGetValue(effect.CardKeyWord, out currentKeyWordEffectList))
            {
                currentKeyWordEffectList.Add(newKeyWordEffect);
                playerFighterEffectObject.KeyWordDuration[effect.CardKeyWord] = currentKeyWordEffectList;
            }
            else
            {
                List<CardEffectObject> newKeyWordList = new List<CardEffectObject>();
                newKeyWordList.Add(newKeyWordEffect);
                playerFighterEffectObject.KeyWordDuration.Add(effect.CardKeyWord, newKeyWordList);
            }
        }

        if (characterPriming == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(effect.CardKeyWord, out currentKeyWordEffectList))
            {
                currentKeyWordEffectList.Add(newKeyWordEffect);
                opponentFighterEffectObject.KeyWordDuration[effect.CardKeyWord] = currentKeyWordEffectList;
            }
            else
            {
                List<CardEffectObject> newKeyWordList = new List<CardEffectObject>();
                newKeyWordList.Add(newKeyWordEffect);
                opponentFighterEffectObject.KeyWordDuration.Add(effect.CardKeyWord, newKeyWordList);
            }
        }
    }

    private List<Channels> GetChannelListFromFlags(Channels channelToInterpret)
    {
        List<Channels> channelList = new List<Channels>();

        if (channelToInterpret.HasFlag(Channels.High))
            channelList.Add(Channels.High);
        if (channelToInterpret.HasFlag(Channels.Mid))
            channelList.Add(Channels.Mid);
        if (channelToInterpret.HasFlag(Channels.Low))
            channelList.Add(Channels.Low);

        return channelList;
    }
}

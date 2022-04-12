using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                                AddElementalStacks(effect, MechComponent.Head, destinationMech);
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

    private void Start()
    {
        CardPlayManager.OnCombatComplete += IncrementEffectsAtTurnEnd;

        playerFighterEffectObject = new FighterEffectObject();
        opponentFighterEffectObject = new FighterEffectObject();
    }

    private void OnDestroy()
    {
        CardPlayManager.OnCombatComplete -= IncrementEffectsAtTurnEnd;
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
        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    if (playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Punch] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Punch])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Kick:
                    if (playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Kick] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Kick])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Special:
                    if (playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Special] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.CardCategoryDamageBonus[CardCategory.Special])
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
                    if (opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Punch] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Punch])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Kick:
                    if (opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Kick] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Kick])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case CardCategory.Special:
                    if (opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Special] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.CardCategoryDamageBonus[CardCategory.Special])
                            damageToReturn += effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
    }

    private int GetCardChannelDamageBonus(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (attack.CardChannel)
            {
                case Channels.High:
                    if (playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case Channels.Mid:
                    if (playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case Channels.Low:
                    if (playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (attack.CardChannel)
            {
                case Channels.High:
                    if (opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case Channels.Mid:
                    if (opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
                case Channels.Low:
                    if (opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageBonus[attack.CardChannel])
                            damageToReturn += effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
    }

    private int GetKeyWordDamageBonus(CardChannelPairObject attack, ref int damageToReturn, CharacterSelect defensiveCharacter)
    {
        CardKeyWord keyWord = CardKeyWord.None;

        if (attack.CardData.CardEffects.Select(x => x.EffectType).Contains(CardEffectTypes.KeyWordExecute))
        {
            foreach (SOCardEffectObject effect in attack.CardData.CardEffects)
                if (effect.EffectType == CardEffectTypes.KeyWordExecute)
                    keyWord = effect.CardKeyWord;

            if(defensiveCharacter == CharacterSelect.Opponent)
                if (playerFighterEffectObject.KeyWordDuration[keyWord] != null)
                    foreach (CardEffectObject effect in playerFighterEffectObject.KeyWordDuration[keyWord])
                        damageToReturn += effect.EffectMagnitude;
            else
                if (opponentFighterEffectObject.KeyWordDuration[keyWord] != null)
                    foreach (CardEffectObject effect in opponentFighterEffectObject.KeyWordDuration[keyWord])
                        damageToReturn += effect.EffectMagnitude;
        }

        return damageToReturn;
    }

    private int GetComponentDamageBonus(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        if(defensiveCharacter == CharacterSelect.Opponent)
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
        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (attack.CardChannel)
            {
                case Channels.High:
                    if (opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
                case Channels.Mid:
                    if (opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
                case Channels.Low:
                    if (opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in opponentFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (attack.CardChannel)
            {
                case Channels.High:
                    if (playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
                case Channels.Mid:
                    if (playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
                case Channels.Low:
                    if (playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel] != null)
                        foreach (CardEffectObject effect in playerFighterEffectObject.ChannelDamageReduction[attack.CardChannel])
                            damageToReturn -= effect.EffectMagnitude;
                    break;
            }

            return damageToReturn;
        }
    }
    
    private int GetDamageReducedByShield(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        int initialShield;

        if(defensiveCharacter == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelShields.TryGetValue(attack.CardChannel, out initialShield))
            {
                int shieldAmount = initialShield;

                shieldAmount -= damageToReturn;
                damageToReturn = Mathf.RoundToInt(Mathf.Clamp(damageToReturn - initialShield, 0, Mathf.Infinity));


                if (shieldAmount <= 0)
                    opponentFighterEffectObject.ChannelShields.Remove(attack.CardChannel);
                else
                    opponentFighterEffectObject.ChannelShields[attack.CardChannel] = shieldAmount;

            }

            return damageToReturn;
        }
        else
        {
            if (playerFighterEffectObject.ChannelShields.TryGetValue(attack.CardChannel, out initialShield))
            {
                int shieldAmount = initialShield;

                shieldAmount -= damageToReturn;
                damageToReturn = Mathf.RoundToInt(Mathf.Clamp(damageToReturn - initialShield, 0, Mathf.Infinity));


                if (shieldAmount <= 0)
                    playerFighterEffectObject.ChannelShields.Remove(attack.CardChannel);
                else
                    playerFighterEffectObject.ChannelShields[attack.CardChannel] = shieldAmount;

               
            }

            return damageToReturn;
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
        List<CardEffectObject> previousBoostList = new List<CardEffectObject>();
        CardEffectObject newBoost = new CardEffectObject(effect);

        if (characterBoosting == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] != null)
            {
                foreach (CardEffectObject previousEffect in playerFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost])
                    previousBoostList.Add(previousEffect);

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
            if (opponentFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost] != null)
            {
                foreach (CardEffectObject previousEffect in opponentFighterEffectObject.CardCategoryDamageBonus[effect.CardTypeToBoost])
                    previousBoostList.Add(previousEffect);

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
            if (playerFighterEffectObject.ChannelDamageBonus[channel] != null)
            {
                foreach (CardEffectObject previousEffect in playerFighterEffectObject.ChannelDamageBonus[channel])
                    previousBoostList.Add(previousEffect);

                previousBoostList.Add(newBoost);
                playerFighterEffectObject.ChannelDamageBonus[channel] = previousBoostList;
            }
            else
            {
                List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                newBoostList.Add(newBoost);
                playerFighterEffectObject.ChannelDamageBonus.Add(channel, newBoostList);
            }
        }

        if (characterBoosting == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelDamageBonus[channel] != null)
            {
                foreach (CardEffectObject previousEffect in opponentFighterEffectObject.ChannelDamageBonus[channel])
                    previousBoostList.Add(previousEffect);

                previousBoostList.Add(newBoost);
                opponentFighterEffectObject.ChannelDamageBonus[channel] = previousBoostList;
            }
            else
            {
                List<CardEffectObject> newBoostList = new List<CardEffectObject>();
                newBoostList.Add(newBoost);
                opponentFighterEffectObject.ChannelDamageBonus.Add(channel, newBoostList);
            }
        }
    }

    private void ReduceChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterReducing)
    {
        List<CardEffectObject> previousReductionList = new List<CardEffectObject>();
        CardEffectObject newReduction = new CardEffectObject(effect);

        if (characterReducing == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.ChannelDamageReduction[channel] != null)
            {
                foreach (CardEffectObject previousEffect in playerFighterEffectObject.ChannelDamageReduction[channel])
                    previousReductionList.Add(previousEffect);

                previousReductionList.Add(newReduction);
                playerFighterEffectObject.ChannelDamageReduction[channel] = previousReductionList;
            }
            else
            {
                List<CardEffectObject> newReductionList = new List<CardEffectObject>();
                newReductionList.Add(newReduction);
                playerFighterEffectObject.ChannelDamageReduction.Add(channel, newReductionList);
            }
        }

        if (characterReducing == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelDamageReduction[channel] != null)
            {
                foreach (CardEffectObject previousEffect in opponentFighterEffectObject.ChannelDamageReduction[channel])
                    previousReductionList.Add(previousEffect);

                previousReductionList.Add(newReduction);
                opponentFighterEffectObject.ChannelDamageReduction[channel] = previousReductionList;
            }
            else
            {
                List<CardEffectObject> newReductionList = new List<CardEffectObject>();
                newReductionList.Add(newReduction);
                opponentFighterEffectObject.ChannelDamageReduction.Add(channel, newReductionList);
            }
        }
    }

    private void KeyWordInitialize(SOCardEffectObject effect, Channels channel, CharacterSelect characterPriming)
    {
        List<CardEffectObject> currentKeyWordEffectList = new List<CardEffectObject>();
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (characterPriming == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.KeyWordDuration[effect.CardKeyWord] != null)
            {
                foreach (CardEffectObject previousEffect in opponentFighterEffectObject.ChannelDamageReduction[channel])
                    currentKeyWordEffectList.Add(previousEffect);

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
            if (opponentFighterEffectObject.KeyWordDuration[effect.CardKeyWord] != null)
            {
                foreach (CardEffectObject previousEffect in opponentFighterEffectObject.ChannelDamageReduction[channel])
                    currentKeyWordEffectList.Add(previousEffect);

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

    private void IncrementEffectsAtTurnEnd()
    {
        //Iterate through elements
        //Clean all other effects out
    }
}

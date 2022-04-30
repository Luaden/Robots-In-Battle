using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectController
{
    private FighterEffectObject playerFighterEffectObject;
    private FighterEffectObject opponentFighterEffectObject;

    public FighterEffectObject PlayerEffects { get => playerFighterEffectObject; }
    public FighterEffectObject OpponentEffects { get => opponentFighterEffectObject; }

    private Queue<CardCharacterPairObject> effectQueue;

    public EffectController()
    {
        playerFighterEffectObject = new FighterEffectObject();
        opponentFighterEffectObject = new FighterEffectObject();
        effectQueue = new Queue<CardCharacterPairObject>();

        CombatManager.OnDestroyScene += DisableEffectListeners;
        CombatAnimationManager.OnEndedAnimation += EnableEffects;
        CardPlayManager.OnCombatComplete += IncrementEffectsAtTurnEnd;
    }

    public void AddToEffectQueue(CardChannelPairObject cardChannelPairObject, CharacterSelect destinationMech)
    {
        CardCharacterPairObject newEffect = new CardCharacterPairObject();
        newEffect.cardChannelPair = cardChannelPairObject;
        newEffect.character = destinationMech;

        effectQueue.Enqueue(newEffect);
    }

    public int GetMechDamageWithAndConsumeModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        int damageToReturn = attack.CardData.BaseDamage;

        //Get Damage Boosts and Modifiers
        damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
        //damageToReturn = GetAndConsumeKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);
        damageToReturn = GetComponentDamageBonus(attack, damageToReturn, defensiveCharacter);

        //Get Damage Reductions and Modifiers
        damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);

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

    private void UpdateFighterBuffs()
    {
        //This currently doesn't account for decreases in channel damage.
        CombatManager.instance.BuffUIManager.UpdateChannelDamageBuffs(CharacterSelect.Player, playerFighterEffectObject.ChannelDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateChannelElementStacks(CharacterSelect.Player, playerFighterEffectObject.IceAcidStacks);
        CombatManager.instance.BuffUIManager.UpdateChannelShields(CharacterSelect.Player, playerFighterEffectObject.ChannelShields);
        CombatManager.instance.BuffUIManager.UpdateChannelShieldsFalloff(CharacterSelect.Player, playerFighterEffectObject.ChannelShieldsFalloff);
        CombatManager.instance.BuffUIManager.UpdateGlobalElementStacks(CharacterSelect.Player, playerFighterEffectObject.FirePlasmaStacks);
        CombatManager.instance.BuffUIManager.UpdateGlobalCategoryDamageBuffs(CharacterSelect.Player, playerFighterEffectObject.CardCategoryDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateGlobalKeyWordDamageBuffs(CharacterSelect.Player, playerFighterEffectObject.KeyWordDuration);

        CombatManager.instance.BuffUIManager.UpdateChannelDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateChannelElementStacks(CharacterSelect.Opponent, opponentFighterEffectObject.IceAcidStacks);
        CombatManager.instance.BuffUIManager.UpdateChannelShields(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelShields);
        CombatManager.instance.BuffUIManager.UpdateChannelShieldsFalloff(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelShieldsFalloff);
        CombatManager.instance.BuffUIManager.UpdateGlobalElementStacks(CharacterSelect.Opponent, opponentFighterEffectObject.FirePlasmaStacks);
        CombatManager.instance.BuffUIManager.UpdateGlobalCategoryDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.CardCategoryDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateGlobalKeyWordDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.KeyWordDuration);
    }

    public int GetMechDamageWithModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter)
    {
        int damageToReturn = attack.CardData.BaseDamage;

        //Get Damage Boosts and Modifiers
        damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
        //damageToReturn = GetAndConsumeKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);
        damageToReturn = GetComponentDamageBonus(attack, damageToReturn, defensiveCharacter);

        //Get Damage Reductions and Modifiers
        damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);

        return damageToReturn;
    }

    public int GetAndConsumeFlurryBonus(CharacterSelect characterToCheck)
    {
        int flurryBonus = 0;

        if (characterToCheck == CharacterSelect.Player)
            if (playerFighterEffectObject.KeyWordDuration.ContainsKey(CardKeyWord.Flurry))
            {
                List<CardEffectObject> flurryBonuses = new List<CardEffectObject>();
                List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();

                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(CardKeyWord.Flurry, out flurryBonuses))
                {
                    foreach (CardEffectObject effect in flurryBonuses)
                    {
                        flurryBonus += effect.EffectMagnitude;
                        removalKeyWordEffects.Add(effect);
                    }

                    foreach (CardEffectObject effect in removalKeyWordEffects)
                        if (flurryBonuses.Contains(effect))
                            flurryBonuses.Remove(effect);
                }

                playerFighterEffectObject.KeyWordDuration[CardKeyWord.Flurry] = flurryBonuses;

                if (playerFighterEffectObject.KeyWordDuration[CardKeyWord.Flurry].Count == 0)
                    playerFighterEffectObject.KeyWordDuration.Remove(CardKeyWord.Flurry);

            }
            else
            if (opponentFighterEffectObject.KeyWordDuration.ContainsKey(CardKeyWord.Flurry))
            {
                List<CardEffectObject> flurryBonuses = new List<CardEffectObject>();
                List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();

                if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(CardKeyWord.Flurry, out flurryBonuses))
                {
                    if (flurryBonuses == null)
                        return 0;

                    foreach (CardEffectObject effect in flurryBonuses)
                    {
                        flurryBonus += effect.EffectMagnitude;
                        removalKeyWordEffects.Add(effect);
                    }

                    foreach (CardEffectObject effect in removalKeyWordEffects)
                        if (flurryBonuses.Contains(effect))
                            flurryBonuses.Remove(effect);
                }

                opponentFighterEffectObject.KeyWordDuration[CardKeyWord.Flurry] = flurryBonuses;

                if (opponentFighterEffectObject.KeyWordDuration[CardKeyWord.Flurry].Count == 0)
                    opponentFighterEffectObject.KeyWordDuration.Remove(CardKeyWord.Flurry);

            }

        return flurryBonus;
    }

    public int GetFlurryBonus(CharacterSelect characterToCheck)
    {
        int flurryBonus = 0;

        if (characterToCheck == CharacterSelect.Player)
            if (playerFighterEffectObject.KeyWordDuration.ContainsKey(CardKeyWord.Flurry))
            {
                List<CardEffectObject> flurryBonuses = new List<CardEffectObject>();
                List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();

                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(CardKeyWord.Flurry, out flurryBonuses))
                {
                    foreach (CardEffectObject effect in flurryBonuses)
                        flurryBonus += effect.EffectMagnitude;
                }
            }

        if (characterToCheck == CharacterSelect.Opponent)
            if (opponentFighterEffectObject.KeyWordDuration.ContainsKey(CardKeyWord.Flurry))
            {
                List<CardEffectObject> flurryBonuses = new List<CardEffectObject>();
                List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();

                if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(CardKeyWord.Flurry, out flurryBonuses))
                {
                    foreach (CardEffectObject effect in flurryBonuses)
                        flurryBonus += effect.EffectMagnitude;
                }
            }

        return flurryBonus;
    }
    
    private void DisableEffectListeners()
    {
        CombatAnimationManager.OnEndedAnimation -= EnableEffects;
        CombatAnimationManager.OnEndedAnimation -= UpdateFighterBuffs;
        CardPlayManager.OnCombatComplete -= IncrementEffectsAtTurnEnd;
        CombatManager.OnDestroyScene -= DisableEffectListeners;
    }

    private void EnableEffects()
    {
        if (effectQueue.Count == 0)
            return;

        CardCharacterPairObject currentEffect = effectQueue.Dequeue();
        CardChannelPairObject cardChannelPair = currentEffect.cardChannelPair;
        CharacterSelect destinationMech = currentEffect.character;

        if (cardChannelPair == null || cardChannelPair.CardData == null)
            return;

        if (cardChannelPair.CardData.CardEffects == null)
            return;

        int repeatPlay = 1;

        foreach (SOCardEffectObject effect in cardChannelPair.CardData.CardEffects)
        {
            if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                repeatPlay += effect.EffectMagnitude;

            if (effect.EffectType == CardEffectTypes.KeyWord && effect.CardKeyWord == CardKeyWord.Flurry)
                repeatPlay += GetFlurryBonus(destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
        }

        for (int i = 0; i < repeatPlay; i++)
        {
            foreach (SOCardEffectObject effect in cardChannelPair.CardData.CardEffects)
            {
                switch (effect.EffectType)
                {
                    case CardEffectTypes.PlayMultipleTimes:
                        break;

                    case CardEffectTypes.AdditionalElementStacks:
                        //Adding bonus element stacks from card effects
                        switch (cardChannelPair.CardData.CardCategory)
                        {
                            case CardCategory.Punch:
                                AddElementalStacks(effect, cardChannelPair.CardChannel, MechComponent.Arms, destinationMech);
                                break;
                            case CardCategory.Kick:
                                AddElementalStacks(effect, cardChannelPair.CardChannel, MechComponent.Legs, destinationMech);
                                break;
                            case CardCategory.Special:
                                AddElementalStacks(effect, cardChannelPair.CardChannel, MechComponent.Head, destinationMech);
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

                    case CardEffectTypes.ReduceOutgoingChannelDamage:
                        ReduceChannelDamage(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels, destinationMech);
                        break;

                    case CardEffectTypes.KeyWord:
                        GainKeyWord(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;
                }

                //Adding stand alone element stacks from components.
                switch (cardChannelPair.CardData.CardCategory)
                {
                    case CardCategory.Punch:
                        AddComponentElementStacks(cardChannelPair, MechComponent.Arms, destinationMech);
                        break;
                    case CardCategory.Kick:
                        AddComponentElementStacks(cardChannelPair, MechComponent.Legs, destinationMech);
                        break;
                    case CardCategory.Special:
                        AddComponentElementStacks(cardChannelPair, MechComponent.Head, destinationMech);
                        break;
                    case CardCategory.None:
                        break;
                    case CardCategory.Guard:
                        break;
                    case CardCategory.Counter:
                        break;
                    case CardCategory.Offensive:
                        break;
                    case CardCategory.Defensive:
                        break;
                    case CardCategory.All:
                        break;
                }
            }
        }

        UpdateFighterBuffs();
    }

    private void IncrementEffectsAtTurnEnd()
    {
        playerFighterEffectObject.IncrementFighterEffects();
        opponentFighterEffectObject.IncrementFighterEffects();
    }

    private void AddElementalStacks(SOCardEffectObject effect, Channels channel, MechComponent component, CharacterSelect characterAdding)
    {
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
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechArms.ComponentElement;
                    
                    if (currentElement == ElementType.None)
                        return;

                    if(currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (playerFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            playerFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            playerFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();
                        
                        if(playerFighterEffectObject.IceAcidStacks.TryGetValue(channel, out currentStackObject))
                        {
                            foreach(ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            playerFighterEffectObject.IceAcidStacks.Add(channel, currentStackObject);
                        }
                    }
                }

                if (characterAdding == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechArms.ComponentElement;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (opponentFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            opponentFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (playerFighterEffectObject.IceAcidStacks.TryGetValue(channel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            playerFighterEffectObject.IceAcidStacks.Add(channel, currentStackObject);
                        }
                    }
                }
                break;

            case MechComponent.Legs:
                if (characterAdding == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechLegs.ComponentElement;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (playerFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            playerFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            playerFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (playerFighterEffectObject.IceAcidStacks.TryGetValue(channel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            playerFighterEffectObject.IceAcidStacks.Add(channel, currentStackObject);
                        }
                    }
                }

                if (characterAdding == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechLegs.ComponentElement;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (opponentFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            opponentFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(channel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            opponentFighterEffectObject.IceAcidStacks.Add(channel, currentStackObject);
                        }
                    }
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

    private int GetAndConsumeKeyWordDamageBonus(CardChannelPairObject attack, ref int damageToReturn, CharacterSelect defensiveCharacter)
    {
        CardKeyWord keyWord = CardKeyWord.None;
        List<CardEffectObject> previousKeyWordEffects = new List<CardEffectObject>();
        List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();


        if (attack.CardData.CardEffects.Select(x => x.EffectType).Contains(CardEffectTypes.KeyWord))
        {
            foreach (SOCardEffectObject effect in attack.CardData.CardEffects)
                if (effect.EffectType == CardEffectTypes.KeyWord)
                    keyWord = effect.CardKeyWord;

            if(defensiveCharacter == CharacterSelect.Opponent)
            {
                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                {
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                    {
                        damageToReturn += effect.EffectMagnitude;
                        removalKeyWordEffects.Add(effect);
                    }

                    foreach (CardEffectObject effect in removalKeyWordEffects)
                        if (previousKeyWordEffects.Contains(effect))
                            previousKeyWordEffects.Remove(effect);

                    playerFighterEffectObject.KeyWordDuration[keyWord] = previousKeyWordEffects;
                }
            }
            else
            {
                if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                {
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                    {
                        damageToReturn += effect.EffectMagnitude;
                        removalKeyWordEffects.Add(effect);
                    }

                    foreach (CardEffectObject effect in removalKeyWordEffects)
                        if (previousKeyWordEffects.Contains(effect))
                            previousKeyWordEffects.Remove(effect);

                    opponentFighterEffectObject.KeyWordDuration[keyWord] = previousKeyWordEffects;
                }
            }
        }

        return damageToReturn;
    }

    private int GetKeyWordDamageBonus(CardChannelPairObject attack, ref int damageToReturn, CharacterSelect defensiveCharacter)
    {
        CardKeyWord keyWord = CardKeyWord.None;
        List<CardEffectObject> previousKeyWordEffects = new List<CardEffectObject>();


        if (attack.CardData.CardEffects.Select(x => x.EffectType).Contains(CardEffectTypes.KeyWord))
        {
            foreach (SOCardEffectObject effect in attack.CardData.CardEffects)
                if (effect.EffectType == CardEffectTypes.KeyWord)
                    keyWord = effect.CardKeyWord;

            if (defensiveCharacter == CharacterSelect.Opponent)
            {
                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                        damageToReturn += effect.EffectMagnitude;
            }
            else
            {
                if (opponentFighterEffectObject.KeyWordDuration.TryGetValue(keyWord, out previousKeyWordEffects))
                    foreach (CardEffectObject effect in previousKeyWordEffects)
                        damageToReturn += effect.EffectMagnitude;            }
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
                    damageToReturn = Mathf.RoundToInt(damageToReturn *CombatManager.instance.PlayerFighter.FighterMech.MechArms.CDMFromComponent);
                    break;
                case CardCategory.Kick:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.PlayerFighter.FighterMech.MechLegs.CDMFromComponent);
                    break;
                case CardCategory.Special:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.PlayerFighter.FighterMech.MechTorso.CDMFromComponent);
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (attack.CardData.CardCategory)
            {
                case CardCategory.Punch:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.OpponentFighter.FighterMech.MechArms.CDMFromComponent);
                    break;
                case CardCategory.Kick:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.OpponentFighter.FighterMech.MechLegs.CDMFromComponent);
                    break;
                case CardCategory.Special:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.OpponentFighter.FighterMech.MechTorso.CDMFromComponent);
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

    private void ReduceChannelDamage(SOCardEffectObject effect, Channels channel, CharacterSelect characterDamageBeingReduced)
    {
        List<CardEffectObject> previousReductionList = new List<CardEffectObject>();
        CardEffectObject newReduction = new CardEffectObject(effect);

        if (characterDamageBeingReduced == CharacterSelect.Player)
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

        if (characterDamageBeingReduced == CharacterSelect.Opponent)
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

    private void GainKeyWord(SOCardEffectObject effect, Channels channel, CharacterSelect characterPriming)
    {
        List<CardEffectObject> currentKeyWordEffectList = new List<CardEffectObject>();
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (effect.EffectMagnitude == 0)
            return;

        if (characterPriming == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.KeyWordDuration.TryGetValue(effect.CardKeyWord, out currentKeyWordEffectList))
            {
                Debug.Log("Player Keyword found.");

                currentKeyWordEffectList.Add(newKeyWordEffect);
                playerFighterEffectObject.KeyWordDuration[effect.CardKeyWord] = currentKeyWordEffectList;
            }
            else
            {
                List<CardEffectObject> newKeyWordList = new List<CardEffectObject>();
                newKeyWordList.Add(newKeyWordEffect);
                playerFighterEffectObject.KeyWordDuration.Add(effect.CardKeyWord, newKeyWordList);

                Debug.Log("Added player Keyword.");
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

    private void AddComponentElementStacks(CardChannelPairObject cardChannelPair, MechComponent component, CharacterSelect destinationMech)
    {
        int newStacks = 1;

        switch (component)
        {
            case MechComponent.Head:
                if (destinationMech == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechHead.ComponentElement;
                    newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechArms.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (opponentFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            opponentFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            opponentFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }

                if (destinationMech == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechHead.ComponentElement;
                    newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechArms.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (playerFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            playerFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (playerFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            playerFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }
                break;

            case MechComponent.Torso:
                break;

            case MechComponent.Arms:
                if (destinationMech == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechArms.ComponentElement;
                    newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechArms.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (playerFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            playerFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            playerFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (playerFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);
                            playerFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }

                if (destinationMech == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechArms.ComponentElement;
                    newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechArms.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (opponentFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            opponentFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            opponentFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }
                break;

            case MechComponent.Legs:
                if (destinationMech == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechLegs.ComponentElement;
                    newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (playerFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            playerFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            playerFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (playerFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);
                            playerFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }

                if (destinationMech == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechLegs.ComponentElement;
                    newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechLegs.ExtraElementStacks;

                    if (currentElement == ElementType.None)
                        return;

                    if (currentElement == ElementType.Fire || currentElement == ElementType.Plasma)
                    {
                        int currentStacks;

                        if (opponentFighterEffectObject.FirePlasmaStacks.TryGetValue(currentElement, out currentStacks))
                        {
                            newStacks += currentStacks;
                            opponentFighterEffectObject.FirePlasmaStacks[currentElement] = newStacks;
                        }
                        else
                            opponentFighterEffectObject.FirePlasmaStacks.Add(currentElement, newStacks);
                    }

                    if (currentElement == ElementType.Ice || currentElement == ElementType.Acid)
                    {
                        List<ElementStackObject> currentStackObject = new List<ElementStackObject>();

                        if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(cardChannelPair.CardChannel, out currentStackObject))
                        {
                            foreach (ElementStackObject elementStack in currentStackObject)
                                if (elementStack.ElementType == currentElement)
                                    elementStack.ElementStacks += newStacks;
                        }
                        else
                        {
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);
                            opponentFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }
                break;
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
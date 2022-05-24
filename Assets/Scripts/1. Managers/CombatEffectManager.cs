using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatEffectManager : MonoBehaviour
{
    private FighterEffectObject playerFighterEffectObject;
    private FighterEffectObject opponentFighterEffectObject;

    public FighterEffectObject PlayerEffects { get => playerFighterEffectObject; }
    public FighterEffectObject OpponentEffects { get => opponentFighterEffectObject; }


    public void EnableCombatEffects(CardCharacterPairObject currentEffect)
    {
        if (currentEffect == null)
            return;

        CardChannelPairObject cardChannelPair = currentEffect.cardChannelPair;
        CharacterSelect destinationMech = currentEffect.character;

        if (cardChannelPair == null || cardChannelPair.CardData == null)
            return;

        if (cardChannelPair.CardData.CardEffects == null)
            return;

        for(int i = 0; i < currentEffect.repeatEffect; i++)
        {
            foreach (SOCardEffectObject effect in cardChannelPair.CardData.CardEffects)
            {
                switch (effect.EffectType)
                {
                    case CardEffectTypes.None:
                        break;
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
                                AddElementalStacks(effect, cardChannelPair.CardChannel, MechComponent.Torso, destinationMech);
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
                        GainKeyWord(effect, destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.GainShieldWithFalloff:
                        GainShieldsWithFalloff(effect, cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels,
                            destinationMech == CharacterSelect.Opponent ? CharacterSelect.Player : CharacterSelect.Opponent);
                        break;

                    case CardEffectTypes.EnergyDestroy:
                        DestroyEnergy(effect, destinationMech);
                        break;

                    case CardEffectTypes.ShieldDestroy:
                        DestroyShields(cardChannelPair.CardData.AffectedChannels == AffectedChannels.SelectedChannel ?
                            cardChannelPair.CardChannel : cardChannelPair.CardData.PossibleChannels, destinationMech == CharacterSelect.Opponent ?
                            CharacterSelect.Player : CharacterSelect.Opponent);
                        break;
                }
            }

            //Adding stand alone element stacks from components.
            switch (cardChannelPair.CardData.CardCategory)
            {
                case CardCategory.None:
                    break;
                case CardCategory.All:
                    AddComponentElementStacks(cardChannelPair, MechComponent.Arms, destinationMech);
                    AddComponentElementStacks(cardChannelPair, MechComponent.Legs, destinationMech);
                    AddComponentElementStacks(cardChannelPair, MechComponent.Torso, destinationMech);
                    break;
                case CardCategory.Punch:
                    AddComponentElementStacks(cardChannelPair, MechComponent.Arms, destinationMech);
                    break;
                case CardCategory.Kick:
                    AddComponentElementStacks(cardChannelPair, MechComponent.Legs, destinationMech);
                    break;
                case CardCategory.Special:
                    AddComponentElementStacks(cardChannelPair, MechComponent.Torso, destinationMech);
                    break;
            }
        }

        UpdateFighterBuffs();
    }

    public void EnablePilotEffects(CharacterSelect character, ActiveEffects effect)
    {
        switch (effect)
        {
            case ActiveEffects.None:
                break;
            case ActiveEffects.Jazzersize:
                GainPilotEffect(effect, character);
                break;
        }
    }

    public int GetDamageWithAndConsumeModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter, bool counter, bool guard)
    {
        int mechDamageToReturn = attack.CardData.BaseDamage;

        if (guard)
            mechDamageToReturn = Mathf.RoundToInt(mechDamageToReturn * CombatManager.instance.GuardDamageMultiplier);
        if (counter)
            mechDamageToReturn = Mathf.RoundToInt(mechDamageToReturn * CombatManager.instance.CounterDamageMultiplier);

        //Get Damage Boosts and Modifiers
        mechDamageToReturn = GetCardCategoryDamageBonus(attack, mechDamageToReturn, defensiveCharacter);
        mechDamageToReturn = GetCardChannelDamageBonus(attack, mechDamageToReturn, defensiveCharacter);
        mechDamageToReturn = GetJazzersizeBonusDamage(mechDamageToReturn, defensiveCharacter);
        //damageToReturn = GetAndConsumeKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);

        //Get Damage Reductions and Modifiers
        mechDamageToReturn = GetCardChannelDamageReduction(attack, mechDamageToReturn, defensiveCharacter);
        mechDamageToReturn = GetDamageAndReduceShield(attack, mechDamageToReturn, defensiveCharacter);

        return mechDamageToReturn;
    }

    public int GetDamageWithModifiers(CardChannelPairObject attack, CharacterSelect defensiveCharacter, bool counter, bool guard)
    {
        int damageToReturn = attack.CardData.BaseDamage;

        if (guard)
            damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.GuardDamageMultiplier);
        if (counter)
            damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.CounterDamageMultiplier);

        //Get Damage Boosts and Modifiers
        damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetJazzersizeBonusDamage(damageToReturn, defensiveCharacter);
        //damageToReturn = GetAndConsumeKeyWordDamageBonus(attack, ref damageToReturn, defensiveCharacter);

        //Get Damage Reductions and Modifiers
        damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);

        return damageToReturn;
    }

    public Vector2Int GetDamageWithModifiersAndShield(CardChannelPairObject attack, CharacterSelect defensiveCharacter, bool counter, bool guard)
    {
        int damageToReturn = attack.CardData.BaseDamage;

        if (guard)
            damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.GuardDamageMultiplier);
        if (counter)
            damageToReturn = Mathf.RoundToInt(damageToReturn * CombatManager.instance.CounterDamageMultiplier);

        //Get Damage Boosts and Modifiers
        damageToReturn = GetCardCategoryDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetCardChannelDamageBonus(attack, damageToReturn, defensiveCharacter);
        damageToReturn = GetJazzersizeBonusDamage(damageToReturn, defensiveCharacter);

        damageToReturn = GetCardChannelDamageReduction(attack, damageToReturn, defensiveCharacter);
        int damageReducedByShield = GetDamageReducedByShield(attack, damageToReturn, defensiveCharacter);

        Vector2Int damageShieldPair = new Vector2Int(damageReducedByShield, damageToReturn - damageReducedByShield);

        return damageShieldPair;
    }

    public int GetComponentDamageWithModifiers(int attackDamage, Channels channel, CharacterSelect defensiveCharacter)
    {
        attackDamage = GetComponentDamageBonus(attackDamage, channel, defensiveCharacter);
        attackDamage = GetAcidDamageBonus(attackDamage, channel, defensiveCharacter);

        return attackDamage;
    }

    public void AddFlurryBonus(SOCardEffectObject effect, CharacterSelect characterToAdd)
    {
        if (effect.CardKeyWord != CardKeyWord.Flurry)
            return;

        List<CardEffectObject> currentKeyWordEffectList = new List<CardEffectObject>();
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (effect.EffectMagnitude == 0)
            return;

        if (characterToAdd == CharacterSelect.Player)
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

        if (characterToAdd == CharacterSelect.Opponent)
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

    public int GetAndConsumeFlurryBonus(CharacterSelect characterToCheck)
    {
        int flurryBonus = 0;

        if (characterToCheck == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.KeyWordDuration.ContainsKey(CardKeyWord.Flurry))
            {
                List<CardEffectObject> flurryBonuses = new List<CardEffectObject>();
                List<CardEffectObject> removalKeyWordEffects = new List<CardEffectObject>();

                if (playerFighterEffectObject.KeyWordDuration.TryGetValue(CardKeyWord.Flurry, out flurryBonuses))
                {
                    foreach (CardEffectObject effect in flurryBonuses)
                    {
                        Debug.Log("Found a player Flurry.");
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

            return flurryBonus;
        }
        else
        {
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
                        Debug.Log("Found an opponent Flurry.");

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

    public bool GetIceElementInChannel(Channels channel, CharacterSelect checkCharacter)
    {
        List<ElementStackObject> previousElementChannelEffects = new List<ElementStackObject>();

        if (checkCharacter == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(channel, out previousElementChannelEffects))
                foreach (ElementStackObject element in previousElementChannelEffects)
                    if (element.ElementType == ElementType.Ice)
                        return true;
            return false;
        }
        else
        {
            if (playerFighterEffectObject.IceAcidStacks.TryGetValue(channel, out previousElementChannelEffects))
                foreach (ElementStackObject element in previousElementChannelEffects)
                    if (element.ElementType == ElementType.Ice)
                        return true;
            return false;
        }
    }

    public int GetJazzersizeBonusDamage(int damageToReturn, CharacterSelect defensiveCharacter)
    {
        int currentActiveEffectMagnitude = 0;

        if (damageToReturn == 0)
            return damageToReturn;

        if (defensiveCharacter == CharacterSelect.Player)
        {
            if (opponentFighterEffectObject.PilotEffectDuration.TryGetValue(ActiveEffects.Jazzersize, out currentActiveEffectMagnitude))
            {
                if(CombatManager.instance.NarrateCombat)
                {
                    Debug.Log("Dealing " + damageToReturn + " base damage and " + currentActiveEffectMagnitude + " bonus damage.");
                }
                return damageToReturn + currentActiveEffectMagnitude;
            }
        }

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            if (playerFighterEffectObject.PilotEffectDuration.TryGetValue(ActiveEffects.Jazzersize, out currentActiveEffectMagnitude))
            {
                if(CombatManager.instance.NarrateCombat)
                {
                    Debug.Log("Dealing " + damageToReturn + " base damage and " + currentActiveEffectMagnitude + " bonus damage.");
                }
                return damageToReturn + currentActiveEffectMagnitude;
            }
        }

        return damageToReturn;
    }

    private void Awake()
    {
        playerFighterEffectObject = new FighterEffectObject(CharacterSelect.Player);
        opponentFighterEffectObject = new FighterEffectObject(CharacterSelect.Opponent);

        CombatSequenceManager.OnCombatComplete += IncrementEffectsAtTurnEnd;
        CombatSequenceManager.OnCombatComplete += UpdateFighterBuffs;
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= UpdateFighterBuffs;
        CombatSequenceManager.OnCombatComplete -= IncrementEffectsAtTurnEnd;
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
        CombatManager.instance.BuffUIManager.UpdatePilotEffectBuffs(CharacterSelect.Player, playerFighterEffectObject.PilotEffectDuration);

        CombatManager.instance.BuffUIManager.UpdateChannelDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateChannelElementStacks(CharacterSelect.Opponent, opponentFighterEffectObject.IceAcidStacks);
        CombatManager.instance.BuffUIManager.UpdateChannelShields(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelShields);
        CombatManager.instance.BuffUIManager.UpdateChannelShieldsFalloff(CharacterSelect.Opponent, opponentFighterEffectObject.ChannelShieldsFalloff);
        CombatManager.instance.BuffUIManager.UpdateGlobalElementStacks(CharacterSelect.Opponent, opponentFighterEffectObject.FirePlasmaStacks);
        CombatManager.instance.BuffUIManager.UpdateGlobalCategoryDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.CardCategoryDamageBonus);
        CombatManager.instance.BuffUIManager.UpdateGlobalKeyWordDamageBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.KeyWordDuration);
        CombatManager.instance.BuffUIManager.UpdatePilotEffectBuffs(CharacterSelect.Opponent, opponentFighterEffectObject.PilotEffectDuration);
    }

    private void IncrementEffectsAtTurnEnd()
    {
        playerFighterEffectObject.IncrementFighterEffects();
        opponentFighterEffectObject.IncrementFighterEffects();
        UpdateFighterBuffs();
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

        if (defensiveCharacter == CharacterSelect.Opponent)
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
        int initialDamage = damageToReturn;

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
            {
                if (playerFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn += effect.EffectMagnitude;
            }

            if(CombatManager.instance.NarrateEffects)
            {
                if (damageToReturn > initialDamage)
                    Debug.Log("Player damage was modified due to channel damage bonus. Initial damage: " + initialDamage + ". Boosted damage: " + damageToReturn);
            }

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
            {
                if (opponentFighterEffectObject.ChannelDamageBonus.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn += effect.EffectMagnitude;

                if (CombatManager.instance.NarrateEffects)
                {
                    if (damageToReturn > initialDamage)
                        Debug.Log("Opponent damage was modified due to channel damage bonus. Initial damage: " + initialDamage + ". Boosted damage: " + damageToReturn);
                }
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

            if (defensiveCharacter == CharacterSelect.Opponent)
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
                        damageToReturn += effect.EffectMagnitude;
            }
        }

        return damageToReturn;
    }

    private int GetComponentDamageBonus(int damageToReturn, Channels channel, CharacterSelect defensiveCharacter)
    {
        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            switch (channel)
            {
                case Channels.High:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.PlayerFighter.FighterMech.MechHead.CDMFromComponent));
                    break;
                case Channels.Mid:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.PlayerFighter.FighterMech.MechLegs.CDMFromComponent));
                    break;
                case Channels.Low:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.PlayerFighter.FighterMech.MechTorso.CDMFromComponent));
                    break;
            }

            return damageToReturn;
        }
        else
        {
            switch (channel)
            {
                case Channels.High:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.OpponentFighter.FighterMech.MechHead.CDMFromComponent));
                    break;
                case Channels.Mid:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.OpponentFighter.FighterMech.MechLegs.CDMFromComponent));
                    break;
                case Channels.Low:
                    damageToReturn = Mathf.RoundToInt(damageToReturn * (1 + CombatManager.instance.OpponentFighter.FighterMech.MechTorso.CDMFromComponent));
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
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
            {
                if (opponentFighterEffectObject.ChannelDamageReduction.TryGetValue(channel, out previousChannelEffects))
                    foreach (CardEffectObject effect in previousChannelEffects)
                        damageToReturn -= effect.EffectMagnitude;
            }

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
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

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(channel, out initialShield))
                    damageToReturn = Mathf.Clamp(damageToReturn - initialShield, 0, int.MaxValue);

            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
                if (playerFighterEffectObject.ChannelShields.TryGetValue(channel, out initialShield))
                    damageToReturn = Mathf.Clamp(damageToReturn - initialShield, 0, int.MaxValue);

            return damageToReturn;
        }
    }

    private int GetDamageAndReduceShield(CardChannelPairObject attack, int damageToReturn, CharacterSelect defensiveCharacter)
    {
        int initialShield;

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
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

            UpdateFighterBuffs();
            return damageToReturn;
        }
        else
        {
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(attack.CardChannel))
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

            UpdateFighterBuffs();
            return damageToReturn;
        }
    }

    private void GainShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        int shieldAmount;

        if (characterGaining == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    opponentFighterEffectObject.ChannelShields[returnedChannel] =
                        opponentFighterEffectObject.ChannelShields[returnedChannel] + effect.EffectMagnitude;
                else
                    opponentFighterEffectObject.ChannelShields.Add(returnedChannel, effect.EffectMagnitude);
            }
        }

        UpdateFighterBuffs();
    }

    private void GainShieldsWithFalloff(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        List<ChannelShieldFalloffObject> oldShieldFalloffEffects = new List<ChannelShieldFalloffObject>();
        ChannelShieldFalloffObject newShieldFalloffEffect = new ChannelShieldFalloffObject();
        newShieldFalloffEffect.FalloffPerTurn = effect.EffectFallOffPerTurn;
        newShieldFalloffEffect.StartingShieldPerTurn = effect.EffectMagnitude;

        if (effect.EffectMagnitude == 0)
            return;

        if (characterGaining == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.ChannelShieldsFalloff.TryGetValue(channel, out oldShieldFalloffEffects))
            {
                oldShieldFalloffEffects.Add(newShieldFalloffEffect);
                playerFighterEffectObject.ChannelShieldsFalloff[channel] = oldShieldFalloffEffects;
            }
            else
            {
                List<ChannelShieldFalloffObject> newShieldFalloffEffectsList = new List<ChannelShieldFalloffObject>();
                newShieldFalloffEffectsList.Add(newShieldFalloffEffect);

                playerFighterEffectObject.ChannelShieldsFalloff.Add(channel, newShieldFalloffEffectsList);
            }
        }

        if (characterGaining == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.ChannelShieldsFalloff.TryGetValue(channel, out oldShieldFalloffEffects))
            {
                oldShieldFalloffEffects.Add(newShieldFalloffEffect);
                opponentFighterEffectObject.ChannelShieldsFalloff[channel] = oldShieldFalloffEffects;
            }
            else
            {
                List<ChannelShieldFalloffObject> newShieldFalloffEffectsList = new List<ChannelShieldFalloffObject>();
                newShieldFalloffEffectsList.Add(newShieldFalloffEffect);
                opponentFighterEffectObject.ChannelShieldsFalloff.Add(channel, newShieldFalloffEffectsList);
            }
        }
    }

    private void MultiplyShields(SOCardEffectObject effect, Channels channel, CharacterSelect characterGaining)
    {
        int shieldAmount;

        if (characterGaining == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelShields.TryGetValue(returnedChannel, out shieldAmount))
                    opponentFighterEffectObject.ChannelShields[returnedChannel] =
                        opponentFighterEffectObject.ChannelShields[returnedChannel] * effect.EffectMagnitude;
                else
                    return;
            }
        }
    }

    private void DestroyShields(Channels channel, CharacterSelect defensiveCharacter)
    {
        if (defensiveCharacter == CharacterSelect.Player)
        {
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
            {
                if (playerFighterEffectObject.ChannelShields.ContainsKey(returnedChannel))
                    playerFighterEffectObject.ChannelShields.Remove(returnedChannel);
            }
        }

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
            {
                if (opponentFighterEffectObject.ChannelShields.ContainsKey(returnedChannel))
                    opponentFighterEffectObject.ChannelShields.Remove(returnedChannel);
            }
        }

        UpdateFighterBuffs();
    }

    private void DestroyEnergy(SOCardEffectObject effect, CharacterSelect defensiveCharacter)
    {
        EnergyRemovalObject newEnergyToRemove = new EnergyRemovalObject();
        newEnergyToRemove.firstMech = defensiveCharacter;
        newEnergyToRemove.firstMechEnergyRemoval = effect.EffectMagnitude;

        CombatManager.instance.RemoveEnergyFromMechs(newEnergyToRemove);
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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
            foreach (Channels returnedChannel in CombatManager.instance.GetChannelListFromFlags(channel))
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

    private void GainKeyWord(SOCardEffectObject effect, CharacterSelect characterPriming)
    {
        List<CardEffectObject> currentKeyWordEffectList = new List<CardEffectObject>();
        CardEffectObject newKeyWordEffect = new CardEffectObject(effect);

        if (effect.CardKeyWord == CardKeyWord.Flurry)
            return;

        if (effect.EffectMagnitude == 0)
            return;

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

    private void GainPilotEffect(ActiveEffects effect, CharacterSelect characterPriming)
    {
        int currentActiveEffectMagnitude;

        if (characterPriming == CharacterSelect.Player)
        {
            if (playerFighterEffectObject.PilotEffectDuration.TryGetValue(effect, out currentActiveEffectMagnitude))
            {
                currentActiveEffectMagnitude++;
                playerFighterEffectObject.PilotEffectDuration[effect] = currentActiveEffectMagnitude;
            }
            else
            {
                playerFighterEffectObject.PilotEffectDuration.Add(effect, 1);
            }
        }

        if (characterPriming == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.PilotEffectDuration.TryGetValue(effect, out currentActiveEffectMagnitude))
            {
                currentActiveEffectMagnitude++;
                opponentFighterEffectObject.PilotEffectDuration[effect] = currentActiveEffectMagnitude;
            }
            else
            {
                opponentFighterEffectObject.PilotEffectDuration.Add(effect, 1);
            }
        }

        UpdateFighterBuffs();
    }

    private void AddComponentElementStacks(CardChannelPairObject cardChannelPair, MechComponent component, CharacterSelect destinationMech)
    {
        int newStacks = 1;

        switch (component)
        {
            case MechComponent.Torso:
                if (destinationMech == CharacterSelect.Opponent)
                {
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechTorso.ComponentElement;
                    newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechTorso.ExtraElementStacks;

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
                            if (currentStackObject.Select(x => x.ElementType).Contains(currentElement))
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
                                opponentFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
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
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechTorso.ComponentElement;
                    newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechTorso.ExtraElementStacks;

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
                            if(currentStackObject.Select(x => x.ElementType).Contains(currentElement))
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
                                playerFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);
                            playerFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }
                break;

            case MechComponent.Arms:
                if (destinationMech == CharacterSelect.Player)
                {
                    ElementType currentElement = CombatManager.instance.OpponentFighter.FighterMech.MechHead.ComponentElement;
                    newStacks += CombatManager.instance.OpponentFighter.FighterMech.MechHead.ExtraElementStacks;

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
                            if (currentStackObject.Select(x => x.ElementType).Contains(currentElement))
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
                                playerFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
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
                    ElementType currentElement = CombatManager.instance.PlayerFighter.FighterMech.MechHead.ComponentElement;
                    newStacks += CombatManager.instance.PlayerFighter.FighterMech.MechHead.ExtraElementStacks;

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
                            if (currentStackObject.Select(x => x.ElementType).Contains(currentElement))
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
                                opponentFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
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
                            if (currentStackObject.Select(x => x.ElementType).Contains(currentElement))
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
                                playerFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
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
                            if (currentStackObject.Select(x => x.ElementType).Contains(currentElement))
                            {
                                foreach (ElementStackObject elementStack in currentStackObject)
                                    if (elementStack.ElementType == currentElement)
                                    {
                                        elementStack.ElementStacks += newStacks;
                                    }
                            }
                            else
                            {
                                ElementStackObject newStackObject = new ElementStackObject();
                                newStackObject.ElementType = currentElement;
                                newStackObject.ElementStacks = newStacks;
                                currentStackObject.Add(newStackObject);
                                opponentFighterEffectObject.IceAcidStacks[cardChannelPair.CardChannel] = currentStackObject;
                            }
                        }
                        else
                        {
                            currentStackObject = new List<ElementStackObject>();
                            ElementStackObject newStackObject = new ElementStackObject();
                            newStackObject.ElementType = currentElement;
                            newStackObject.ElementStacks = newStacks;
                            currentStackObject.Add(newStackObject);

                            opponentFighterEffectObject.IceAcidStacks.Add(cardChannelPair.CardChannel, currentStackObject);
                        }
                    }
                }
                break;

            case MechComponent.None:
                break;
            case MechComponent.Head:
                break;
            case MechComponent.Back:
                break;
        }

        UpdateFighterBuffs();

    }

    private int GetAcidDamageBonus(int damageToDeal, Channels channel, CharacterSelect defensiveCharacter)
    {
        List<ElementStackObject> previousElementChannelEffects = new List<ElementStackObject>();

        if (defensiveCharacter == CharacterSelect.Opponent)
        {
            if (opponentFighterEffectObject.IceAcidStacks.TryGetValue(channel, out previousElementChannelEffects))
            {
                foreach (ElementStackObject element in previousElementChannelEffects)
                {
                    if (element.ElementType == ElementType.Acid)
                    {
                        if (CombatManager.instance.NarrateEffects)
                        {
                            Debug.Log(defensiveCharacter + "is recieving acid damage. Base Damage: " + damageToDeal + ", modified damage: "
                                + Mathf.RoundToInt(damageToDeal * CombatManager.instance.AcidComponentDamageMultiplier));
                        }
                        return Mathf.RoundToInt(damageToDeal * CombatManager.instance.AcidComponentDamageMultiplier);
                    }
                }
            }

            return damageToDeal;
        }
        else
        {
            if (playerFighterEffectObject.IceAcidStacks.TryGetValue(channel, out previousElementChannelEffects))
            {
                foreach (ElementStackObject element in previousElementChannelEffects)
                {
                    if (element.ElementType == ElementType.Acid)
                    {
                        if(CombatManager.instance.NarrateEffects)
                        {
                            Debug.Log(defensiveCharacter + "is recieving acid damage. Base Damage: " + damageToDeal + ", modified damage: " 
                                + Mathf.RoundToInt(damageToDeal * CombatManager.instance.AcidComponentDamageMultiplier));
                        }                        
                        return Mathf.RoundToInt(damageToDeal * CombatManager.instance.AcidComponentDamageMultiplier);
                    }
                }
            }

            return damageToDeal;
        }
    }
}
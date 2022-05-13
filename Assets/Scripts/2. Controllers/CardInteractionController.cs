using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardInteractionController
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private CombatQueueObject combatQueueObject;
    Queue<CardCharacterPairObject> preCombatEffectsQueue;
    Queue<CardCharacterPairObject> postCombatEffectsQueue;
    #region Debug
    private string combatLog;
    #endregion


    public void DetermineCardInteractions(AttackPlanObject newPlayerAttackPlan, AttackPlanObject newOpponentAttackPlan)
    {
        playerAttackPlan = new AttackPlanObject(newPlayerAttackPlan.cardChannelPairA, newPlayerAttackPlan.cardChannelPairB, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(newOpponentAttackPlan.cardChannelPairA, newOpponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, CharacterSelect.Player);
        preCombatEffectsQueue = new Queue<CardCharacterPairObject>();
        postCombatEffectsQueue = new Queue<CardCharacterPairObject>();

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null &&
            CardCategory.Defensive.HasFlag(opponentAttackPlan.cardChannelPairB.CardData.CardCategory) &&
            playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = playerAttackPlan.cardChannelPairA.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Player;
            combatQueueObject.cardBurnObject.secondCard = opponentAttackPlan.cardChannelPairB.CardData.CardUIController;

            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null &&
            CardCategory.Defensive.HasFlag(playerAttackPlan.cardChannelPairB.CardData.CardCategory) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = opponentAttackPlan.cardChannelPairA.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Opponent;
            combatQueueObject.cardBurnObject.secondCard = playerAttackPlan.cardChannelPairB.CardData.CardUIController;

            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = playerAttackPlan.cardChannelPairA.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Player;

            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, null, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = opponentAttackPlan.cardChannelPairA.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Opponent;

            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, null, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = playerAttackPlan.cardChannelPairB.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Player;

            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Player, null, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            combatQueueObject = new CombatQueueObject();

            combatQueueObject.cardBurnObject.firstCard = opponentAttackPlan.cardChannelPairB.CardData.CardUIController;
            combatQueueObject.cardBurnObject.firstCharacter = CharacterSelect.Opponent;

            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, null, CharacterSelect.Player);
        }

        ClearAllCards();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveMech, 
                                               CardChannelPairObject defensiveCard, CharacterSelect defensiveMech)
    {
        //Card is a Guard Type
        if (defensiveCard.CardData.CardCategory == CardCategory.Guard)
        {
            //Guard affects all possible channels.
            if (defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                //Guard affects all possible channels and is successful.
                if (defensiveCard.CardData.PossibleChannels.HasFlag(offensiveCard.CardChannel))
                {
                    //Calculate damage with guarded caveat.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
                }

                //Guard affects all possible channels, but was not successful.
                else
                {
                    //Calculate damage normally, enable offensive attack's effects
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                }
            }

            //Guard doesn't affect all possible channels, but is still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Calculate damage with guarded caveat.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
            }

            //Guard doesn't affect all channels and was not successful.
            else
            {
                //Calculate damage normally, enable offensive attack's effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
            }
        }

        //Card type is a counter
        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
        {
            //Counter affects all possible channels
            if (defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                //Counter was successful
                if (defensiveCard.CardData.PossibleChannels.HasFlag(offensiveCard.CardChannel))
                {
                    //Calculate damage with counter caveat.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true, false);
                }

                //Counter affects all possible channels but was not successful.
                else
                {
                    //Calculate damage normally and enable offensive effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                }
            }

            //Counter doesn't affect all channels but was still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Calculate damage with counter caveat.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true);
            }

            //Counter doesn't affect all channels and was not successful.
            else
            {
                //Calculate damage normally and enable offensive effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
            }
        }

        //Clear the attacks from further checks.
        ClearCardsAfterDefenses(offensiveMech);
    }

    private void ClearCardsAfterDefenses(CharacterSelect offensiveCharacter)
    {
        if (offensiveCharacter == CharacterSelect.Player)
        {
            playerAttackPlan.cardChannelPairA = null;
            opponentAttackPlan.cardChannelPairB = null;
        }
        else
        {
            playerAttackPlan.cardChannelPairB = null;
            opponentAttackPlan.cardChannelPairA = null;
        }
    }

    private void ClearAllCards()
    {
        playerAttackPlan = null;
        opponentAttackPlan = null;
    }

    private void CalculateMechDamage(CardChannelPairObject offensiveAttack, CharacterSelect offensiveMech, 
                                     CardChannelPairObject defensiveCard,CharacterSelect defensiveMech, 
                                        bool counterDamage = false, bool guardDamage = false)
    {
        Queue<AnimationQueueObject> newAnimations = new Queue<AnimationQueueObject>();
        Queue<DamageMechPairObject> newDamageQueue = new Queue<DamageMechPairObject>();


        EnergyRemovalObject newEnergyToRemove = QueueEnergyRemoval(offensiveAttack, offensiveMech, defensiveCard, defensiveMech);

        int repeatOffense = 1;
        int repeatDefense = 1;

        if (offensiveAttack != null && offensiveAttack.CardData != null && offensiveAttack.CardData.CardEffects != null)
        {
            foreach (SOCardEffectObject effect in offensiveAttack.CardData.CardEffects)
            {
                if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                    repeatOffense = effect.EffectMagnitude;

                if (effect.EffectType == CardEffectTypes.KeyWord && effect.CardKeyWord == CardKeyWord.Flurry)
                    repeatOffense += CombatManager.instance.CombatEffectManager.GetAndConsumeFlurryBonus(offensiveMech);

                CombatManager.instance.CombatEffectManager.AddFlurryBonus(effect, offensiveMech);
            }
        }
        
        if (defensiveCard != null && defensiveCard.CardData != null & defensiveCard.CardData.CardEffects != null)
        {
            foreach (SOCardEffectObject effect in defensiveCard.CardData.CardEffects)
            {
                if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                    repeatDefense = effect.EffectMagnitude;

                if (effect.EffectType == CardEffectTypes.KeyWord && effect.CardKeyWord == CardKeyWord.Flurry)
                    repeatDefense += CombatManager.instance.CombatEffectManager.GetAndConsumeFlurryBonus(defensiveMech);

                CombatManager.instance.CombatEffectManager.AddFlurryBonus(effect, defensiveMech);
            }
        }

        for (int i = 0; i < repeatOffense; i++)
        {
            if (counterDamage)
            {
                if (CombatManager.instance.NarrateCombat)
                {
                    combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                    combatLog += ("\n" + offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " +
                        CombatManager.instance.CombatEffectManager.GetDamageWithModifiers(offensiveAttack, defensiveMech, true, false) + " with modifiers.");
                    combatLog += ("\n" + offensiveMech + "'s attack is " + (i + 1) + " of " + repeatOffense + " total attacks. ");
                }

                newDamageQueue.Enqueue(new DamageMechPairObject(new CardCharacterPairObject(offensiveAttack, offensiveMech), 
                                                                new CardCharacterPairObject(defensiveCard, offensiveMech, repeatDefense), true, false));
                newAnimations.Enqueue(new AnimationQueueObject(offensiveMech, offensiveAttack.CardData.AnimationType, defensiveMech, defensiveCard.CardData.AnimationType));
                combatLog = string.Empty;
            }
            else if (guardDamage)
            {
                if (CombatManager.instance.NarrateCombat)
                {
                    combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                    combatLog += ("\n" + offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                        (CombatManager.instance.CombatEffectManager.GetDamageWithModifiers(offensiveAttack, defensiveMech, false, true)) + " with modifiers. ");
                    combatLog += ("\n" + offensiveMech + "'s attack is " + (i + 1) + " of " + repeatOffense + " total attacks. ");
                }

                newAnimations.Enqueue(new AnimationQueueObject(offensiveMech, offensiveAttack.CardData.AnimationType, defensiveMech, defensiveCard.CardData.AnimationType));
                newDamageQueue.Enqueue(new DamageMechPairObject(new CardCharacterPairObject(offensiveAttack, defensiveMech),
                                                                new CardCharacterPairObject(defensiveCard, offensiveMech, repeatDefense), false, true, combatLog));

                combatLog = string.Empty;
            }
            else
            {
                if (CombatManager.instance.NarrateCombat)
                {
                    combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " for " + offensiveAttack.CardData.BaseDamage + " damage." +
                    " \n This is changed to " + CombatManager.instance.CombatEffectManager.GetDamageWithModifiers(offensiveAttack, defensiveMech, false, false) + " damage with modifiers. ");
                    combatLog += ("\n" + offensiveMech + "'s attack is " + (i + 1) + " of " + repeatOffense + " total attacks. ");
                }

                newAnimations.Enqueue(new AnimationQueueObject(offensiveMech, offensiveAttack.CardData.AnimationType, defensiveMech, AnimationType.Damaged));

                if(defensiveCard == null)
                {
                    newDamageQueue.Enqueue(new DamageMechPairObject(new CardCharacterPairObject(offensiveAttack, defensiveMech, repeatDefense), null, false, false, combatLog));
                }
                else
                {
                    newDamageQueue.Enqueue(new DamageMechPairObject(new CardCharacterPairObject(offensiveAttack, defensiveMech, repeatDefense),
                                                                defensiveCard.CardData.CardCategory == CardCategory.Counter ?
                                                                null :
                                                                new CardCharacterPairObject(defensiveCard, offensiveMech, repeatDefense), false, false, combatLog));
                }
            }

            combatQueueObject.damageQueue = newDamageQueue;
            combatQueueObject.animationQueue = newAnimations;

            if (i == 0)
                combatQueueObject.energyRemovalObject = newEnergyToRemove;
            else
                combatQueueObject.energyRemovalObject = null;
            CombatManager.instance.CombatSequenceManager.AddCombatSequenceToQueue(combatQueueObject);
            combatLog = string.Empty;
        }
    }

    private EnergyRemovalObject QueueEnergyRemoval(CardChannelPairObject offensiveAttack, CharacterSelect offensiveMech, CardChannelPairObject defensiveCard, CharacterSelect defensiveMech)
    {
        EnergyRemovalObject newEnergyToRemove = new EnergyRemovalObject();
        newEnergyToRemove.firstMech = offensiveMech;

        if (offensiveAttack.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(offensiveAttack.CardData.PossibleChannels))
            {
                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(channel, offensiveMech))
                {
                    newEnergyToRemove.firstMechEnergyRemoval = Mathf.RoundToInt(offensiveAttack.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
                    break;
                }
                else
                    newEnergyToRemove.firstMechEnergyRemoval = offensiveAttack.CardData.EnergyCost;
            }
        else
        {
            if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(offensiveAttack.CardChannel, offensiveMech))
            {
                newEnergyToRemove.firstMechEnergyRemoval = Mathf.RoundToInt(offensiveAttack.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
            }
            else
                newEnergyToRemove.firstMechEnergyRemoval = offensiveAttack.CardData.EnergyCost;
        }



        if (defensiveCard != null && defensiveCard.CardData != null)
            if (defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                foreach (Channels channel in CombatManager.instance.GetChannelListFromFlags(defensiveCard.CardData.PossibleChannels))
                {
                    if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(channel, defensiveMech))
                    {
                        newEnergyToRemove.secondMechEnergyRemoval = Mathf.RoundToInt(defensiveCard.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
                        break;
                    }
                    else
                        newEnergyToRemove.secondMechEnergyRemoval = defensiveCard.CardData.EnergyCost;
                }
            else
            {
                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(defensiveCard.CardChannel, defensiveMech))
                    newEnergyToRemove.secondMechEnergyRemoval = Mathf.RoundToInt(defensiveCard.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
                else
                    newEnergyToRemove.secondMechEnergyRemoval = defensiveCard.CardData.EnergyCost;
            }

        return newEnergyToRemove;
    }
}

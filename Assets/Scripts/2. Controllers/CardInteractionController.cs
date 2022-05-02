using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardInteractionController
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private Queue<DamageMechPairObject> damageQueue;

    #region Debug
    private string combatLog;
    #endregion

    public CardInteractionController()
    {
        damageQueue = new Queue<DamageMechPairObject>();
        CombatAnimationManager.OnEndedAnimation += DealDamage;
        CombatManager.OnDestroyScene += DisableDamageListeners;
    }

    public void DetermineCardInteractions(AttackPlanObject newPlayerAttackPlan, AttackPlanObject newOpponentAttackPlan)
    {
        playerAttackPlan = new AttackPlanObject(newPlayerAttackPlan.cardChannelPairA, newPlayerAttackPlan.cardChannelPairB, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(newOpponentAttackPlan.cardChannelPairA, newOpponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, CharacterSelect.Player);

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null &&
            CardCategory.Defensive.HasFlag(opponentAttackPlan.cardChannelPairB.CardData.CardCategory) &&
            playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(playerAttackPlan.cardChannelPairA.CardData.CardUIController,
                                CharacterSelect.Player, opponentAttackPlan.cardChannelPairB.CardData.CardUIController);

            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null &&
            CardCategory.Defensive.HasFlag(playerAttackPlan.cardChannelPairB.CardData.CardCategory) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(opponentAttackPlan.cardChannelPairA.CardData.CardUIController,
                                CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB.CardData.CardUIController);

            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(playerAttackPlan.cardChannelPairA.CardData.CardUIController,
                CharacterSelect.Player);

            if(playerAttackPlan.cardChannelPairA.CardData.ApplyEffectsFirst)
                CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            else
                CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, null, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(opponentAttackPlan.cardChannelPairA.CardData.CardUIController,
                CharacterSelect.Opponent);

            if(opponentAttackPlan.cardChannelPairA.CardData.ApplyEffectsFirst)
                CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
            else
                CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);

            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, null, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(playerAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Player);

            if(playerAttackPlan.cardChannelPairB.CardData.ApplyEffectsFirst)
                CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
            else
                CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);

            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Player, null, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(opponentAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Opponent);
            
            if(opponentAttackPlan.cardChannelPairB.CardData.ApplyEffectsFirst)
                CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            else
                CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            
            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, null, CharacterSelect.Player);

        }

        ClearAllCards();
    }
    
    private void DisableDamageListeners()
    {
        CombatAnimationManager.OnEndedAnimation -= DealDamage;
        CombatManager.OnDestroyScene -= DisableDamageListeners;
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
                    //Enable offensive attack's effects.
                    if(offensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                    //Enable defensive card's effects.
                    if (defensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

                    //Calculate damage with guarded caveat.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
                }
                //Guard affects all possible channels, but was not successful.
                else
                {
                    //Enable offensive attack's effects.
                    if (offensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                    //Enable defensive card's effects.
                    if (defensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

                    //Calculate damage normally, enable offensive attack's effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                }
            }

            //Guard doesn't affect all possible channels, but is still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Enable offensive attack's effects.
                if (offensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                //Enable defensive card's effects.
                if (defensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

                //Calculate damage with guarded caveat.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
            }

            //Guard doesn't affect all channels and was not successful.
            else
            {
                //Enable offensive attack's effects.
                if (offensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                //Enable defensive card's effects.
                if (defensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

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
                    //Enable defensive card's effects and ignore the offensive card's effects.
                    if (defensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

                    //Calculate damage with counter caveat.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true, false);
                }

                //Counter affects all possible channels but was not successful.
                else
                {
                    //Enable offensive attack's effects.
                    if (offensiveCard.CardData.ApplyEffectsFirst)
                        CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                    else
                        CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                    //Calculate damage normally and enable offensive effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                }
            }

            //Counter doesn't affect all channels but was still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Enable offensive attack's effects.
                if (offensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

                //Enable defensive card's effects.
                if (defensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(defensiveCard, offensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(defensiveCard, offensiveMech);

                //Calculate damage with counter caveat.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true);
            }

            //Counter doesn't affect all channels and was not successful.
            else
            {
                //Enable offensive attack's effects.
                if (offensiveCard.CardData.ApplyEffectsFirst)
                    CombatManager.instance.CardPlayManager.EffectController.AddToPreDamageEffectQueue(offensiveCard, defensiveMech);
                else
                    CombatManager.instance.CardPlayManager.EffectController.AddToPostDamageEffectQueue(offensiveCard, defensiveMech);

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
        if (offensiveAttack == null || offensiveAttack.CardData == null)
            return;

        int repeatPlay = 1;

        if(CombatManager.instance.NarrateCombat)
        {
            combatLog += offensiveMech + " starting HP: " + (offensiveMech == CharacterSelect.Player ?
            CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP.ToString() : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
            combatLog += ". ";
            combatLog += defensiveMech + " starting HP: " + (defensiveMech == CharacterSelect.Player ?
                CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP.ToString() : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
            combatLog += ". ";
        }

        if (offensiveAttack.CardData.CardEffects != null)
            foreach (SOCardEffectObject effect in offensiveAttack.CardData.CardEffects)
            {
                if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                    repeatPlay = effect.EffectMagnitude;

                if (effect.EffectType == CardEffectTypes.KeyWord && effect.CardKeyWord == CardKeyWord.Flurry)
                    repeatPlay += CombatManager.instance.CardPlayManager.EffectController.GetAndConsumeFlurryBonus(offensiveMech);
            }

        if (offensiveAttack.CardData != null)
        {
            for (int i = 0; i < repeatPlay; i++)
            {
                if (counterDamage)
                {
                    CombatManager.instance.CombatAnimationManager.AddAnimationToQueue(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                    defensiveMech, defensiveCard.CardData.AnimationType);

                    AddDamageToQueue(new DamageMechPairObject(offensiveAttack, offensiveMech, true, false));

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                        combatLog += (offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " +
                            (CombatManager.instance.CardPlayManager.GetDamageEstimate(offensiveAttack, defensiveMech) 
                            * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier)) + ". "); 
                        combatLog += (offensiveMech + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else if(guardDamage)
                {
                    CombatManager.instance.CombatAnimationManager.AddAnimationToQueue(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                   defensiveMech, defensiveCard.CardData.AnimationType);

                    AddDamageToQueue(new DamageMechPairObject(offensiveAttack, defensiveMech, false, true));

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                        combatLog += (offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                            (CombatManager.instance.CardPlayManager.GetDamageEstimate(offensiveAttack, defensiveMech) * CombatManager.instance.GuardDamageMultiplier) + ". ");
                        combatLog += (offensiveMech + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else
                {
                    CombatManager.instance.CombatAnimationManager.AddAnimationToQueue(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                   defensiveMech, AnimationType.Damaged);

                    AddDamageToQueue(new DamageMechPairObject(offensiveAttack, defensiveMech, false, false));

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " for " + 
                            CombatManager.instance.CardPlayManager.GetDamageEstimate(offensiveAttack, defensiveMech) + " damage. ");
                        combatLog += (offensiveMech + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
            }

            if(CombatManager.instance.NarrateCombat)
            {
                combatLog += offensiveMech + " ending HP: " + (offensiveMech == CharacterSelect.Player ?
                    CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP);
                combatLog += ". ";

                combatLog += defensiveMech + " ending HP: " + (defensiveMech == CharacterSelect.Player ?
                    CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP);
                combatLog += ". ";

                Debug.Log(combatLog);
                combatLog = "";
            }
        }
    }

    private void AddDamageToQueue(DamageMechPairObject damageToDeal)
    {
        damageQueue.Enqueue(damageToDeal);
    }

    private void DealDamage()
    {
        if (damageQueue.Count == 0)
            return;

        DamageMechPairObject newDamage = damageQueue.Dequeue();

        CombatManager.instance.RemoveHealthFromMech(newDamage);
    }
}

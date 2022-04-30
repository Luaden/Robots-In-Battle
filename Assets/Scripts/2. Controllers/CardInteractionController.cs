using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardInteractionController
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    #region Debug
    private string combatLog;
    #endregion


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

            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, null, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(opponentAttackPlan.cardChannelPairA.CardData.CardUIController,
                CharacterSelect.Opponent);

            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, null, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(playerAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Player);

            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Player, null, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.CombatAnimationManager.SetCardOnBurnPile(opponentAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Opponent);

            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, null, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        ClearAllCards();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveMech, 
                                               CardChannelPairObject defensiveCard, CharacterSelect defensiveMech)
    {
        //Card is a Guard Type
        if (defensiveCard.CardData.CardCategory == CardCategory.Guard)
        {
            //Enable guard effects whether it's successful or not.
            CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(defensiveCard, offensiveMech);

            //Guard affects all possible channels.
            if (defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                //Guard affects all possible channels and is successful.
                if (defensiveCard.CardData.PossibleChannels.HasFlag(offensiveCard.CardChannel))
                {
                    //Deal damage with guarded caveat, enable offensive attack's effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
                    CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
                }

                //Guard affects all possible channels, but was not successful.
                else
                {
                    //Calculate damage normally, enable offensive attack's effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                    CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
                }
            }

            //Guard doesn't affect all possible channels, but is still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Calculate damage with guarded caveat, enable offensive attack's effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, false, true);
                CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
            }

            //Guard doesn't affect all channels and was not successful.
            else
            {
                //Calculate damage normally, enable offensive attack's effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
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
                    //Calculate damage with counter caveat, enable defensive effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true, false);
                    CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(defensiveCard, offensiveMech);
                }

                //Counter affects all possible channels but was not successful.
                else
                {
                    //Calculate damage normally and enable offensive effects.
                    CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                    CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
                }
            }

            //Counter doesn't affect all channels but was still successful.
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                //Calculate damage with counter caveat and enable defensive effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech, true);
                CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(defensiveCard, offensiveMech);
            }

            //Counter doesn't affect all channels and was not successful.
            else
            {
                //Calculate damage normally and enable offensive effects.
                CalculateMechDamage(offensiveCard, offensiveMech, defensiveCard, defensiveMech);
                CombatManager.instance.CardPlayManager.EffectController.AddToEffectQueue(offensiveCard, defensiveMech);
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

        combatLog += offensiveMech + " starting HP: " + (offensiveMech == CharacterSelect.Player ?
            CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP.ToString() : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
        combatLog += ". ";
        combatLog += defensiveMech + " starting HP: " + (defensiveMech == CharacterSelect.Player ?
            CombatManager.instance.PlayerFighter.FighterMech.MechCurrentHP.ToString() : CombatManager.instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
        combatLog += ". ";

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
                int damageToDeal = CombatManager.instance.CardPlayManager.EffectController.GetMechDamageWithAndConsumeModifiers(offensiveAttack, defensiveMech);
                
                if(counterDamage)
                {
                    CombatManager.instance.CombatAnimationManager.SetMechAnimation(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                    defensiveMech, defensiveCard.CardData.AnimationType);

                    CombatManager.instance.RemoveHealthFromMech(offensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
                    //CalculateComponentDamage(offensiveAttack, defensiveMech);

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                        combatLog += (offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " +
                            (damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier)) + ". ");
                        combatLog += (offensiveMech + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else if(guardDamage)
                {
                    CombatManager.instance.CombatAnimationManager.SetMechAnimation(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                   defensiveMech, defensiveCard.CardData.AnimationType);

                    CombatManager.instance.RemoveHealthFromMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier));
                    //CalculateComponentDamage(offensiveAttack, defensiveMech);

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                        combatLog += (offensiveMech + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                            (Mathf.RoundToInt(damageToDeal * CombatManager.instance.GuardDamageMultiplier)) + ". ");
                        combatLog += (offensiveMech + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else
                {
                    CombatManager.instance.CombatAnimationManager.SetMechAnimation(offensiveMech, offensiveAttack.CardData.AnimationType,
                                                                                   defensiveMech, AnimationType.Damaged);

                    CombatManager.instance.RemoveHealthFromMech(defensiveMech, damageToDeal);
                    CalculateComponentDamage(offensiveAttack, defensiveMech);

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (offensiveMech + " is playing " + offensiveAttack.CardData.CardName + " for " + damageToDeal + " damage. ");
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

            CombatManager.instance.CardPlayManager.EffectController.UpdateFighterBuffs();
        }
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        //Need to figure out the carddata damage + multiplier and figure out what component we're affecting.
    }
}

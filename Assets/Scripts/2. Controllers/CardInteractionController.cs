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
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(playerAttackPlan.cardChannelPairA.CardData.CardUIController,
                                CharacterSelect.Player, opponentAttackPlan.cardChannelPairB.CardData.CardUIController);

            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null &&
            CardCategory.Defensive.HasFlag(playerAttackPlan.cardChannelPairB.CardData.CardCategory) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(opponentAttackPlan.cardChannelPairA.CardData.CardUIController,
                                CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB.CardData.CardUIController);

            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB);
        }

        if (playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(playerAttackPlan.cardChannelPairA.CardData.CardUIController,
                CharacterSelect.Player);

            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(opponentAttackPlan.cardChannelPairA.CardData.CardUIController,
                CharacterSelect.Opponent);

            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(playerAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Player);

            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.MechAnimationManager.SetCardPlayAnimation(opponentAttackPlan.cardChannelPairB.CardData.CardUIController,
                CharacterSelect.Opponent);

            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        ClearAllCards();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        
        if (defensiveCard.CardData.CardCategory == CardCategory.Guard)
        {
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(defensiveCard, offensiveCharacter);

            if(defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                if (defensiveCard.CardData.PossibleChannels.HasFlag(offensiveCard.CardChannel))
                {
                    CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
                    CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
                }
                else
                {
                    CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                    CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
                }

            }
            else if (offensiveCard.CardChannel == defensiveCard.CardChannel)
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }

        }

        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
        {
            if (defensiveCard.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                if (defensiveCard.CardData.PossibleChannels.HasFlag(offensiveCard.CardChannel))
                {
                    CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
                    CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
                }
            }
            else if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
            {
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(defensiveCard, offensiveCharacter);
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), true);
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }
        }

        ClearCardsAfterDefenses(offensiveCharacter);
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

    private CharacterSelect GetOtherMech(CharacterSelect firstMech)
    {
        return firstMech == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player;
    }

    private void CalculateMechDamage(CardChannelPairObject offensiveAttack, CharacterSelect defensiveMech, bool counterDamage = false, bool guardDamage = false)
    {
        if (offensiveAttack == null || offensiveAttack.CardData == null)
            return;

        int repeatPlay = 1;
        bool hasDefended = false;


        combatLog += GetOtherMech(defensiveMech) + " starting HP: " + (GetOtherMech(defensiveMech) == CharacterSelect.Player ?
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
                    repeatPlay += CombatManager.instance.CardPlayManager.EffectController.GetAndConsumeFlurryBonus(GetOtherMech(defensiveMech));
            }

        if (offensiveAttack.CardData != null)
        {
            for (int i = 0; i < repeatPlay; i++)
            {
                int damageToDeal = CombatManager.instance.CardPlayManager.EffectController.GetMechDamageWithAndConsumeModifiers(offensiveAttack, defensiveMech);
                
                if(counterDamage && !hasDefended)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Counter, 
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    
                    CombatManager.instance.RemoveHealthFromMech(GetOtherMech(defensiveMech), damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
                    //CalculateComponentDamage(offensiveAttack, defensiveMech);
                    
                    hasDefended = true;

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " +
                            (damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier)) + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }

                    Debug.Log("Default Counter scenario.");
                }
                else if(guardDamage && !hasDefended)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Guard,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    
                    CombatManager.instance.RemoveHealthFromMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier));
                    //CalculateComponentDamage(offensiveAttack, defensiveMech);
                    
                    hasDefended = true;

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                            (Mathf.RoundToInt(damageToDeal * CombatManager.instance.GuardDamageMultiplier)) + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                    Debug.Log("Default Guard scenario.");
                }
                else if(CardCategory.Defensive.HasFlag(offensiveAttack.CardData.CardCategory))
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Idle,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.RemoveHealthFromMech(defensiveMech, damageToDeal);
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    
                    if(CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " for " + damageToDeal + " damage. ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }

                    Debug.Log("Awkward Guard scenario.");
                }
                else
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Damaged,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.RemoveHealthFromMech(defensiveMech, damageToDeal);
                    CalculateComponentDamage(offensiveAttack, defensiveMech);

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " for " + damageToDeal + " damage. ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }

                    Debug.Log("Default Attack scenario.");
                }
            }

            if(CombatManager.instance.NarrateCombat)
            {
                combatLog += GetOtherMech(defensiveMech) + " ending HP: " + (GetOtherMech(defensiveMech) == CharacterSelect.Player ?
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

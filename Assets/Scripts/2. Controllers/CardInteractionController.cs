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


    public void DetermineABInteraction(AttackPlanObject newPlayerAttackPlan, AttackPlanObject newOpponentAttackPlan)
    {
        playerAttackPlan = new AttackPlanObject(newPlayerAttackPlan.cardChannelPairA, newPlayerAttackPlan.cardChannelPairB, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(newOpponentAttackPlan.cardChannelPairA, newOpponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, CharacterSelect.Player);

        if (opponentAttackPlan.cardChannelPairB != null && CardCategory.Defensive.HasFlag(opponentAttackPlan.cardChannelPairB.CardData.CardCategory) &&
            playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairB != null && CardCategory.Defensive.HasFlag(playerAttackPlan.cardChannelPairB.CardData.CardCategory) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        ClearAllCards();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        
        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
        {
            CombatManager.instance.CardPlayManager.EffectController.EnableEffects(defensiveCard, offensiveCharacter);

            if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel) || defensiveCard.CardChannel.HasFlag(offensiveCard.CardChannel))
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }

            ClearCardsAfterDefenses(offensiveCharacter);
            return;
        }

        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
        {
            if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel) || defensiveCard.CardChannel.HasFlag(offensiveCard.CardChannel))
            {
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(defensiveCard, offensiveCharacter);
                CalculateMechDamage(offensiveCard, offensiveCharacter, true);
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                CombatManager.instance.CardPlayManager.EffectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }

            ClearCardsAfterDefenses(offensiveCharacter);
            return;
        }
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
        string combatLog = "";

        if(offensiveAttack.CardData.CardEffects != null)
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
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(GetOtherMech(defensiveMech), AnimationType.Counter, 
                        defensiveMech, CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    hasDefended = true;

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " +
                            (damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier)) + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else if(guardDamage && !hasDefended)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Guard,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier));
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    hasDefended = true;

                    if (CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                            (Mathf.RoundToInt(damageToDeal * CombatManager.instance.GuardDamageMultiplier)) + ". ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
                else
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Damaged,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal);
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    
                    if(CombatManager.instance.NarrateCombat)
                    {
                        combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " for " + damageToDeal + " damage. ");
                        combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                    }
                }
            }

            if(CombatManager.instance.NarrateCombat)
            {
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

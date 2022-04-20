using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageCalculatorController
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;
    private EffectController effectController;

    public EffectController EffectController { get => effectController; }

    public void DetermineABInteraction(AttackPlanObject newPlayerAttackPlan, AttackPlanObject newOpponentAttackPlan)
    {
        playerAttackPlan = new AttackPlanObject(newPlayerAttackPlan.cardChannelPairA, newPlayerAttackPlan.cardChannelPairB, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(newOpponentAttackPlan.cardChannelPairA, newOpponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent, CharacterSelect.Player);

        if (effectController == null)
            effectController = new EffectController();

        if (opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive) &&
            playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairA != null)
        {
            effectController.EnableEffects(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null)
        {
            effectController.EnableEffects(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null)
        {
            effectController.EnableEffects(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null)
        {
            effectController.EnableEffects(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        ClearAllCards();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
        {
            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
            {
                effectController.EnableEffects(defensiveCard, GetOtherMech(offensiveCharacter));

                effectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
            }

            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
            {
                effectController.EnableEffects(defensiveCard, GetOtherMech(offensiveCharacter));
                CalculateMechDamage(offensiveCard, offensiveCharacter, true);
            }
        }
        else
        {
            if (offensiveCharacter == CharacterSelect.Player)
                effectController.EnableEffects(offensiveCard, CharacterSelect.Opponent);
            else
                effectController.EnableEffects(offensiveCard, CharacterSelect.Player);
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

    private void CalculateMechDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech, bool counterDamage = false, bool guardDamage = false)
    {
        if (originAttack == null || originAttack.CardData == null)
            return;

        int repeatPlay = 1;

        if(originAttack.CardData.CardEffects != null)
            foreach (SOCardEffectObject effect in originAttack.CardData.CardEffects)
                if (effect.EffectType == CardEffectTypes.PlayMultipleTimes)
                    repeatPlay = effect.EffectMagnitude;

        if (originAttack.CardData != null)
        {
            for (int i = 0; i < repeatPlay; i++)
            {
                int damageToDeal = effectController.GetMechDamageWithModifiers(originAttack, destinationMech);
                
                if(counterDamage)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(destinationMech, AnimationType.Counter, 
                        GetOtherMech(destinationMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(originAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(destinationMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
                    CalculateComponentDamage(originAttack, destinationMech);
                }
                else if(guardDamage)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(destinationMech, AnimationType.Counter,
                        GetOtherMech(destinationMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(originAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(destinationMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier));
                    CalculateComponentDamage(originAttack, destinationMech);
                }
                else
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(destinationMech, AnimationType.Damaged,
                        GetOtherMech(destinationMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(originAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(destinationMech, damageToDeal);
                    CalculateComponentDamage(originAttack, destinationMech);
                }
            }
        }
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        //Need to figure out the carddata damage + multiplier and figure out what component we're affecting.
    }
}

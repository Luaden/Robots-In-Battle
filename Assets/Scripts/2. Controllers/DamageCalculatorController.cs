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

        if (opponentAttackPlan.cardChannelPairB != null && CardCategory.Defensive.HasFlag(opponentAttackPlan.cardChannelPairB.CardData.CardCategory) &&
            playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            Debug.Log("Defensive Interaction: Player Attack A vs Opponent Defense B.");
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);
        }

        if (playerAttackPlan.cardChannelPairB != null && CardCategory.Defensive.HasFlag(playerAttackPlan.cardChannelPairB.CardData.CardCategory) && 
            opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            {
                Debug.Log("Defensive Interaction: Opponent Attack A vs Player Defense B.");
                CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB);
            }
        }

        if (playerAttackPlan.cardChannelPairA != null)
        {
            Debug.Log("Player Attack A.");
            CalculateMechDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
            Debug.Log("Player Effect A.");
            effectController.EnableEffects(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairA != null)
        {
            Debug.Log("Player Attack B.");
            CalculateMechDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
            Debug.Log("Player Effect B.");
            effectController.EnableEffects(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);
        }

        if (playerAttackPlan.cardChannelPairB != null)
        {
            Debug.Log("Opponent Attack A.");
            CalculateMechDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
            Debug.Log("Opponent Effect A.");
            effectController.EnableEffects(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
        }

        if (opponentAttackPlan.cardChannelPairB != null)
        {
            Debug.Log("Opponent Attack B.");
            CalculateMechDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
            Debug.Log("Opponent Effect B.");
            effectController.EnableEffects(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);
        }

        ClearAllCards();
        effectController.UpdateFighterBuffs();
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        
        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
        {
            effectController.EnableEffects(defensiveCard, offensiveCharacter);

            if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel) || defensiveCard.CardChannel.HasFlag(offensiveCard.CardChannel))
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter), false, true);
                effectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                effectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
            }

            ClearCardsAfterDefenses(offensiveCharacter);
            return;
        }

        if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
        {
            if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel) || defensiveCard.CardChannel.HasFlag(offensiveCard.CardChannel))
            {
                effectController.EnableEffects(defensiveCard, offensiveCharacter);
                CalculateMechDamage(offensiveCard, offensiveCharacter, true);
            }
            else
            {
                CalculateMechDamage(offensiveCard, GetOtherMech(offensiveCharacter));
                effectController.EnableEffects(offensiveCard, GetOtherMech(offensiveCharacter));
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

                if (effect.EffectType == CardEffectTypes.KeyWordInitialize && effect.CardKeyWord == CardKeyWord.Flurry)
                    repeatPlay += effectController.GetFlurryBonus(GetOtherMech(defensiveMech));
            }



        if (offensiveAttack.CardData != null)
        {
            for (int i = 0; i < repeatPlay; i++)
            {
                int damageToDeal = effectController.GetMechDamageWithModifiers(offensiveAttack, defensiveMech);
                
                if(counterDamage && !hasDefended)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(GetOtherMech(defensiveMech), AnimationType.Counter, 
                        defensiveMech, CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    hasDefended = true;

                    combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is Countered by " + defensiveMech + ". ");
                    combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but will instead be dealt " + 
                        (damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier)) + ". ");
                    combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                }
                else if(guardDamage && !hasDefended)
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Guard,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier));
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    hasDefended = true;

                    combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " but is guarded by " + defensiveMech + ". ");
                    combatLog += (GetOtherMech(defensiveMech) + " would have dealt " + offensiveAttack.CardData.BaseDamage + " damage but this was reduced to " +
                        (damageToDeal * Mathf.RoundToInt(CombatManager.instance.GuardDamageMultiplier)) + ". ");
                    combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                }
                else
                {
                    CombatManager.instance.MechAnimationManager.SetMechAnimation(defensiveMech, AnimationType.Damaged,
                        GetOtherMech(defensiveMech), CombatManager.instance.MechAnimationManager.GetAnimationFromCategory(offensiveAttack.CardData.CardCategory));
                    CombatManager.instance.DealDamageToMech(defensiveMech, damageToDeal);
                    CalculateComponentDamage(offensiveAttack, defensiveMech);
                    combatLog += (GetOtherMech(defensiveMech) + " is playing " + offensiveAttack.CardData.CardName + " for " + damageToDeal + " damage. ");
                    combatLog += (GetOtherMech(defensiveMech) + "'s attack is " + i + " of " + repeatPlay + " total attacks. ");
                }
            }

            Debug.Log(combatLog);
        }
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        //Need to figure out the carddata damage + multiplier and figure out what component we're affecting.
    }
}

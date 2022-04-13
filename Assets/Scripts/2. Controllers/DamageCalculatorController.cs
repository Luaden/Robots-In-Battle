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

        if (opponentAttackPlan.cardChannelPairB.CardData != null && opponentAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairB.CardData != null && playerAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
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
        //End turn here
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        if (offensiveCard == null)
        {
            //Need some kind of confused block/counter animation?
            //Apply defensive effects anyway?
            return;
        }

        if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
        {
            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
            {
                //Attack animation
                //Guard Animation

                Debug.Log(offensiveCharacter + " was blocked, but this CardType currently does nothing on its own. Defensive effects will now be applied.");
                effectController.EnableEffects(defensiveCard, offensiveCharacter == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player);

                Debug.Log(offensiveCharacter + " will now apply offensive effects.");
                //Change this to CalculateMechDamage if effects don't apply to guarded characters.
                effectController.EnableEffects(offensiveCard, offensiveCharacter == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player);
            }

            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
            {
                //Attack animation
                //Counter animation

                Debug.Log(offensiveCharacter + " was countered. Their attack will be returned at 150%.");
                effectController.EnableEffects(defensiveCard, offensiveCharacter == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player);
                CalculateMechDamage(offensiveCard, offensiveCharacter, true);
            }
        }
        else
        {
            Debug.Log((offensiveCharacter == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player) + "'s defense was misplaced.");
            Debug.Log(offensiveCharacter + "'s attack made it through!");

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

    private void CalculateMechDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech, bool counterDamage = false)
    {
        Debug.Log(destinationMech + " was attacked!");

        //We need to check and account for multiple attacks as well as KeyWordExecutes here.

        //This is where we would pull our additional buffs/stats from
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
                
                //Animations for attacks
                CombatManager.instance.DealDamageToMech(destinationMech, 
                    counterDamage == false ? damageToDeal : damageToDeal * Mathf.RoundToInt(CombatManager.instance.CounterDamageMultiplier));
            }
        }

        CalculateComponentDamage(originAttack, destinationMech);
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        //Need to figure out the carddata damage + multiplier and figure out what component we're affecting.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        if (opponentAttackPlan.cardChannelPairB.CardData != null && opponentAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairB.CardData != null && playerAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairA != null)
            CalculateDamage(playerAttackPlan.cardChannelPairA, CharacterSelect.Opponent);

        if (opponentAttackPlan.cardChannelPairA != null)
            CalculateDamage(opponentAttackPlan.cardChannelPairA, CharacterSelect.Player);

        if (playerAttackPlan.cardChannelPairB != null)
            CalculateDamage(playerAttackPlan.cardChannelPairB, CharacterSelect.Opponent);

        if (opponentAttackPlan.cardChannelPairB != null)
            CalculateDamage(opponentAttackPlan.cardChannelPairB, CharacterSelect.Player);

        ClearAllCards();
        //End turn here
    }

    private void Start()
    {
        effectController = new EffectController();
    }

    private void CalculateDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech, bool counterDamage = false)
    {
        foreach(CardEffectObject cardEffect in originAttack.CardData.CardEffects)
            if(cardEffect.EffectType == CardEffectTypes.PlayMultipleTimes)
                for(int i = 0; i < cardEffect.EffectMagnitude; i++)
                {
                    CalculateMechDamage(originAttack, destinationMech, counterDamage);
                    CalculateComponentDamage(originAttack, destinationMech);
                    return;
                }

        CalculateMechDamage(originAttack, destinationMech, counterDamage);
        CalculateComponentDamage(originAttack, destinationMech);           
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        if (offensiveCard == null)
        {
            //Need some kind of confused block/counter animation?
            return;
        }

        if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
        {
            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
            {
                //Attack animation
                //Guard Animation

                Debug.Log(offensiveCharacter + " was blocked, but this card type currently does nothing on its own.");
                effectController.EnableDefensiveEffect(defensiveCard);
                CalculateDamage(offensiveCard, offensiveCharacter == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player);
            }

            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
            {
                //Attack animation
                //Counter animation

                Debug.Log(offensiveCharacter + " was countered.");
                effectController.EnableDefensiveEffect(defensiveCard);
                CalculateDamage(offensiveCard, offensiveCharacter, true);
            }
        }
        else
        {
            Debug.Log(offensiveCharacter + "'s attack made it through!");

            if (offensiveCharacter == CharacterSelect.Player)
                CalculateDamage(offensiveCard, CharacterSelect.Opponent);
            else
                CalculateDamage(offensiveCard, CharacterSelect.Player);
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
        
        //This is where we would pull our additional buffs/stats from
        //This is where we would check the EffectManager as well
        
        if (originAttack.CardData != null)
        {
            //Animations for attacks
            CombatManager.instance.DealDamageToMech(destinationMech, counterDamage == false ? 
                originAttack.CardData.BaseDamage : 
                Mathf.RoundToInt(originAttack.CardData.BaseDamage * 1.5f)); 
        }
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        //Need to figure out the carddata damage + multiplier and figure out what component we're affecting.
    }
}

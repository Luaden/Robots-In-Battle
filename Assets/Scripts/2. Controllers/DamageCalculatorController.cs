using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculatorController
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    public void DetermineABInteraction(AttackPlanObject playerAttackPlan, AttackPlanObject opponentAttackPlan)
    {
        this.playerAttackPlan = playerAttackPlan;
        this.opponentAttackPlan = opponentAttackPlan;

        if (opponentAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA.CardData, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB.CardData);

        if (playerAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
            CalculateDefensiveInteraction(opponentAttackPlan.cardChannelPairA.CardData, CharacterSelect.Opponent, playerAttackPlan.cardChannelPairB.CardData);

        if (playerAttackPlan.cardChannelPairA != null)
            CalculateOffensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player);

        if (opponentAttackPlan.cardChannelPairA != null)
            CalculateOffensiveInteraction(opponentAttackPlan.cardChannelPairA, CharacterSelect.Opponent);

        if (playerAttackPlan.cardChannelPairB != null)
            CalculateOffensiveInteraction(playerAttackPlan.cardChannelPairB, CharacterSelect.Player);

        if (opponentAttackPlan.cardChannelPairB != null)
            CalculateOffensiveInteraction(opponentAttackPlan.cardChannelPairB, CharacterSelect.Opponent);
    }

    private void CalculateOffensiveInteraction(CardChannelPairObject originAttack, CharacterSelect offensiveCharacter)
    {
        CalculateMechDamage(originAttack, offensiveCharacter);
        CalculateComponentDamage(originAttack, offensiveCharacter);
    }

    private void CalculateDefensiveInteraction(CardDataObject offensiveCard, CharacterSelect offensiveCharacter, CardDataObject defensiveCard)
    {
        //Check interactions

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

    private void CalculateMechDamage(CardChannelPairObject originAttack, CharacterSelect offensiveCharacter)
    {
        
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect offensiveCharacter)
    {

    }
}

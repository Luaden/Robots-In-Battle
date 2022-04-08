using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private DamageCalculatorController damageCalculator;

    public void BuildPlayerAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Player, CharacterSelect.Opponent);
        playerAttackPlan = attackPlan;
    }

    public void BuildOpponentAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Opponent, CharacterSelect.Player);
        opponentAttackPlan = attackPlan;

        if (playerAttackPlan != null && opponentAttackPlan != null)
            damageCalculator.DetermineABInteraction(playerAttackPlan, opponentAttackPlan);
        
        //Call for energy consumption here.
        
        ClearAttackPlans();
    }


    private void ClearAttackPlans()
    {
        playerAttackPlan = null;
        opponentAttackPlan = null;
    }

    private void Start()
    {
        damageCalculator = new DamageCalculatorController();
    }
}

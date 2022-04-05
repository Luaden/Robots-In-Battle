using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private DamageCalculatorController damageCalculator;

    public void SetPlayerAttackPlan(AttackPlanObject attackPlan)
    {
        playerAttackPlan = attackPlan;

        //Get opponent AttackPlanObject

        //DetermineAttackPlanInteraction();
    }

    private void Awake()
    {
        damageCalculator = new DamageCalculatorController();
    }
}

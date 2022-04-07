using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private DamageCalculatorController damageCalculator;

    public void BuildPlayerAttackPlan(List<CardChannelPairObject> cardChannelPairObjects)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjects, CharacterSelect.Player, CharacterSelect.Opponent);
        SetPlayerAttackPlan(attackPlan);
    }

    public void BuildOpponentAttackPlan(List<CardChannelPairObject> cardChannelPairObjects)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjects, CharacterSelect.Opponent, CharacterSelect.Player);
        SetPlayerAttackPlan(attackPlan);
    }

    private void SetPlayerAttackPlan(AttackPlanObject attackPlan)
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

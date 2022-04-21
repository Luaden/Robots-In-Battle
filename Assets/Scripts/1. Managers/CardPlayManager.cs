using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private DamageCalculatorController damageCalculator;
    
    public AttackPlanObject PlayerAttackPlan { get => playerAttackPlan; }

    public delegate void onCombatStart();
    public static event onCombatStart OnCombatStart;
    public delegate void onCombatComplete();
    public static event onCombatComplete OnCombatComplete;

    //public void BuildPlayerAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    //{
    //    AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Player, CharacterSelect.Opponent);
    //    playerAttackPlan = attackPlan;
    //}

    public void BuildOpponentAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Opponent, CharacterSelect.Player);
        opponentAttackPlan = attackPlan;        
    }

    public void PlayCards()
    {
        OnCombatStart?.Invoke();

        damageCalculator.DetermineABInteraction(playerAttackPlan, opponentAttackPlan);

        ClearAttackPlans();
    }


    private void ClearAttackPlans()
    {
        if(playerAttackPlan != null && playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairA.CardData.EnergyCost);
            CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairA.CardData);
        }

        if (playerAttackPlan != null && playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairB.CardData.EnergyCost);
            CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairB.CardData);
        }

        if(opponentAttackPlan != null && opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
        {
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairA.CardData.EnergyCost);
            CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairA.CardData);
        }

        if (opponentAttackPlan != null && opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
        {
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairB.CardData.EnergyCost);
            CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairB.CardData);
        }

        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Opponent, CharacterSelect.Player);

        OnCombatComplete?.Invoke();
    }

    private void Start()
    {
        damageCalculator = new DamageCalculatorController();
        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
    }
}

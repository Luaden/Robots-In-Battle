using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private DamageCalculatorController damageCalculator;
    
    public AttackPlanObject PlayerAttackPlan { get => playerAttackPlan; }

    public delegate void onCombatComplete();
    public static event onCombatComplete OnCombatComplete;

    public void BuildPlayerAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Player, CharacterSelect.Opponent);
        playerAttackPlan = attackPlan;
    }

    public void BuildOpponentAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Opponent, CharacterSelect.Player);
        opponentAttackPlan = attackPlan;
        
        //Call for energy consumption here.
        
    }

    public void PlayCards()
    {
        damageCalculator.DetermineABInteraction(playerAttackPlan, opponentAttackPlan);

        ClearAttackPlans();   
    }


    private void ClearAttackPlans()
    {
        CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairA.CardData.EnergyCost);
        CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairB.CardData.EnergyCost);
        CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairA.CardData.EnergyCost);
        CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairB.CardData.EnergyCost);

        CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairA.CardData);
        CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairB.CardData);
        CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairA.CardData);
        CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairB.CardData);

        CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairA.CardData);
        CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairB.CardData);
        CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairA.CardData);
        CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairB.CardData);



        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = null;

        OnCombatComplete?.Invoke();
    }

    private void Start()
    {
        damageCalculator = new DamageCalculatorController();
        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
    }
}

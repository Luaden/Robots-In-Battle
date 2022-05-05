using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private CardInteractionController cardInteractionController;

    public AttackPlanObject PlayerAttackPlan { get => playerAttackPlan; }

    public delegate void onBeginCardPlay();
    public static event onBeginCardPlay OnBeginCardPlay;


    public void BuildOpponentAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Opponent, CharacterSelect.Player);
        opponentAttackPlan = attackPlan;        
    }

    public void PlayCards()
    {
        OnBeginCardPlay.Invoke();

        if(CombatManager.instance.CanPlayCards)
        {
            cardInteractionController.DetermineCardInteractions(playerAttackPlan, opponentAttackPlan);
            ClearAttackPlans();
        }
    }

    private void ClearAttackPlans()
    {
        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Opponent, CharacterSelect.Player);
    }

    private void Start()
    {
        cardInteractionController = new CardInteractionController();
        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
    }
}

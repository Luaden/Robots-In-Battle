using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour
{
    private AttackPlanObject playerAttackPlan;
    private AttackPlanObject opponentAttackPlan;

    private CardInteractionController cardInteractionController;
    private EffectController effectController;

    public EffectController EffectController { get => effectController; }
    public AttackPlanObject PlayerAttackPlan { get => playerAttackPlan; }

    public delegate void onCombatStart();
    public static event onCombatStart OnCombatStart;
    public delegate void onCombatComplete();
    public static event onCombatComplete OnCombatComplete;

    public void BuildOpponentAttackPlan(CardChannelPairObject cardChannelPairObjectA, CardChannelPairObject cardChannelPairObjectB)
    {
        AttackPlanObject attackPlan = new AttackPlanObject(cardChannelPairObjectA, cardChannelPairObjectB, CharacterSelect.Opponent, CharacterSelect.Player);
        opponentAttackPlan = attackPlan;        
    }

    public void PlayCards()
    {
        if(CombatManager.instance.CanPlayCards)
        {
            OnCombatStart?.Invoke();

            cardInteractionController.DetermineCardInteractions(playerAttackPlan, opponentAttackPlan);

            ClearAttackPlans();
        }
    }

    public int GetDamageEstimate(CardChannelPairObject checkAttack, CharacterSelect target)
    {
        return effectController.GetMechDamageWithModifiers(checkAttack, target);
    }

    public FighterEffectObject GetFighterEffects(CharacterSelect character)
    {
        if(character == CharacterSelect.Player)
            return effectController.PlayerEffects;
        else 
            return effectController.OpponentEffects;
    }

    private void ClearAttackPlans()
    {
        if(playerAttackPlan != null && playerAttackPlan.cardChannelPairA != null && playerAttackPlan.cardChannelPairA.CardData != null)
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairA.CardData.EnergyCost);

        if (playerAttackPlan != null && playerAttackPlan.cardChannelPairB != null && playerAttackPlan.cardChannelPairB.CardData != null)
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, playerAttackPlan.cardChannelPairB.CardData.EnergyCost);

        if(opponentAttackPlan != null && opponentAttackPlan.cardChannelPairA != null && opponentAttackPlan.cardChannelPairA.CardData != null)
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairA.CardData.EnergyCost);

        if (opponentAttackPlan != null && opponentAttackPlan.cardChannelPairB != null && opponentAttackPlan.cardChannelPairB.CardData != null)
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Opponent, opponentAttackPlan.cardChannelPairB.CardData.EnergyCost);

        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
        opponentAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Opponent, CharacterSelect.Player);
    }

    private void Start()
    {
        cardInteractionController = new CardInteractionController();
        playerAttackPlan = new AttackPlanObject(null, null, CharacterSelect.Player, CharacterSelect.Opponent);
        effectController = new EffectController();

        CombatAnimationManager.OnAnimationsComplete += TurnComplete;
    }

    private void OnDestroy()
    {
        CombatAnimationManager.OnAnimationsComplete -= TurnComplete;
        EffectController.DisableEffectListeners();    
    }

    private void TurnComplete()
    {
        OnCombatComplete?.Invoke();
    }
}

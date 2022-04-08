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
            CalculateDefensiveInteraction(playerAttackPlan.cardChannelPairA, CharacterSelect.Player, opponentAttackPlan.cardChannelPairB);

        if (playerAttackPlan.cardChannelPairB.CardData.CardCategory.HasFlag(CardCategory.Defensive))
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

    private void CalculateDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        CalculateMechDamage(originAttack, destinationMech);
        CalculateComponentDamage(originAttack, destinationMech);           
    }

    private void CalculateDefensiveInteraction(CardChannelPairObject offensiveCard, CharacterSelect offensiveCharacter, CardChannelPairObject defensiveCard)
    {
        if (offensiveCard.CardChannel.HasFlag(defensiveCard.CardChannel))
        {
            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Guard))
            {
                //Attack animation
                //Guard Animation
                Debug.Log(offensiveCharacter + " was guarded against.");
            }

            if (defensiveCard.CardData.CardCategory.HasFlag(CardCategory.Counter))
            {
                //Attack animation
                //Counter animation

                Debug.Log(offensiveCharacter + " was countered.");
                CalculateDamage(offensiveCard, offensiveCharacter);
            }
        }
        else
        {
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
            CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairA.CardData);
            CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairB.CardData);

            ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairA.CardData);
            ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairB.CardData);

            playerAttackPlan.cardChannelPairA = null;
            opponentAttackPlan.cardChannelPairB = null;
        }
        else
        {
            CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairA.CardData);
            CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairB.CardData);

            ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairA.CardData);
            ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairB.CardData);

            playerAttackPlan.cardChannelPairB = null;
            opponentAttackPlan.cardChannelPairA = null;
        }
    }

    private void ClearAllCards()
    {
        if (playerAttackPlan.cardChannelPairA != null)
        {
            CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairA.CardData);
            ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairA.CardData);
        }

        if (playerAttackPlan.cardChannelPairB != null)
        {
            CombatManager.instance.CardUIManager.DestroyCardUI(playerAttackPlan.cardChannelPairB.CardData);
            ReturnCardToPlayerDeck(playerAttackPlan.cardChannelPairB.CardData);
        }

        if(opponentAttackPlan.cardChannelPairA != null)
        {
            CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairA.CardData);
            ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairA.CardData);
        }

        if(opponentAttackPlan.cardChannelPairB != null)
        {
            CombatManager.instance.CardUIManager.DestroyCardUI(opponentAttackPlan.cardChannelPairB.CardData);
            ReturnCardToOpponentDeck(opponentAttackPlan.cardChannelPairB.CardData);
        }

        playerAttackPlan = null;
        opponentAttackPlan = null;
    }

    private void CalculateMechDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {
        
    }

    private void CalculateComponentDamage(CardChannelPairObject originAttack, CharacterSelect destinationMech)
    {

    }

    private void ReturnCardToPlayerDeck(CardDataObject cardToReturn)
    {
        CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(cardToReturn);
    }

    private void ReturnCardToOpponentDeck(CardDataObject cardToReturn)
    {
        CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(cardToReturn);
    }
}

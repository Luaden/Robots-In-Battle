using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPileController : MonoBehaviour
{
    [SerializeField] private GameObject playerHighBurnPile;
    [SerializeField] private GameObject playerMidBurnPile;
    [SerializeField] private GameObject playerLowBurnPile;
    [SerializeField] private GameObject playerHighMidBurnPile;
    [SerializeField] private GameObject playerLowMidBurnPile;
    [SerializeField] private GameObject opponentHighBurnPile;
    [SerializeField] private GameObject opponentMidBurnPile;
    [SerializeField] private GameObject opponentLowBurnPile;
    [SerializeField] private GameObject opponentHighMidBurnPile;
    [SerializeField] private GameObject opponentLowMidBurnPile;

    private CardBurnObject currentCardBurnObject;

    public void PrepCardsToBurn(CardBurnObject newCardsToBurn)
    {
        currentCardBurnObject = newCardsToBurn;

        if (currentCardBurnObject.firstCharacter == CharacterSelect.Player)
        {
            ParentCardUIObject(currentCardBurnObject.firstCard, CharacterSelect.Player);

            if(currentCardBurnObject.secondCard != null)
                ParentCardUIObject(currentCardBurnObject.secondCard, CharacterSelect.Opponent);
        }
        else
        {
            ParentCardUIObject(currentCardBurnObject.firstCard, CharacterSelect.Opponent);

            if (currentCardBurnObject.secondCard != null)
                ParentCardUIObject(currentCardBurnObject.secondCard, CharacterSelect.Player);
        }
    }

    public void BurnCards()
    {
        if(currentCardBurnObject.firstCharacter == CharacterSelect.Player)
        {
            CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(currentCardBurnObject.firstCard.CardData);

            if (currentCardBurnObject.secondCard != null)
                CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(currentCardBurnObject.secondCard.CardData);
        }

        if(currentCardBurnObject.firstCharacter == CharacterSelect.Opponent)
        {
            CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(currentCardBurnObject.firstCard.CardData);

            if (currentCardBurnObject.secondCard != null)
                CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(currentCardBurnObject.secondCard.CardData);
        }
    }

    private void ParentCardUIObject(CardUIController card, CharacterSelect character)
    {
        if (character == CharacterSelect.Player)
        {
            if (card.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                switch (card.CardData.PossibleChannels)
                {
                    case Channels.HighMid:
                        card.PreviousParentObject = playerHighMidBurnPile.transform;
                        card.transform.SetParent(playerHighMidBurnPile.transform);
                        break;

                    case Channels.LowMid:
                        card.PreviousParentObject = playerLowMidBurnPile.transform;
                        card.transform.SetParent(playerLowMidBurnPile.transform);
                        break;
                }

                card.ShrinkCard();
                return;
            }

            switch (card.CardData.SelectedChannels)
            {
                case Channels.High:
                    card.PreviousParentObject = playerHighBurnPile.transform;
                    card.transform.SetParent(playerHighBurnPile.transform);
                    break;

                case Channels.Mid:
                    card.PreviousParentObject = playerMidBurnPile.transform;
                    card.transform.SetParent(playerMidBurnPile.transform);
                    break;

                case Channels.Low:
                    card.PreviousParentObject = playerLowBurnPile.transform;
                    card.transform.SetParent(playerLowBurnPile.transform);
                    break;
            }

            card.ShrinkCard();
            return;
        }
        else
        {
            if (card.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
            {
                switch (card.CardData.PossibleChannels)
                {
                    case Channels.HighMid:
                        card.PreviousParentObject = opponentHighMidBurnPile.transform;
                        card.transform.SetParent(opponentHighMidBurnPile.transform);
                        break;

                    case Channels.LowMid:
                        card.PreviousParentObject = opponentLowMidBurnPile.transform;
                        card.transform.SetParent(opponentLowMidBurnPile.transform);
                        break;
                }

                card.ShrinkCard();
                return;
            }

            switch (card.CardData.SelectedChannels)
            {
                case Channels.High:
                    card.PreviousParentObject = opponentHighBurnPile.transform;
                    card.transform.SetParent(opponentHighBurnPile.transform);
                    break;

                case Channels.Mid:
                    card.PreviousParentObject = opponentMidBurnPile.transform;
                    card.transform.SetParent(opponentMidBurnPile.transform);
                    break;

                case Channels.Low:
                    card.PreviousParentObject = opponentLowBurnPile.transform;
                    card.transform.SetParent(opponentLowBurnPile.transform);
                    break;
            }

            card.ShrinkCard();
            return;
        }
    }
}

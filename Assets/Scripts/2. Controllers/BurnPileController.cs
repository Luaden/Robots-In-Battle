using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPileController : MonoBehaviour
{
    [SerializeField] private GameObject playerHighBurnPile;
    [SerializeField] private GameObject playerMidBurnPile;
    [SerializeField] private GameObject playerLowBurnPile;
    [SerializeField] private GameObject opponentHighBurnPile;
    [SerializeField] private GameObject opponentMidBurnPile;
    [SerializeField] private GameObject opponentLowBurnPile;

    private Queue<List<CardCharacterPairObject>> burnCardCharacterQueue = new Queue<List<CardCharacterPairObject>>();
    private Queue<List<CardCharacterPairObject>> destroyQueue = new Queue<List<CardCharacterPairObject>>();

    public delegate void onCardBurnComplete();
    public static event onCardBurnComplete OnCardBurnComplete;

    public void SetCardOnBurnPile(CardUIController firstCard, CharacterSelect firstCardOwner, CardUIController secondCard = null)
    {
        if (firstCard == null)
            return;

        List<CardCharacterPairObject> newCardList = new List<CardCharacterPairObject>();

        if(firstCardOwner == CharacterSelect.Player)
        {
            CardCharacterPairObject cardCharacterPair = new CardCharacterPairObject();
            cardCharacterPair.card = firstCard;
            cardCharacterPair.character = CharacterSelect.Player;
            firstCard.CardSlotController.SlotManager.RemoveItemFromCollection(firstCard);
            firstCard.CardSlotController = null;

            newCardList.Add(cardCharacterPair);
            
            if(secondCard != null)
            {
                cardCharacterPair = new CardCharacterPairObject();
                cardCharacterPair.card = secondCard;
                cardCharacterPair.character = CharacterSelect.Opponent;
                secondCard.CardSlotController.SlotManager.RemoveItemFromCollection(secondCard);
                secondCard.CardSlotController = null;
                
                newCardList.Add(cardCharacterPair);
            }

            burnCardCharacterQueue.Enqueue(newCardList);
            return;
        }
        else
        {
            CardCharacterPairObject cardCharacterPair = new CardCharacterPairObject();
            cardCharacterPair.card = firstCard;
            cardCharacterPair.character = CharacterSelect.Opponent;
            firstCard.CardSlotController.SlotManager.RemoveItemFromCollection(firstCard);
            firstCard.CardSlotController = null;

            newCardList.Add(cardCharacterPair);

            if (secondCard != null)
            {
                cardCharacterPair = new CardCharacterPairObject();
                cardCharacterPair.card = secondCard;
                cardCharacterPair.character = CharacterSelect.Player;
                secondCard.CardSlotController.SlotManager.RemoveItemFromCollection(secondCard);
                secondCard.CardSlotController = null;

                newCardList.Add(cardCharacterPair);
            }

            burnCardCharacterQueue.Enqueue(newCardList);
        }
    }

    public void BurnCards()
    {
        for(int i = 0; i < destroyQueue.Count; i++)
        {
            List<CardCharacterPairObject> cardCharacterPairs = new List<CardCharacterPairObject>(destroyQueue.Dequeue());

            foreach (CardCharacterPairObject cardCharacterPair in cardCharacterPairs)
            {
                if (cardCharacterPair.character == CharacterSelect.Player)
                    CombatManager.instance.DeckManager.ReturnCardToPlayerDeck(cardCharacterPair.card.CardData);
                else
                    CombatManager.instance.DeckManager.ReturnCardToOpponentDeck(cardCharacterPair.card.CardData);
            }

            if (destroyQueue.Count == 0 && burnCardCharacterQueue.Count == 0)
                OnCardBurnComplete?.Invoke();
        }

        if (burnCardCharacterQueue.Count > 0)
        {
            List<CardCharacterPairObject> newDestroyCardList = new List<CardCharacterPairObject>();
            List<CardCharacterPairObject> cardCharacterPairs = new List<CardCharacterPairObject>(burnCardCharacterQueue.Dequeue());

            foreach (CardCharacterPairObject cardCharacterPair in cardCharacterPairs)
            {
                if(cardCharacterPair.character == CharacterSelect.Player)
                {
                    Debug.Log(cardCharacterPair.card.CardData.SelectedChannels);

                    switch (cardCharacterPair.card.CardData.SelectedChannels)
                    {
                        case Channels.High:
                            cardCharacterPair.card.PreviousParentObject = playerHighBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(playerHighBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                        case Channels.Mid:
                            cardCharacterPair.card.PreviousParentObject = playerMidBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(playerMidBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                        case Channels.Low:
                            cardCharacterPair.card.PreviousParentObject = playerLowBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(playerLowBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                    }
                }
                else
                {
                    switch (cardCharacterPair.card.CardData.SelectedChannels)
                    {
                        case Channels.High:
                            cardCharacterPair.card.PreviousParentObject = opponentHighBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(opponentHighBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                        case Channels.Mid:
                            cardCharacterPair.card.PreviousParentObject = opponentMidBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(opponentMidBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                        case Channels.Low:
                            cardCharacterPair.card.PreviousParentObject = opponentLowBurnPile.transform;
                            cardCharacterPair.card.transform.SetParent(opponentLowBurnPile.transform);
                            newDestroyCardList.Add(cardCharacterPair);
                            break;
                    }
                }
            }

            destroyQueue.Enqueue(newDestroyCardList);
        }
    }

    private class CardCharacterPairObject
    {
        public CharacterSelect character;
        public CardUIController card;
    }

    private void Start()
    {
        MechAnimationManager.OnStartingAnimation += BurnCards;
    }

    private void OnDestroy()
    {
        MechAnimationManager.OnStartingAnimation -= BurnCards;
    }
}

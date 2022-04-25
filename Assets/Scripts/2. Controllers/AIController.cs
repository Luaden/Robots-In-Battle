using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Debug Functions
    public void TestCardSelection()
    {
        List<CardPriorityPairObject> cardPool = new List<CardPriorityPairObject>();
        cardPool = GetCurrentHandPossibleAttacks(true);

        RankChoicesWithAggressiveness(cardPool);

        foreach (CardPriorityPairObject card in cardPool)
            Debug.Log(card.card.CardName + " : " + card.priority);
    }
    #endregion

    [Tooltip("A value range between -10 and 10 that represents the AI preference for either offense or defense. " +
        "A value of -10 means the AI will always use defensive cards unless none are available, whereas a value of 10 means the AI will always use damageing " +
        "neutral cards unless there are none are available.")]
    [Range(-10, 10)] [SerializeField] private int aggressiveness;
    [Tooltip("A value range between -10 and 10 that represents the AI preference for raw damage or targeting components. " +
        "A value of -10 means the AI will primarily target weaker components to break them, whereas a value of 10 means the AI will always prioritize higher " +
        "base damage for attacks.")]
    [Range(-10, 10)] [SerializeField] private int precision;
    [Tooltip("A value range between -10 and 10 that represents the AI ability to choose the best option given its other " +
        "behavioral traits and battle context. A value of -10 means that the AI will always pick the best action from other behavioral guidances, but will ignore " +
        "battlefield context such as buffs, debuffs, or shields, whereas a value of 10 means that the AI will value cards that benefit from buffs higher, and  " +
        "cards that are mitigated lower.")]
    [Range(-10, 10)] [SerializeField] private int battlefieldIntelligence;
    [Tooltip("A value range between -10 and 10 that represents the AI ability to recognize deck composition and play cards " +
        "that will benefit from bonuses when those bonuses are available. I.e. KeyWordInit and KeyWordExec, as well as CardCategory buffs such as Punch damage" +
        "damage buffs when punches are available. A value of -10 means that these contexts are completely ignored while a value of 10 means that cards will be " +
        "not be played if there is a potential boost available.")]
    [Range(-10, 10)] [SerializeField] private int deckIntelligence;

    private List<CardDataObject> opponentHand;
    private List<CardDataObject> cardPool;
    private CardChannelPairObject attackA;
    private CardChannelPairObject attackB;

    //Check hand - what can I hit?
    //Check player - are any components particularly weak? Can I hit them?
    //Assign value ratings based on behavior multipliers to each potential attack.

    //Check player hand, check energy consumed from queued card, any cards consuming the same value, look at the channels they hit.
    //Check self - are any components particularly weak? Can I guard them?
    //Assign ratings to defensive options

    //Queue attack

    private void Start()
    {
        ChannelsUISlotManager.OnASlotFilled += BuildCardChannelPairA;
        ChannelsUISlotManager.OnBSlotFilled += BuildCardChannelPairB;
        CardPlayManager.OnCombatStart += FinalCheck;
    }

    private void OnDestroy()
    {
        ChannelsUISlotManager.OnASlotFilled -= BuildCardChannelPairA;
        ChannelsUISlotManager.OnBSlotFilled -= BuildCardChannelPairB;
        CardPlayManager.OnCombatStart -= FinalCheck;
    }

    private void BuildCardChannelPairA()
    {
        if (attackA != null)
            return;

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        
        List<CardDataObject> possibleCards = new List<CardDataObject>();
        CardDataObject selectedCard;

        foreach (CardDataObject card in opponentHand)
            if (card.EnergyCost <= CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
                if(card.CardType == CardType.Attack || card.CardType == CardType.Neutral)
                    possibleCards.Add(card);


        if(possibleCards.Count == 0)
        {
            selectedCard = null;

            attackA = new CardChannelPairObject(selectedCard, Channels.None);

            CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(null, null);
        }
        else
        {
            selectedCard = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];

            attackA = new CardChannelPairObject(selectedCard, GetRandomChannelFromFlag(selectedCard.PossibleChannels));

            CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(selectedCard.CardUIObject.GetComponent<CardUIController>(),
                selectedCard.CardUIObject.GetComponent<CardUIController>().CardSlotController);
        }
    }

    private void BuildCardChannelPairB()
    {
        if (attackB != null)
            return;

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;

        List<CardDataObject> possibleCards = new List<CardDataObject>();
        CardDataObject selectedCard;

        foreach (CardDataObject card in opponentHand)
            if (card.EnergyCost <= CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
                if (card.CardType == CardType.Defense || card.CardType == CardType.Neutral)
                    possibleCards.Add(card);

        if (possibleCards.Count == 0)
        {
            selectedCard = null;

            attackB = new CardChannelPairObject(selectedCard, Channels.None);

            CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(null, null);
        }
        else
        {
            selectedCard = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];

            attackB = new CardChannelPairObject(selectedCard, GetRandomChannelFromFlag(selectedCard.PossibleChannels));

            CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(selectedCard.CardUIObject.GetComponent<CardUIController>(),
                selectedCard.CardUIObject.GetComponent<CardUIController>().CardSlotController);
        }
    }

    private void FinalCheck()
    {
        if (attackA == null)
            BuildCardChannelPairA();

        if (attackB == null)
            BuildCardChannelPairB();

        CombatManager.instance.CardPlayManager.BuildOpponentAttackPlan(attackA, attackB);

        attackA = null;
        attackB = null;
    }

    private Channels GetRandomChannelFromFlag(Channels channel)
    {
        System.Random random = new System.Random();

        Channels[] allChannels = Enum.GetValues(typeof(Channels)).Cast<Channels>().Where(x => channel.HasFlag(x)).ToArray();
        Channels randomChannel = allChannels[random.Next(1, allChannels.Length)];

        return randomChannel;
    }

    private List<CardPriorityPairObject> GetCurrentHandPossibleAttacks(bool aSlot)
    {
        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        List<CardPriorityPairObject> cardPool = new List<CardPriorityPairObject>();

        foreach (CardDataObject card in opponentHand)
            if (card.EnergyCost <= CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
            {
                if (aSlot && (card.CardType == CardType.Attack || card.CardType == CardType.Neutral))
                {
                    CardPriorityPairObject newCardPriority = new CardPriorityPairObject();
                    newCardPriority.card = card;
                    newCardPriority.priority = 0;

                    cardPool.Add(newCardPriority);

                }

                if (!aSlot && (card.CardType == CardType.Defense || card.CardType == CardType.Neutral))
                {
                    CardPriorityPairObject newCardPriority = new CardPriorityPairObject();
                    newCardPriority.card = card;
                    newCardPriority.priority = 0;

                    cardPool.Add(newCardPriority);

                }
            }

        return cardPool;
    }

    private void RankChoicesWithAggressiveness(List<CardPriorityPairObject> cardPriorityPairs)
    {
        if(PositiveValue(aggressiveness))
        {
            foreach(CardPriorityPairObject cardPrioPair in cardPriorityPairs)
            {
                switch (cardPrioPair.card.CardType)
                {
                    case CardType.Attack:
                        cardPrioPair.priority += aggressiveness;
                        break;
                    case CardType.Defense:
                        break;
                    case CardType.Neutral:
                        foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                            if (CardEffectTypes.Offensive.HasFlag(cardEffect.EffectType))
                            {
                                cardPrioPair.priority += aggressiveness;
                                break;
                            }
                        break;
                }
            }
        }
        else
            foreach (CardPriorityPairObject cardPrioPair in cardPriorityPairs)
            {
                switch (cardPrioPair.card.CardType)
                {
                    case CardType.Attack:
                        break;
                    case CardType.Defense:
                        cardPrioPair.priority += Mathf.Abs(aggressiveness);
                        break;
                    case CardType.Neutral:
                        foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                            if (CardEffectTypes.Defensive.HasFlag(cardEffect.EffectType))
                            {
                                cardPrioPair.priority += Mathf.Abs(aggressiveness);
                                break;
                            }
                        break;
                }
            }
    }

    private void RankChoicesWithPrecision()
    {

    }

    private void RankChoicesWithBattlefieldIntel()
    {

    }

    private void RankChoicesWithDeckIntel()
    {

    }

    private void GetBestCardChoice()
    {

    }

    private bool PositiveValue(int value)
    {
        if (value >= 0)
            return true;
        else
            return false;
    }

    private class CardPriorityPairObject
    {
        public CardDataObject card;
        public int priority;
    }
}

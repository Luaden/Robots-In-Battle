using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Tooltip("A value range between 1 and 10 that represents the AI preference for either offense or defense. " +
        "A value of 1 means the AI will always use defensive cards unless none are available, whereas a value of 10 means the AI will always use attack cards " +
        "unless there are none are available.")]
    [Range(1, 10)] [SerializeField] private float aggressiveness;
    [Tooltip("A value range between 1 and 10 that represents the AI preference for raw damage or targeting components. " +
        "A value of -10 means the AI will primarily target weaker components to break them, whereas a value of 10 means the AI will always prioritize higher " +
        "base damage for attacks.")]
    [Range(1, 10)] [SerializeField] private float precision;
    [Tooltip("A value range between 1 and 10 that represents the AI ability to choose the best option given its other " +
        "behavioral traits and battle context. A value of 1 means that the AI will always pick the best action from other behavioral guidances, but will ignore " +
        "battlefield context such as buffs, debuffs, or shields, whereas a value of 10 means that the AI will value cards that benefit from buffs higher, and  " +
        "cards that are mitigated lower.")]
    [Range(1, 10)] [SerializeField] private float battlefieldIntelligence;
    [Tooltip("A value range between 1 and 10 that represents the AI ability to recognize deck composition and play cards " +
        "that will benefit from bonuses when those bonuses are available. I.e. KeyWordInit and KeyWordExec, as well as CardCategory buffs such as Punch damage" +
        "damage buffs when punches are available. A value of 1 means that these contexts are completely ignored while a value of 10 means that cards will be " +
        "not be played if there is a potential boost available.")]
    [Range(1, 10)] [SerializeField] private float deckIntelligence;

    private List<CardDataObject> opponentHand;
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

    private void RankChoicesWithAggressiveness()
    {

    }

    private void RankChoicesWithPrecision()
    {

    }

    private void RankChoicesWithIntelligence()
    {

    }

    private void GetBestCardChoice()
    {

    }
}

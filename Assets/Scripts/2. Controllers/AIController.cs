using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Debug Functions

    [SerializeField] private bool aSlot = true;
    
    [ContextMenu("Test Choices")]
    public void TestCardSelection()
    {
        if (CombatManager.instance.HandManager.OpponentHand.CharacterHand == null)
            GameManager.instance.BuildMech();

        List<CardPlayPriorityObject> cardPlays = GetCurrentHandPossibleAttacks(aSlot);
        
        WeightPriorityWithAggressiveness(cardPlays);
        WeightPriorityWithDefensiveness(cardPlays);
        WeightPriorityWithDamage(cardPlays);
        WeightPriorityWithComponentDamage(cardPlays);

        foreach (CardPlayPriorityObject card in cardPlays)
            Debug.Log(card.card.CardName + " : " + card.priority);
    }
    #endregion

    [Tooltip("A value range between 0 and 5 that represents the AI preference for B Slot offense or defense. A value of 5 means the AI will value damaging " +
        "Neutral cards more than Defense cards.")]
    [Range(0, 5)] [SerializeField] private int aggressivenessWeight;

    [Tooltip("A value range between 0 and 5 that represents the AI preference for A Slot offense or defense. A value of 5 means the AI will value defensive " +
        "Neutral cards more than Attack cards.")]
    [Range(0, 5)] [SerializeField] private int defensivenessWeight;
    
    [Tooltip("A value range between 0 and 5 that represents the AI preference for base damage. A value of 5 means the AI will value attacks that deal more" +
    "base damage more than those that do less.")]
    [Range(0, 5)] [SerializeField] private int baseDamageWeight;
    
    [Tooltip("A value range between 0 and 5 that represents the AI preference for component damage. A value of 5 means the AI will value attacks that deal " +
        "or benefit from bonus component damage more than those that do not.")]
    [Range(0, 5)] [SerializeField] private int componentDamageWeight;

    [Tooltip("A value range between 0 and 5 that represents the AI preference for targeting weaker components. A value of 5 means that the AI will value attacks " +
        "that can target weaker components more.")]
    [Range(0, 5)] [SerializeField] private int targetingWeight;

    [Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that benefit from buffs. A value of 5 means that the AI will " +
        "value cards that benefit from buffs more than those that do not.")]
    [Range(0, 5)] [SerializeField] private int benefitWeight;
    
    [Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that are not mitigated by debuffs. A value of 5 means that the " +
        "AI will value cards that are mitigated by buffs and shields less than those that are not.")]
    [Range(0, 5)] [SerializeField] private int mitigationWeight;

    [Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that apply effects. A value of 5 means that the AI will value " +
        "cards that apply effects such as buffs, debuffs, and shields more than those that do not.")]
    [Range(0, 5)] [SerializeField] private int applicationWeight;

    [Tooltip("A value range between 0 and 5 that represents the AI ability to accurately defend against attacks. A value of 5 means that the AI will make an " +
        "accurate guess as to where to defend given the available information, whereas a value of 0 will randomly guess from the channels able to be guarded.")]
    [Range(0, 5)] [SerializeField] private int defensivePredictionAccuracy;

    [Tooltip("A value range between 0 and 5 that represents the AI ability to accurately choose attacks that cannot be guarded or countered. A value of 5 " +
        "means that the AI will value attacks that cannot be guarded or countered more where a value of 0 means that the AI will not consider the player's " +
        "defensive cards when picking where to attack.")]
    [Range(0, 5)] [SerializeField] private int offensivePredictionAccuracy;


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

    #region AI Demo
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
    #endregion

    private List<CardPlayPriorityObject> GetCurrentHandPossibleAttacks(bool aSlot)
    {
        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        List<CardPlayPriorityObject> cardPlays = new List<CardPlayPriorityObject>();

        foreach (CardDataObject card in opponentHand)
        {
            if (card.EnergyCost > CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
                continue;

            if (aSlot && (card.CardType == CardType.Attack || card.CardType == CardType.Neutral))
                foreach (Channels channel in GetChannelListFromFlags(card.PossibleChannels))
                    cardPlays.Add(CreateCardPlayPriorityObject(card, channel));

            if (!aSlot && (card.CardType == CardType.Defense || card.CardType == CardType.Neutral))
                foreach (Channels channel in GetChannelListFromFlags(card.PossibleChannels))
                    cardPlays.Add(CreateCardPlayPriorityObject(card, channel));
        }

        return cardPlays;
    }

    private void WeightPriorityWithAggressiveness(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        foreach (CardPlayPriorityObject cardPrioPair in cardPlayPriorityObjects)
        {
            switch (cardPrioPair.card.CardType)
            {
                case CardType.Attack:
                    cardPrioPair.priority += aggressivenessWeight;
                    break;
                case CardType.Defense:
                    break;
                case CardType.Neutral:
                    foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                        if (CardEffectTypes.Offensive.HasFlag(cardEffect.EffectType))
                        {
                            cardPrioPair.priority += aggressivenessWeight;
                            break;
                        }
                    break;
            }
        }
    }

    private void WeightPriorityWithDefensiveness(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        foreach (CardPlayPriorityObject cardPrioPair in cardPlayPriorityObjects)
        {
            switch (cardPrioPair.card.CardType)
            {
                case CardType.Attack:
                    break;
                case CardType.Defense:
                    cardPrioPair.priority += aggressivenessWeight;
                    break;
                case CardType.Neutral:
                    foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                        if (CardEffectTypes.Defensive.HasFlag(cardEffect.EffectType))
                        {
                            cardPrioPair.priority += aggressivenessWeight;
                            break;
                        }
                    break;
            }
        }
    }

    private void WeightPriorityWithDamage(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        int maximumDamage = 0;

        foreach (CardPlayPriorityObject cardPlayPriority in cardPlayPriorityObjects)
            if (cardPlayPriority.card.BaseDamage > maximumDamage)
                maximumDamage = cardPlayPriority.card.BaseDamage;

        foreach (CardPlayPriorityObject cardPlayPriority in cardPlayPriorityObjects)
        {
            if (cardPlayPriority.card.BaseDamage <= 0)
                continue;

            cardPlayPriority.priority += Mathf.RoundToInt((cardPlayPriority.card.BaseDamage / maximumDamage) * baseDamageWeight);
        }
    }

    private void WeightPriorityWithComponentDamage(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        FighterEffectObject playerEffectObject = CombatManager.instance.CardPlayManager.GetFighterEffects(CharacterSelect.Player);
        List<ElementStackObject> channelElementStacks = new List<ElementStackObject>();
        int maximumComponentDamage = 0;

        foreach (CardPlayPriorityObject cardPlayPriority in cardPlayPriorityObjects)
            if (cardPlayPriority.card.BaseDamage > maximumComponentDamage)
                maximumComponentDamage = Mathf.RoundToInt(cardPlayPriority.card.BaseDamage * GetComponentDamageMultiplier(cardPlayPriority));

        foreach (CardPlayPriorityObject cardPlayPriority in cardPlayPriorityObjects)
        {
            if (cardPlayPriority.card.BaseDamage <= 0)
                continue;

            cardPlayPriority.priority += Mathf.RoundToInt(((cardPlayPriority.card.BaseDamage * GetComponentDamageMultiplier(cardPlayPriority)) 
                                                            / maximumComponentDamage) * componentDamageWeight);
        }

        float GetComponentDamageMultiplier(CardPlayPriorityObject cardPlayPriorityObject)
        {
            CardDataObject cardData = cardPlayPriorityObject.card;

            float componentDamageMultiplier = 1;

            switch (cardData.CardCategory)
            {
                case CardCategory.Punch:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechArms.BonusDamageFromComponent;
                    break;

                case CardCategory.Kick:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.BonusDamageFromComponent;
                    break;

                case CardCategory.Special:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechHead.BonusDamageFromComponent;
                    break;
            }

            if (playerEffectObject.IceAcidStacks.TryGetValue(cardPlayPriorityObject.channel, out channelElementStacks))
            {
                Debug.Log("Found IceAcid Dictionary.");
                foreach (ElementStackObject element in channelElementStacks)
                    if (element.ElementType == ElementType.Acid)
                    {
                        Debug.Log("Found acid element in channel.");
                        componentDamageMultiplier += CombatManager.instance.AcidComponentDamageMultiplier;
                    }
            }

            return componentDamageMultiplier;
        }
    }

    private void WeightPriorityWithTargetWeight(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        FighterEffectObject playerEffectObject = CombatManager.instance.CardPlayManager.GetFighterEffects(CharacterSelect.Player);
        
        foreach (CardPlayPriorityObject cardPrioPair in cardPlayPriorityObjects)
        {
            switch (cardPrioPair.card.CardType)
            {
                case CardType.Attack:
                    break;
                case CardType.Defense:
                    cardPrioPair.priority += aggressivenessWeight;
                    break;
                case CardType.Neutral:
                    foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                        if (CardEffectTypes.Defensive.HasFlag(cardEffect.EffectType))
                        {
                            cardPrioPair.priority += aggressivenessWeight;
                            break;
                        }
                    break;
            }
        }
    }

    #region Utility
    private List<Channels> GetChannelListFromFlags(Channels channelToInterpret)
    {
        List<Channels> channelList = new List<Channels>();

        if (channelToInterpret.HasFlag(Channels.High))
            channelList.Add(Channels.High);
        if (channelToInterpret.HasFlag(Channels.Mid))
            channelList.Add(Channels.Mid);
        if (channelToInterpret.HasFlag(Channels.Low))
            channelList.Add(Channels.Low);

        return channelList;
    }


    private CardPlayPriorityObject CreateCardPlayPriorityObject(CardDataObject card, Channels channel)
    {
        CardPlayPriorityObject newCardPriority = new CardPlayPriorityObject();
        newCardPriority.card = card;
        newCardPriority.channel = channel;
        newCardPriority.priority = 0;

        return newCardPriority;
    }

    private class CardPlayPriorityObject
    {
        public CardDataObject card;
        public Channels channel;
        public int priority;
    }
    #endregion
}

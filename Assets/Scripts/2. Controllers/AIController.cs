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
        List<CardPlayPriorityObject> cardPlays = GetCurrentPossibleCards(aSlot);

        WeightCardPlayValues(cardPlays);

        foreach (CardPlayPriorityObject card in cardPlays)
            Debug.Log(card.card.CardName + ", " + card.channel + ": " + card.priority);
    }
    #endregion

    [Tooltip("A value range between 0 and 9 that represents variance for AI priority preference. This value is added or subtracted from each possible move's " +
        "valuation in order to add variance from rigid strategy. 0 represents an accurate strategy of the weighted values below while 9 represents the most " +
        "added randomness. Chaos reigns.")]
    [Range(0, 9)] [SerializeField] private int randomizationWeight;
    [Tooltip("A value range between 0 and 9 that represents the AI preference for B Slot offense or defense. A value of 5 means the AI will value damaging " +
        "Neutral cards more than Defense cards.")]
    [Range(0, 9)] [SerializeField] private int aggressivenessWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for A Slot offense or defense. A value of 5 means the AI will value defensive " +
        "Neutral cards more than Attack cards.")]
    [Range(0, 9)] [SerializeField] private int defensivenessWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for base damage. A value of 5 means the AI will value attacks that deal more" +
    "base damage more than those that do less.")]
    [Range(0, 9)] [SerializeField] private int baseDamageWeight;

    [Tooltip("(Component Damage Multiplier) A value range between 0 and 9 that represents the AI preference for component damage. A value of 5 means the AI " +
        "will value attacks that deal or benefit from bonus component damage more than those that do not.")]
    [Range(0, 9)] [SerializeField] private int cDMWeight;

    [Tooltip("A value range between 0 and 9 that represents the AI preference for targeting weaker components. A value of 5 means that the AI will value attacks " +
        "that can target weaker components more.")]
    [Range(0, 9)] [SerializeField] private int componentHealthWeight;

    //[Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that benefit from buffs. A value of 5 means that the AI will " +
    //    "value cards that benefit from buffs more than those that do not.")]
    //[Range(0, 5)] [SerializeField] private int benefitWeight;

    //[Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that are not mitigated by debuffs. A value of 5 means that the " +
    //    "AI will value cards that are mitigated by buffs and shields less than those that are not.")]
    //[Range(0, 5)] [SerializeField] private int mitigationWeight;

    //[Tooltip("A value range between 0 and 5 that represents the AI preference to play cards that apply effects. A value of 5 means that the AI will value " +
    //    "cards that apply effects such as buffs, debuffs, and shields more than those that do not.")]
    //[Range(0, 5)] [SerializeField] private int applicationWeight;

    //[Tooltip("A value range between 0 and 5 that represents the AI ability to accurately defend against attacks. A value of 5 means that the AI will make an " +
    //    "accurate guess as to where to defend given the available information, whereas a value of 0 will randomly guess from the channels able to be guarded.")]
    //[Range(0, 5)] [SerializeField] private int defensivePredictionAccuracy;

    //[Tooltip("A value range between 0 and 5 that represents the AI ability to accurately choose attacks that cannot be guarded or countered. A value of 5 " +
    //    "means that the AI will value attacks that cannot be guarded or countered more where a value of 0 means that the AI will not consider the player's " +
    //    "defensive cards when picking where to attack.")]
    //[Range(0, 5)] [SerializeField] private int offensivePredictionAccuracy;


    private List<CardDataObject> opponentHand;
    private CardChannelPairObject attackA;
    private CardChannelPairObject attackB;
    private string aICombatLog = "";

    private void Start()
    {
        CombatManager.OnStartNewTurn += BuildCardChannelPairA;
        ChannelsUISlotManager.OnASlotFilled += BuildCardChannelPairB;
        CardPlayManager.OnBeginCardPlay += FinalCheck;
    }

    private void OnDestroy()
    {
        CombatManager.OnStartNewTurn -= BuildCardChannelPairA;
        ChannelsUISlotManager.OnASlotFilled -= BuildCardChannelPairB;
        CardPlayManager.OnBeginCardPlay -= FinalCheck;
    }

    private void BuildCardChannelPairA()
    {
        if (attackA != null)
            return;

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;

        List<CardPlayPriorityObject> cardPlays = GetCurrentPossibleCards(true);
        CardDataObject selectedCard;

        WeightCardPlayValues(cardPlays);

        if (cardPlays.Count == 0)
        {
            selectedCard = null;

            attackA = new CardChannelPairObject(selectedCard, Channels.None);
        }
        else
        {
            int highestCardPriority = 0;
            int highestCardIndex = 0;

            for (int i = 0; i < cardPlays.Count; i++)
            {
                if (cardPlays[i].priority > highestCardPriority)
                {
                    highestCardPriority = cardPlays[i].priority;
                    highestCardIndex = i;
                }
            }

            attackA = new CardChannelPairObject(cardPlays[highestCardIndex].card, cardPlays[highestCardIndex].channel);
            attackA.CardData.SelectedChannels = attackA.CardChannel;

            CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Opponent, attackA.CardData.EnergyCost);

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Opponent selected " + attackA.CardData.CardName + " for their A Slot.");
        }
    }

    private void BuildCardChannelPairB()
    {
        if (attackB != null)
            return;

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;

        List<CardPlayPriorityObject> cardPlays = GetCurrentPossibleCards(false);
        CardDataObject selectedCard;

        WeightCardPlayValues(cardPlays);

        if (cardPlays.Count == 0)
        {
            selectedCard = null;

            attackB = new CardChannelPairObject(selectedCard, Channels.None);
        }
        else
        {
            int highestCardPriority = 0;
            int highestCardIndex = 0;

            for (int i = 0; i < cardPlays.Count; i++)
            {
                if (cardPlays[i].priority > highestCardPriority)
                {
                    highestCardPriority = cardPlays[i].priority;
                    highestCardIndex = i;
                }
            }

            attackB = new CardChannelPairObject(cardPlays[highestCardIndex].card, cardPlays[highestCardIndex].channel);
            attackB.CardData.SelectedChannels = attackB.CardChannel;

            if(attackA != null && attackA.CardData != null)
                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Opponent, attackA.CardData.EnergyCost + attackB.CardData.EnergyCost);
            else
                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Opponent, attackB.CardData.EnergyCost);

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Opponent selected " + attackB.CardData.CardName + " for their B Slot.");
        }
    }

    private void WeightCardPlayValues(List<CardPlayPriorityObject> cardPlays)
    {
        WeightPriorityWithAggressiveness(cardPlays);
        WeightPriorityWithDefensiveness(cardPlays);
        WeightPriorityWithDamage(cardPlays);
        WeightPriorityWithComponentDamage(cardPlays);
        WeightPriorityWithTargetWeight(cardPlays);
        WeightPriorityWithRanzomization(cardPlays);

        if (CombatManager.instance.NarrateAIDecisionMaking) 
            Debug.Log(aICombatLog);
    }

    private void FinalCheck()
    {
        if (attackA == null)
        {
            Debug.Log("AI failed to build Attack A in time.");
            BuildCardChannelPairA();
        }

        if (attackB == null)
        {
            Debug.Log("AI failed to build Attack B in time.");
            BuildCardChannelPairB();
        }

        CombatManager.instance.CardPlayManager.BuildOpponentAttackPlan(attackA, attackB);

        attackA = null;
        attackB = null;
    }

    private List<CardPlayPriorityObject> GetCurrentPossibleCards(bool aSlot)
    {
        aICombatLog += "AI is card plays hand.";

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        List<CardPlayPriorityObject> cardPlays = new List<CardPlayPriorityObject>();

        foreach (CardDataObject card in opponentHand)
        {
            foreach(Channels channel in CombatManager.instance.GetChannelListFromFlags(card.PossibleChannels))
            {
                if (CombatManager.instance.EffectManager.GetIceElementInChannel(channel, CharacterSelect.Player))
                    if (card.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier > CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
                        continue;

                if (aSlot && (card.CardType == CardType.Attack || card.CardType == CardType.Neutral))
                    cardPlays.Add(CreateCardPlayPriorityObject(card, channel));

                if (!aSlot && (card.CardType == CardType.Defense || card.CardType == CardType.Neutral) && card != attackA.CardData)
                    cardPlays.Add(CreateCardPlayPriorityObject(card, channel));
            }
        }

        aICombatLog += "\nAI found " + cardPlays.Count + " possible cards to play:";

        foreach (CardPlayPriorityObject cardPlay in cardPlays)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;

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
        FighterEffectObject playerEffectObject = CombatManager.instance.EffectManager.PlayerEffects;
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
                                                            / maximumComponentDamage) * cDMWeight);
        }

        float GetComponentDamageMultiplier(CardPlayPriorityObject cardPlayPriorityObject)
        {
            CardDataObject cardData = cardPlayPriorityObject.card;

            float componentDamageMultiplier = 1;

            switch (cardData.CardCategory)
            {
                case CardCategory.Punch:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechArms.CDMFromComponent;
                    break;

                case CardCategory.Kick:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechLegs.CDMFromComponent;
                    break;

                case CardCategory.Special:
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechHead.CDMFromComponent;
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
        MechObject playerFighter = CombatManager.instance.PlayerFighter.FighterMech;

        List<MechComponentDataObject> componentList = new List<MechComponentDataObject>();
        componentList.Add(playerFighter.MechArms);
        componentList.Add(playerFighter.MechTorso);
        componentList.Add(playerFighter.MechLegs);

        componentList = componentList.OrderBy(o => o.ComponentCurrentHP).ToList();

        Channels highPriority = Channels.None;
        Channels midPriority = Channels.None; 
        Channels lowPriority = Channels.None;

        for (int i = 0; i < componentList.Count; i++)
        {
            switch (componentList[i].ComponentType)
            {
                case MechComponent.Torso:
                    if (i == 0)
                        highPriority = Channels.Mid;
                    if (i == 1)
                        midPriority = Channels.Mid;
                    if (i == 2)
                        lowPriority = Channels.Mid;
                    break;

                case MechComponent.Arms:
                    if (i == 0)
                        highPriority = Channels.High;
                    if (i == 1)
                        midPriority = Channels.High;
                    if (i == 2)
                        lowPriority = Channels.High;
                    break;

                case MechComponent.Legs:
                    if (i == 0)
                        highPriority = Channels.Low;
                    if (i == 1)
                        midPriority = Channels.Low;
                    if (i == 2)
                        lowPriority = Channels.Low;
                    break;
            }

        }

        foreach (CardPlayPriorityObject card in cardPlayPriorityObjects)
        {
            if (card.channel == highPriority)
                card.priority += componentHealthWeight;
            if (card.channel == midPriority)
                card.priority += Mathf.RoundToInt(componentHealthWeight / 2);
            if (card.channel == lowPriority)
                continue;
        }
    }

    private void WeightPriorityWithRanzomization(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        foreach(CardPlayPriorityObject cardPlayPrio in cardPlayPriorityObjects)
        {
            cardPlayPrio.priority += Mathf.RoundToInt(UnityEngine.Random.Range(-randomizationWeight, randomizationWeight));
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

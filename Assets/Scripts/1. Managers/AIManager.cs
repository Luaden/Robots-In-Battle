using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private int randomizationWeight;
    private int aggressivenessWeight;
    private int defensivenessWeight;
    private int baseDamageWeight;
    private int cDMWeight;
    private int componentHealthWeight;
    private int benefitWeight;
    private int mitigationWeight;
    private int applicationWeight;
    private int defensivePredictionAccuracy;
    private int offensivePredictionAccuracy;
    //Need some way to weigh energy consumption as well

    private AIDialogueController aIDialogueController;

    private List<CardDataObject> opponentHand;
    private CardChannelPairObject attackA;
    private CardChannelPairObject attackB;
    private string aICombatLog = "";

    #region Utility
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

    public void LoadAIBehaviorModule(SOAIBehaviorObject newBehavior)
    {
        randomizationWeight = newBehavior.RandomizationWeight;
        aggressivenessWeight = newBehavior.AggressivenessWeight;
        defensivenessWeight = newBehavior.DefensivenessWeight;
        baseDamageWeight = newBehavior.BaseDamageWeight;
        cDMWeight = newBehavior.CDMWeight;
        componentHealthWeight = newBehavior.ComponentHealthWeight;
    }

    public void LoadAIDialogueModule(SOAIDialogueObject newDialogue)
    {
        aIDialogueController.LoadCombatDialogue(newDialogue);
    }

    public void PlayAIIntroDialogue()
    {
        aIDialogueController.PlayIntroDialogue();
    }

    public void PlayAIWinDialogue()
    {
        aIDialogueController.PlayAIWinDialogue();
    }

    public void PlayAILoseDialogue()
    {
        aIDialogueController.PlayAILoseDialogue();
    }

    private void Start()
    {
        aIDialogueController = GetComponent<AIDialogueController>();
        CombatManager.OnStartNewTurn += BuildCardChannelPairA;
        ChannelsUISlotManager.OnSlotFilled += BuildCardChannelPairB;
        CardPlayManager.OnBeginCardPlay += FinalCheck;
    }

    private void OnDestroy()
    {
        CombatManager.OnStartNewTurn -= BuildCardChannelPairA;
        ChannelsUISlotManager.OnSlotFilled -= BuildCardChannelPairB;
        CardPlayManager.OnBeginCardPlay -= FinalCheck;
    }

    private void OnCombatComplete()
    {
        aIDialogueController.CheckPlayDialogue();
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
        aICombatLog += "AI is checking card plays hand.";

        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        List<CardPlayPriorityObject> cardPlays = new List<CardPlayPriorityObject>();

        foreach (CardDataObject card in opponentHand)
        {
            foreach(Channels channel in CombatManager.instance.GetChannelListFromFlags(card.PossibleChannels))
            {
                if (CombatManager.instance.EffectManager.GetIceElementInChannel(channel, CharacterSelect.Player))
                {
                    if (card.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier > CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
                        continue;
                }                    
                else
                    if (card.EnergyCost > CombatManager.instance.OpponentFighter.FighterMech.MechCurrentEnergy)
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

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards with aggression:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
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
                    cardPrioPair.priority += defensivenessWeight;
                    break;
                case CardType.Neutral:
                    foreach (SOCardEffectObject cardEffect in cardPrioPair.card.CardEffects)
                        if (CardEffectTypes.Defensive.HasFlag(cardEffect.EffectType))
                        {
                            cardPrioPair.priority += defensivenessWeight;
                            break;
                        }
                    break;
            }
        }

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards with defensiveness:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
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

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards with base damage:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
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
                    componentDamageMultiplier += CombatManager.instance.OpponentFighter.FighterMech.MechTorso.CDMFromComponent;
                    break;
            }

            if (playerEffectObject.IceAcidStacks.TryGetValue(cardPlayPriorityObject.channel, out channelElementStacks))
            {
                foreach (ElementStackObject element in channelElementStacks)
                    if (element.ElementType == ElementType.Acid)
                        componentDamageMultiplier += CombatManager.instance.AcidComponentDamageMultiplier;
            }

            return componentDamageMultiplier;
        }

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards by Acid applied and CDM of equipped components:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
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

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards by ability to target weaker components on player:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
    }

    private void WeightPriorityWithRanzomization(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        foreach(CardPlayPriorityObject cardPlayPrio in cardPlayPriorityObjects)
        {
            cardPlayPrio.priority += Mathf.RoundToInt(UnityEngine.Random.Range(-randomizationWeight, randomizationWeight));
        }

        aICombatLog += "\nAI is adding " + randomizationWeight + " randomization weight to " + cardPlayPriorityObjects.Count + " cards:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
    }

    private void WeightPriorityWithBenefit(List<CardPlayPriorityObject> cardPlayPriorityObjects)
    {
        foreach(Channels channel in CombatManager.instance.GetChannelListFromFlags(Channels.All))
        {
            //Check for channel damage buffs
            //Check for card category buffs
        }

        foreach (CardPlayPriorityObject cardPrioPair in cardPlayPriorityObjects)
        {
            foreach(SOCardEffectObject effect in cardPrioPair.card.CardEffects)
            {
                //if(effect.EffectType.HasFlag(CardEffectTypes.))
                //Check for flurry buff
            }
        }

        aICombatLog += "\nAI is weighing " + cardPlayPriorityObjects.Count + " cards that benefit from buffs:";

        foreach (CardPlayPriorityObject cardPlay in cardPlayPriorityObjects)
            aICombatLog += "\nCard: " + cardPlay.card.CardName + "." + "\nChannel: " + cardPlay.channel + "." + "\nCard Weight: " + cardPlay.priority;
    }
}
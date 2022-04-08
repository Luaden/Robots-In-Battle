using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Tooltip("A value range between -1 and 1 that represents the AI preference for either offense or defense. " +
        "A value of -1 means the AI will always use defensive cards unless none are available, whereas a value of 1 means the AI will always use attack cards " +
        "unless there are none are available.")]
    [Range(-1, 1)][SerializeField] private float aggressiveness;
    [Tooltip("A value range between -1 and 1 that represents the AI preference for raw damage or targeting components. " +
        "A value of -1 means the AI will primarily target weaker components to break them, whereas a value of 1 means the AI will always prioritize higher " +
        "base damage for attacks.")]
    [Range(-1, 1)][SerializeField] private float precision;
    [Tooltip("A value range between -1 and 1 that represents the AI ability to choose the best option given its other " +
        "behavioral traits. A value of -1 means that the AI will always randomly pick actions, whereas a value of 1 means that the AI will always choose the " +
        "best choice that fits its other behavioral traits.")]
    [Range(-1, 1)][SerializeField] private float intelligence;

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
    }

    private void OnDestroy()
    {
        ChannelsUISlotManager.OnASlotFilled -= BuildCardChannelPairA;
        ChannelsUISlotManager.OnBSlotFilled -= BuildCardChannelPairB;
    }

    private void BuildCardChannelPairA()
    {
        opponentHand = CombatManager.instance.HandManager.OpponentHand.CharacterHand;
        CardDataObject selectedCard = opponentHand[UnityEngine.Random.Range(0, opponentHand.Count)];

        attackA = new CardChannelPairObject(selectedCard, GetRandomChannelFromFlag(selectedCard.PossibleChannels));

        CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(selectedCard.CardUIObject.GetComponent<CardUIController>(), null);
    }

    private void BuildCardChannelPairB()
    {
        CardDataObject selectedCard = opponentHand[UnityEngine.Random.Range(0, opponentHand.Count)];

        attackB = new CardChannelPairObject(selectedCard, GetRandomChannelFromFlag(selectedCard.PossibleChannels));

        CombatManager.instance.ChannelsUISlotManager.OpponentAssignAttackSlot(selectedCard.CardUIObject.GetComponent<CardUIController>(), null);
        CombatManager.instance.CardPlayManager.BuildOpponentAttackPlan(attackA, attackB);
    }

    private Channels GetRandomChannelFromFlag(Channels channel)
    {
        System.Random random = new System.Random();

        Channels[] allChannels = Enum.GetValues(typeof(Channels)).Cast<Channels>().Where(x => channel.HasFlag(x)).ToArray();
        Channels randomChannel = allChannels[random.Next(1, allChannels.Length)];

        return randomChannel;
    }
}

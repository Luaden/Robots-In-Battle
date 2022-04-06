using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChannelsUISlotManager : BaseSlotManager<CardUIController>
{
    [SerializeField] private List<BaseSlotController<CardUIController>> highChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> midChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> lowChannels;

    [SerializeField] private BaseSlotController<CardUIController> firstAttackPosition;
    [SerializeField] private BaseSlotController<CardUIController> secondAttackPosition;

    private List<CardChannelPairObject> cardChannelPairObjects;

    public override void AddItemToCollection(CardUIController item)
    {
        foreach (CardUISlotController slot in slotList)
            if (slot.CurrentSlottedItem == null)
            {
                slot.CurrentSlottedItem = item;
                item.CardSlotController = slot;
                CombatManager.instance.HandManager.AddCardToPlayerHand(item.CardData);
                return;
            }

        if(firstAttackPosition.CurrentSlottedItem == null)
        {
            firstAttackPosition.CurrentSlottedItem = item;
            item.CardSlotController = firstAttackPosition;
            return;
        }

        if (secondAttackPosition.CurrentSlottedItem == null)
        {
            secondAttackPosition.CurrentSlottedItem = item;
            item.CardSlotController = secondAttackPosition;
            return;
        }

        //We also need to update the "SelectedChannel" for the card here.

        Debug.Log("No slots available in the hand to add a card to. This should not happen and should be stopped before this point.");
    }

    public override void AddSlotToList(BaseSlotController<CardUIController> newSlot)
    {
        if (slotList == null)
            slotList = new List<BaseSlotController<CardUIController>>();

        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, CardUIController newData, BaseSlotController<CardUIController> slot)
    {
        Channels selectedChannel = CheckChannelSlot(slot);

        Debug.Log("Selected channel is " + selectedChannel);

        if(CardChannelCheck(newData, selectedChannel))
        {
            newData.CardSlotController.SlotManager.RemoveItemFromCollection(newData);
            AddItemToCollection(newData);
            return;
        }

        Debug.Log("Card does not fit into " + selectedChannel + " slot.");
    }

    public override void RemoveItemFromCollection(CardUIController item)
    {
        if (firstAttackPosition.CurrentSlottedItem == item)
        {
            firstAttackPosition.CurrentSlottedItem = null;
            //We also need to update the "SelectedChannel" for the card here.

            return;
        }

        if (secondAttackPosition.CurrentSlottedItem == item)
        {
            secondAttackPosition.CurrentSlottedItem = null;
            //We also need to update the "SelectedChannel" for the card here.

            return;
        }
    }

    private AttackPlanObject BuildAttackPlanObject()
    {
        // testing
        CharacterSelect destination = CharacterSelect.Player;
        CharacterSelect origin = CharacterSelect.Opponent;

        // create object
        AttackPlanObject attackPlanObject = new AttackPlanObject(cardChannelPairObjects, origin, destination);

        // ends turn

        //send object to CardPlayManager
        return attackPlanObject;
    }

    private Channels CheckChannelSlot(BaseSlotController<CardUIController> slotToCheck)
    {
        if (highChannels.Contains(slotToCheck))
            return Channels.High;
        if (midChannels.Contains(slotToCheck))
            return Channels.Mid;
        if (lowChannels.Contains(slotToCheck))
            return Channels.Low;

        return Channels.None;
    }

    private bool CardChannelCheck(CardUIController card, Channels selectedChannel)
    {
        if (card.CardData.PossibleChannels == Channels.All)
            return true;

        if (selectedChannel == Channels.High)
            if (card.CardData.PossibleChannels == Channels.High || card.CardData.PossibleChannels == Channels.HighLow ||
                card.CardData.PossibleChannels == Channels.HighMid)
                return true;

        if (selectedChannel == Channels.Mid)
            if (card.CardData.PossibleChannels == Channels.Mid || card.CardData.PossibleChannels == Channels.HighMid ||
                card.CardData.PossibleChannels == Channels.MidLow)
                return true;

        if (selectedChannel == Channels.Low)
            if (card.CardData.PossibleChannels == Channels.Low || card.CardData.PossibleChannels == Channels.HighLow ||
                card.CardData.PossibleChannels == Channels.MidLow)
                return true;

        return false;
    }
}

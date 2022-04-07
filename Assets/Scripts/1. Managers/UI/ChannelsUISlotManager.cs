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
    private Channels selectedChannel;

    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(firstAttackPosition.CurrentSlottedItem == null)
        {
            firstAttackPosition.CurrentSlottedItem = item;
            item.CardSlotController = firstAttackPosition;
            item.CardData.SelectedChannels = selectedChannel;
            cardChannelPairObjects[0] = new CardChannelPairObject(item.CardData, selectedChannel);
            return;
        }

        if (secondAttackPosition.CurrentSlottedItem == null)
        {
            secondAttackPosition.CurrentSlottedItem = item;
            item.CardSlotController = secondAttackPosition;
            item.CardData.SelectedChannels = selectedChannel;
            cardChannelPairObjects[1] = new CardChannelPairObject(item.CardData, selectedChannel);
            return;
        }

        if (firstAttackPosition.CurrentSlottedItem != null && secondAttackPosition.CurrentSlottedItem != null)
            CombatManager.instance.CardPlayManager.BuildPlayerAttackPlan(cardChannelPairObjects);

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
        selectedChannel = CheckChannelSlot(slot);

        if(CardChannelCheck(newData, selectedChannel))
        {
            newData.CardSlotController.SlotManager.RemoveItemFromCollection(newData);
            AddItemToCollection(newData, slot);
            return;
        }

        Debug.Log("Card does not fit into " + selectedChannel + " slot.");
    }

    public override void RemoveItemFromCollection(CardUIController item)
    {
        if (firstAttackPosition.CurrentSlottedItem == item)
        {
            firstAttackPosition.CurrentSlottedItem = null;
            item.CardData.SelectedChannels = Channels.None;
            return;
        }

        if (secondAttackPosition.CurrentSlottedItem == item)
        {
            secondAttackPosition.CurrentSlottedItem = null;
            item.CardData.SelectedChannels = Channels.None;
            return;
        }
    }

    private void Start()
    {
        cardChannelPairObjects = new List<CardChannelPairObject>(2);
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

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

    public CardDataObject ASlotItem { get => firstAttackPosition.CurrentSlottedItem.CardData; } 
    public CardDataObject BSlotItem { get => firstAttackPosition.CurrentSlottedItem.CardData; }

    public delegate void onASlotFilled();
    public static event onASlotFilled OnASlotFilled;
    public delegate void onBSlotFilled();
    public static event onBSlotFilled OnBSlotFilled;

    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(firstAttackPosition.CurrentSlottedItem == null)
            if(item.CardData.CardType == CardType.Attack || item.CardData.CardType == CardType.Neutral)
            {
                firstAttackPosition.CurrentSlottedItem = item;
                item.CardSlotController = firstAttackPosition;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel;

                cardChannelPairObjects[0] = new CardChannelPairObject(item.CardData, selectedChannel);

                OnASlotFilled?.Invoke();
                return;
            }
            else
            {
                Debug.Log("This card is not the correct type for Slot A. Slot A must be an Attack or Neutral Card Type.");
                return;
            }

        if (secondAttackPosition.CurrentSlottedItem == null)
            if (item.CardData.CardType == CardType.Defense || item.CardData.CardType == CardType.Neutral)
            {
                secondAttackPosition.CurrentSlottedItem = item;
                item.CardSlotController = secondAttackPosition;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel; cardChannelPairObjects[1] = new CardChannelPairObject(item.CardData, selectedChannel);

                OnBSlotFilled?.Invoke();
                return;
            }
            else
            {
                Debug.Log("This card is not the correct type for Slot B. Slot A must be an Defense or Neutral Card Type.");
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

        if (selectedChannel == Channels.None)
            return;

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

    public void ClearChannelUICards()
    {
        RemoveItemFromCollection(firstAttackPosition.CurrentSlottedItem);
        RemoveItemFromCollection(secondAttackPosition.CurrentSlottedItem);
    }

    private void Start()
    {
        cardChannelPairObjects = new List<CardChannelPairObject>(2);
        cardChannelPairObjects.Add(null);
        cardChannelPairObjects.Add(null);
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

        if (card.CardData.PossibleChannels.HasFlag(Channels.All))
            return true;

        //If we're checking the high channel, bitwise check if PossibleChannels results in Channels.High
        if (selectedChannel == Channels.High)
            if (card.CardData.PossibleChannels.HasFlag(Channels.High))
                return true;

        if (selectedChannel == Channels.Mid)
            if (card.CardData.PossibleChannels.HasFlag(Channels.Mid))
                return true;

        if (selectedChannel == Channels.Low)
            if (card.CardData.PossibleChannels.HasFlag(Channels.Low))
                return true;

        return false;
    }
}

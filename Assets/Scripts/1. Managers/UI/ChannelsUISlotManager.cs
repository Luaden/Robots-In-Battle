using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChannelsUISlotManager : BaseSlotManager<CardUIController>
{
    [SerializeField] private List<BaseSlotController<CardUIController>> highChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> midChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> lowChannels;

    [SerializeField] private BaseSlotController<CardUIController> playerAttackSlotA;
    [SerializeField] private BaseSlotController<CardUIController> playerAttackSlotB;
    [SerializeField] private BaseSlotController<CardUIController> opponentAttackSlotA;
    [SerializeField] private BaseSlotController<CardUIController> opponentAttackSlotB;

    [SerializeField] private GameObject SkipASlotButton;

    private CardChannelPairObject cardChannelPairObjectA;
    private CardChannelPairObject cardChannelPairObjectB;
    private Channels selectedChannel;

    public CardDataObject ASlotItem { get => playerAttackSlotA.CurrentSlottedItem.CardData; } 
    public CardDataObject BSlotItem { get => playerAttackSlotA.CurrentSlottedItem.CardData; }

    public delegate void onASlotFilled();
    public static event onASlotFilled OnASlotFilled;
    public delegate void onBSlotFilled();
    public static event onBSlotFilled OnBSlotFilled;

    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(playerAttackSlotA.CurrentSlottedItem == null)
            if(item.CardData.CardType == CardType.Attack || item.CardData.CardType == CardType.Neutral)
            {
                playerAttackSlotA.CurrentSlottedItem = item;
                item.CardSlotController = playerAttackSlotA;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel;

                cardChannelPairObjectA = new CardChannelPairObject(item.CardData, selectedChannel);
                CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA = cardChannelPairObjectA;
                SkipASlotButton.SetActive(false);
                OnASlotFilled?.Invoke();
                return;
            }
            else
            {
                Debug.Log("This card is not the correct type for Slot A. Slot A must be an Attack or Neutral Card Type.");
                return;
            }

        if (playerAttackSlotB.CurrentSlottedItem == null)
            if (item.CardData.CardType == CardType.Defense || item.CardData.CardType == CardType.Neutral)
            {
                playerAttackSlotB.CurrentSlottedItem = item;
                item.CardSlotController = playerAttackSlotB;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel; 
                
                cardChannelPairObjectB = new CardChannelPairObject(item.CardData, selectedChannel);
                CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairB = cardChannelPairObjectB;

                OnBSlotFilled?.Invoke();
                return;
            }
            else
            {
                Debug.Log("This card is not the correct type for Slot B. Slot A must be an Defense or Neutral Card Type.");
                return;
            }

        Debug.Log("No slots available in the hand to add a card to. This should not happen and should be stopped before this point.");
    }

    public void SkipASlot()
    {
        CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA = 
            new CardChannelPairObject(null, Channels.None);

        OnASlotFilled?.Invoke();

        SkipASlotButton.SetActive(false);
    }

    public void OpponentAssignAttackSlot(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        slot.SlotManager.RemoveItemFromCollection(item);

        if (opponentAttackSlotA.CurrentSlottedItem == null)
            if (item.CardData.CardType == CardType.Attack || item.CardData.CardType == CardType.Neutral)
            {
                opponentAttackSlotA.CurrentSlottedItem = item;
                item.CardSlotController = opponentAttackSlotA;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel;

                return;
            }
            else
            {
                Debug.Log("This card is not the correct type for Slot A. Slot A must be an Attack or Neutral Card Type.");
                return;
            }

        if (opponentAttackSlotB.CurrentSlottedItem == null)
            if (item.CardData.CardType == CardType.Defense || item.CardData.CardType == CardType.Neutral)
            {
                opponentAttackSlotB.CurrentSlottedItem = item;
                item.CardSlotController = opponentAttackSlotB;

                if (item.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
                    item.CardData.SelectedChannels = item.CardData.PossibleChannels;
                else
                    item.CardData.SelectedChannels = selectedChannel; 

                return;
            }
            else
            {
                Debug.Log(item.CardData.CardName + " is not the correct type for Slot B. Slot A must be an Defense or Neutral Card Type.");
                return;
            }

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
        if (newData.CardData.EnergyCost > CombatManager.instance.PlayerFighter.FighterMech.MechCurrentEnergy)
        {
            Debug.Log("Not enough energy to use this card.");
            return;
        }

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
        if (playerAttackSlotA.CurrentSlottedItem == item)
        {
            playerAttackSlotA.CurrentSlottedItem = null;
            item.CardData.SelectedChannels = Channels.None;
            return;
        }

        if (playerAttackSlotB.CurrentSlottedItem == item)
        {
            playerAttackSlotB.CurrentSlottedItem = null;
            item.CardData.SelectedChannels = Channels.None;
            return;
        }
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

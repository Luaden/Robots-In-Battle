using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpponentHandUISlotManager : BaseSlotManager<CardUIController>
{
    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(slot != null && slot.CurrentSlottedItem == null)
        {
            CombatManager.instance.HandManager.AddCardToOpponentHand(item.CardData);
            slot.CurrentSlottedItem = item;
            item.CardSlotController = slot;
            return;
        }

        foreach (BaseSlotController<CardUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                CombatManager.instance.HandManager.AddCardToOpponentHand(item.CardData);
                slotOption.CurrentSlottedItem = item;
                item.CardSlotController = slotOption;
                return;
            }

        Debug.Log("No slots available in the hand to add a card to. This should not happen and should be stopped before this point.");
    }

    public override void RemoveItemFromCollection(CardUIController item)
    {
        foreach (CardUISlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                slot.CurrentSlottedItem = null;
                CombatManager.instance.HandManager.RemoveCardFromOpponentHand(item.CardData);
                return;
            }

        Debug.Log("Could not find the CardUIController for the item: " + item.CardData.CardName);
    }

    public override void HandleDrop(PointerEventData eventData, CardUIController droppedCard, BaseSlotController<CardUIController> slot)
    {
        if (droppedCard.CardSlotController.SlotManager == CombatManager.instance.ChannelsUISlotManager ||
            droppedCard.CardSlotController.SlotManager == CombatManager.instance.PlayerHandSlotManager)
        {
            CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.OpponentFighter.FighterName, 
                                                              "Uh... I really don't think I'm allowed to take your cards.",
                                                              CharacterSelect.Opponent);
            return;
        }

        Debug.Log("Dropping Opponent Card");
        if (droppedCard == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            return;
        }

        droppedCard.CardSlotController.SlotManager.RemoveItemFromCollection(droppedCard);
        AddItemToCollection(droppedCard, slot);
    }

    public override void AddSlotToList(BaseSlotController<CardUIController> newSlot)
    {
        slotList.Add(newSlot);
    }
}


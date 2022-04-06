using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpponentHandUISlotManager : BaseSlotManager<CardUIController>
{
    public override void AddItemToCollection(CardUIController item)
    {
        foreach (CardUISlotController slot in slotList)
            if (slot.CurrentSlottedItem == null)
            {
                slot.CurrentSlottedItem = item;
                item.CardSlotController = slot;
                CombatManager.instance.HandManager.AddCardToOpponentHand(item.CardData);
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
            }
    }

    public override void HandleDrop(PointerEventData eventData, CardUIController droppedCard, BaseSlotController<CardUIController> slot)
    {
        if (droppedCard == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            //Tell card to move to previous slot.
            return;
        }

        droppedCard.CardSlotController.SlotManager.RemoveItemFromCollection(droppedCard);
        AddItemToCollection(droppedCard);
    }

    public override void AddSlotToList(BaseSlotController<CardUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    private void Awake()
    {
        slotList = new List<BaseSlotController<CardUIController>>();
    }
}


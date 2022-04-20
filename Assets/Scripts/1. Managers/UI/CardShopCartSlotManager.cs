using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShopCartSlotManager : BaseSlotManager<CardShopCartUIController>
{
    public override void AddItemToCollection(CardShopCartUIController item, BaseSlotController<CardShopCartUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.CardShopCartSlotController = slot;
            //OnItemAdded(slot);
            return;
        }
    }

    public override void AddSlotToList(BaseSlotController<CardShopCartUIController> newSlot)
    {
        SlotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, CardShopCartUIController newData, BaseSlotController<CardShopCartUIController> slot)
    {
        if (newData == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            return;
        }

        newData.CardShopCartSlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(CardShopCartUIController item)
    {
        foreach (CardShopCartSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                //onItemRemoved(slot);
                slot.CurrentSlottedItem = null;
            }
    }
}

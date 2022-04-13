using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSlotManager : BaseSlotManager<ShopItemUIController>
{
    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.ShopItemUISlotController = slot;
            
            return;
        }

        foreach (BaseSlotController<ShopItemUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.ShopItemUISlotController = slotOption;

                return;
            }

        Debug.Log("No slots available in the hand to add a card to. This should not happen and should be stopped before this point.");
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        if (newData == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            //Tell card to move to previous slot.
            return;
        }

        newData.ShopItemUISlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        foreach (ShopItemSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
                slot.CurrentSlottedItem = null;
    }
}

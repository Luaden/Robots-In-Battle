using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartSlotManager : BaseSlotManager<ShopCartItemController>
{
    public event Action<BaseSlotController<ShopCartItemController>> onItemAdded;
    public event Action<BaseSlotController<ShopCartItemController>> onItemRemoved;
    public override void AddItemToCollection(ShopCartItemController item, BaseSlotController<ShopCartItemController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.ShopCartItemSlotController = slot;
            onItemAdded(slot);
            return;
        }

/*        foreach (BaseSlotController<ShopCartItemController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.ShopCartItemSlotController = slotOption;

                return;
            }*/

        Debug.Log("No slots available to add item to");
    }

    public override void AddSlotToList(BaseSlotController<ShopCartItemController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopCartItemController newData, BaseSlotController<ShopCartItemController> slot)
    {
        if (newData == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            return;
        }
        newData.ShopCartItemSlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopCartItemController item)
    {
        foreach (ShopCartItemSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                onItemRemoved(slot);
                slot.CurrentSlottedItem = null;
            }
    }
   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotManager : BaseSlotManager<InventoryUIController>
{
    public override void AddItemToCollection(InventoryUIController item, BaseSlotController<InventoryUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.InventorySlotController = slot;
            return;
        }
        foreach (BaseSlotController<InventoryUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.InventorySlotController = slotOption;
                return;
            }
    }

    public override void AddSlotToList(BaseSlotController<InventoryUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, InventoryUIController newData, BaseSlotController<InventoryUIController> slot)
    {
        if (newData == null)
            return;

        Debug.Log(newData.InventorySlotController);
        newData.InventorySlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(InventoryUIController item)
    {
        foreach(InventorySlotController slot in slotList)
            if(item == slot.CurrentSlottedItem)
                slot.CurrentSlottedItem = null;
    }
}

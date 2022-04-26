using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotManager : BaseSlotManager<EquipmentUIController>
{
    public override void AddItemToCollection(EquipmentUIController item, BaseSlotController<EquipmentUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.EquipmentSlotController = slot;
            return;
        }
    }

    public override void AddSlotToList(BaseSlotController<EquipmentUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, EquipmentUIController newData, BaseSlotController<EquipmentUIController> slot)
    {
        if (newData == null)
            return;

        newData.EquipmentSlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);

    }

    public override void RemoveItemFromCollection(EquipmentUIController item)
    {
        foreach (EquipmentSlotController slot in slotList)
            if (item == slot.CurrentSlottedItem)
                slot.CurrentSlottedItem = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentShopCartSlotManager : BaseSlotManager<ComponentShopCartUIController>
{
    public override void AddItemToCollection(ComponentShopCartUIController item, BaseSlotController<ComponentShopCartUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            item.ComponentShopSlotUIController = slot;
            slot.CurrentSlottedItem = item;
        }

        foreach (BaseSlotController<ComponentShopCartUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.ComponentShopSlotUIController = slotOption;

                return;
            }
    }

    public override void AddSlotToList(BaseSlotController<ComponentShopCartUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ComponentShopCartUIController newData, BaseSlotController<ComponentShopCartUIController> slot)
    {
        if (newData == null)
            return;

        newData.ComponentShopSlotUIController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ComponentShopCartUIController item)
    {
        foreach(ComponentShopCartSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
                slot.CurrentSlottedItem = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentShopVendorSlotManager : BaseSlotManager<ComponentShopVendorUIController>
{
    public override void AddItemToCollection(ComponentShopVendorUIController item, BaseSlotController<ComponentShopVendorUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.ComponentShopSlotUIController = slot;
            return;
        }
        foreach (BaseSlotController<ComponentShopVendorUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.ComponentShopSlotUIController = slotOption;

                return;
            }
    }

    public override void AddSlotToList(BaseSlotController<ComponentShopVendorUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ComponentShopVendorUIController newData, BaseSlotController<ComponentShopVendorUIController> slot)
    {
        if (newData == null)
            return;
        newData.ComponentShopSlotUIController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ComponentShopVendorUIController item)
    {
        foreach (ComponentShopVendorSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
                slot.CurrentSlottedItem = null;
    }
}

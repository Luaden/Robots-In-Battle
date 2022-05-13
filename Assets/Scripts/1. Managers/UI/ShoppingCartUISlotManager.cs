using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShoppingCartUISlotManager : BaseSlotManager<ShopItemUIController>
{
    private List<ShopItemUIController> currentShoppingCartItems = new List<ShopItemUIController>();
    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> oldSlot)
    {
        if (oldSlot != null && oldSlot.CurrentSlottedItem == null)
        {
            item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);

            oldSlot.CurrentSlottedItem = item;
            item.ItemSlotController = oldSlot;

            currentShoppingCartItems.Add(item);
            DowntimeManager.instance.ShoppingCartManager.UpdateShoppingCartInventory(currentShoppingCartItems);
            return;
        }

        Debug.Log("No available slots to add to the shopping cart.");
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        if (newData == null)
            return;

        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            if (item == slot.CurrentSlottedItem)
            {
                slot.CurrentSlottedItem = null;
                currentShoppingCartItems.Remove(item);
                DowntimeManager.instance.ShoppingCartManager.UpdateShoppingCartInventory(currentShoppingCartItems);
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShopVendorSlotManager : BaseSlotManager<CardShopVendorUIController>
{
    public override void AddItemToCollection(CardShopVendorUIController item, BaseSlotController<CardShopVendorUIController> slot)
    {
        if(slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.CardShopVendorSlotController = slot;
            return;
        }
        foreach (BaseSlotController<CardShopVendorUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.CardShopVendorSlotController = slotOption;

                return;
            }
    }

    public override void AddSlotToList(BaseSlotController<CardShopVendorUIController> newSlot)
    {
        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, CardShopVendorUIController newData, BaseSlotController<CardShopVendorUIController> slot)
    {
        if (newData == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            //Tell card to move to previous slot.
            return;
        }

        newData.CardShopVendorSlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(CardShopVendorUIController item)
    {
        foreach (CardShopVendorSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
                slot.CurrentSlottedItem = null;
    }
}

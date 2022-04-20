using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentShopCartSlotController : BaseSlotController<ComponentShopCartUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        // is this slotted item already occupied?
        if (this.CurrentSlottedItem != null)
            return;

        // is our dragged object not a part of ShopCart?
        if (eventData.pointerDrag.GetComponent<ComponentShopCartUIController>() == null)
        {
            // is it a ShopItem?
            if (eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>() != null)
            {
                // add the ShopCart component to this object
                ComponentShopCartUIController item = eventData.pointerDrag.AddComponent<ComponentShopCartUIController>();
                item.ComponentShopSlotUIController = this;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ComponentShopCartUIController>(), this);
                // turn the shop item component off
                item.GetComponent<ComponentShopVendorUIController>().enabled = false;
            }
        }

        // does the dragged object have ShopCart but deactivated?
        if (!eventData.pointerDrag.GetComponent<ComponentShopCartUIController>().enabled)
        {
            ComponentShopCartUIController shopCartItem = eventData.pointerDrag.GetComponent<ComponentShopCartUIController>();
            shopCartItem.enabled = true;

            slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ComponentShopCartUIController>(), this);

            ComponentShopVendorUIController shopItem = shopCartItem.GetComponent<ComponentShopVendorUIController>();
            shopItem.enabled = false;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ComponentShopCartUIController>(), this);
    }
}

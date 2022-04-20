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

        if (eventData.pointerDrag.GetComponent<ComponentShopCartUIController>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }

        ComponentShopCartUIController shopCartItem = eventData.pointerDrag.GetComponent<ComponentShopCartUIController>();

        // does the dragged object have ShopCart but deactivated?
        if (!shopCartItem.enabled)
        {
            shopCartItem.enabled = true;
            shopCartItem.ComponentShopSlotUIController = this;

            ComponentShopVendorUIController vendorItem = shopCartItem.GetComponent<ComponentShopVendorUIController>();
            vendorItem.enabled = false;
        }

        slotManager.HandleDrop(eventData, shopCartItem, this);
    }
}

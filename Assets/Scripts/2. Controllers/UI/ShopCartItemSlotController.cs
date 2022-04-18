using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartItemSlotController : BaseSlotController<ShopCartItemController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        // is this slotted item already occupied?
        if(this.CurrentSlottedItem != null)
            return;

        // is our dragged object not a part of ShopCart?
        if (eventData.pointerDrag.GetComponent<ShopCartItemController>() == null)
        {
            // is it a ShopItem?
            if (eventData.pointerDrag.GetComponent<ShopItemUIController>() != null)
            {
                // add the ShopCart component to this object
                ShopCartItemController shopItem = eventData.pointerDrag.AddComponent<ShopCartItemController>();
                shopItem.ShopCartItemSlotController = this;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
                // turn the shop item component off
                shopItem.GetComponent<ShopItemUIController>().enabled = false;
            }
        }

        // does the dragged object have ShopCart but deactivated?
        if (!eventData.pointerDrag.GetComponent<ShopCartItemController>().enabled)
        {
            ShopCartItemController shopCartItem = eventData.pointerDrag.GetComponent<ShopCartItemController>();
            shopCartItem.enabled = true;

            slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);

            ShopItemUIController shopItem = shopCartItem.GetComponent<ShopItemUIController>();
            shopItem.enabled = false;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
    }
}

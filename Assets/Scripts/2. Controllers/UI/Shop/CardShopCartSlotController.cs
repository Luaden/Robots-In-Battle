using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShopCartSlotController : BaseSlotController<CardShopCartUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        // is this slotted item already occupied?
        if (this.CurrentSlottedItem != null)
            return;

        // is our dragged object not a part of ShopCart?
        if (eventData.pointerDrag.GetComponent<CardShopCartUIController>() == null)
        {
            // is it a ShopItem?
            if (eventData.pointerDrag.GetComponent<CardShopVendorUIController>() != null)
            {
                // add the ShopCart component to this object
                CardShopCartUIController item = eventData.pointerDrag.AddComponent<CardShopCartUIController>();
                item.CardShopCartSlotController = this;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardShopCartUIController>(), this);
                // turn the shop item component off
                item.GetComponent<CardShopVendorUIController>().enabled = false;
            }
        }

        // does the dragged object have ShopCart but deactivated?
        if (!eventData.pointerDrag.GetComponent<CardShopCartUIController>().enabled)
        {
            CardShopCartUIController shopCartItem = eventData.pointerDrag.GetComponent<CardShopCartUIController>();
            shopCartItem.enabled = true;

            slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardShopCartUIController>(), this);

            CardShopVendorUIController shopItem = shopCartItem.GetComponent<CardShopVendorUIController>();
            shopItem.enabled = false;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardShopCartUIController>(), this);
    }
}

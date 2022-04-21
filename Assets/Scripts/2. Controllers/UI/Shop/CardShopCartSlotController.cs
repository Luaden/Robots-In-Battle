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

        if (eventData.pointerDrag.GetComponent<CardShopCartUIController>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }

        CardShopCartUIController shopCartItem = eventData.pointerDrag.GetComponent<CardShopCartUIController>();

        // does the dragged object have ShopCart but deactivated?
        if (!shopCartItem.enabled)
        {
            shopCartItem.enabled = true;
            shopCartItem.CardShopCartSlotController = this;

            CardShopVendorUIController shopItem = shopCartItem.GetComponent<CardShopVendorUIController>();
            shopItem.enabled = false;
        }

        slotManager.HandleDrop(eventData, shopCartItem, this);
    }
}

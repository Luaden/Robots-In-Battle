using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSlotController : BaseSlotController<ShopItemUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<ShopItemUIController>() == null)
        {
            Debug.Log("OnDrop: ShopItem is null");
            return;
        }
        // is shopitem deactivated?
        if (!eventData.pointerDrag.GetComponent<ShopItemUIController>().enabled)
        {
            // is it part of ShopCart?
            if (eventData.pointerDrag.GetComponent<ShopCartItemController>() != null)
            {
                // enable shopitem
                ShopItemUIController shopItem = eventData.pointerDrag.GetComponent<ShopItemUIController>();
                shopItem.enabled = true;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopItemUIController>(), this);
                // deactivate shopcart
                shopItem.GetComponent<ShopCartItemController>().enabled = false;
            }
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopItemUIController>(), this);
    }
}

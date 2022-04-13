using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSlotController : BaseSlotController<ShopItemUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.GetComponent<ShopItemUIController>().enabled)
        {
            if (eventData.pointerDrag.GetComponent<ShopCartItemController>() != null)
            {
                ShopItemUIController shopItem = eventData.pointerDrag.GetComponent<ShopItemUIController>();
                shopItem.enabled = true;

                shopItem.GetComponent<ShopCartItemController>().enabled = false;
                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopItemUIController>(), this);
            }
            return;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopItemUIController>(), this);
    }
}

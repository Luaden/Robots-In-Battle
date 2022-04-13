using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartItemSlotController : BaseSlotController<ShopCartItemController>
{
    public override void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag.GetComponent<ShopCartItemController>() == null)
        {
            if(eventData.pointerDrag.GetComponent<ShopItemUIController>() != null)
            {
                ShopCartItemController shopItem = eventData.pointerDrag.AddComponent<ShopCartItemController>();
                shopItem.ShopCartItemSlotController = this;

                shopItem.GetComponent<ShopItemUIController>().enabled = false;
                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
                return;
            }
            return;
        }
        if (!eventData.pointerDrag.GetComponent<ShopCartItemController>().enabled)
        {
            ShopCartItemController shopItem = eventData.pointerDrag.GetComponent<ShopCartItemController>();
            shopItem.enabled = true;

            shopItem.GetComponent<ShopItemUIController>().enabled = false;
            slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
            return;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
    }
}

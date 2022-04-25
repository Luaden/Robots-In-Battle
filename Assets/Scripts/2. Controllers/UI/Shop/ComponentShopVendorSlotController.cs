using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentShopVendorSlotController : BaseSlotController<ComponentShopVendorUIController>
{
    public void SetSlotManager(BaseSlotManager<ComponentShopVendorUIController> slotManager)
    {
        this.slotManager = slotManager;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>() == null)
        {
            Debug.Log("OnDrop: CardShopVendorUIController is null");
            return;
        }
        // is shopitem deactivated?
        if (!eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>().enabled)
        {
            // is it part of ShopCart?
            if (eventData.pointerDrag.GetComponent<ComponentShopCartUIController>() != null)
            {
                // enable shopitem
                ComponentShopVendorUIController shopItem = eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>();
                shopItem.enabled = true;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>(), this);
                // deactivate shopcart
                shopItem.GetComponent<ComponentShopCartUIController>().enabled = false;
            }
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ComponentShopVendorUIController>(), this);
    }
}

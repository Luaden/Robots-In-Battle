using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardShopVendorSlotController : BaseSlotController<CardShopVendorUIController>
{
    public void SetSlotManager(BaseSlotManager<CardShopVendorUIController> slotManager) 
    {
        this.slotManager = slotManager;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<CardShopVendorUIController>() == null)
        {
            Debug.Log("OnDrop: CardShopVendorUIController is null");
            return;
        }
        // is shopitem deactivated?
        if (!eventData.pointerDrag.GetComponent<CardShopVendorUIController>().enabled)
        {
            // is it part of ShopCart?
            if (eventData.pointerDrag.GetComponent<CardShopCartUIController>() != null)
            {
                // enable shopitem
                CardShopVendorUIController shopItem = eventData.pointerDrag.GetComponent<CardShopVendorUIController>();
                shopItem.enabled = true;

                slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardShopVendorUIController>(), this);
                // deactivate shopcart
                shopItem.GetComponent<CardShopCartUIController>().enabled = false;
            }
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<CardShopVendorUIController>(), this);
    }
}

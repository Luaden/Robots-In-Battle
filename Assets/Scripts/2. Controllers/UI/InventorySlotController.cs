using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotController : BaseSlotController<InventoryUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (this.CurrentSlottedItem == null)
        {
            Debug.Log("this slot is empty");
            return;
        }

        InventoryUIController inventoryItem = eventData.pointerDrag.GetComponent<InventoryUIController>();
        if (inventoryItem == null)
        {
            Debug.Log("OnDrop: CardShopVendorUIController is null");
            return;
        }

        if (!inventoryItem.enabled)
        {
            EquipmentUIController equipmentItem = inventoryItem.GetComponent<EquipmentUIController>();
            if (equipmentItem != null)
            {
                inventoryItem.enabled = true;
                inventoryItem.InventorySlotController = this;

                equipmentItem.EquipmentSlotController.CurrentSlottedItem = null;
                equipmentItem.enabled = false;
            }
        }
        slotManager.HandleDrop(eventData, inventoryItem, this);
    }
}

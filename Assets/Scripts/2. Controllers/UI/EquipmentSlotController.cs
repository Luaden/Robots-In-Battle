using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotController : BaseSlotController<EquipmentUIController>
{
    public void SetSlotManager(BaseSlotManager<EquipmentUIController> slotManager)
    {
        this.slotManager = slotManager;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("on drop -- EquipmentSlotController");
        if (this.CurrentSlottedItem == null)
        {
            Debug.Log("this slot is empty");
            return;
        }

        EquipmentUIController equipmentItem = eventData.pointerDrag.GetComponent<EquipmentUIController>();
        if (equipmentItem == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }


        if (!equipmentItem.enabled)
        {
            InventoryUIController inventoryItem = equipmentItem.GetComponent<InventoryUIController>();
            if (inventoryItem != null)
            {
                Debug.Log("dropped item");

                //set this slot's "previous" item's slotcontroller to be the dropped item's slotcontroller, swapping places
                EquipmentUIController previousSlottedItem = currentSlottedItem.GetComponent<EquipmentUIController>();
                previousSlottedItem.GetComponent<InventoryUIController>().InventorySlotController = inventoryItem.InventorySlotController;
                previousSlottedItem.GetComponent<InventoryUIController>().enabled = true;
                previousSlottedItem.GetComponent<EquipmentUIController>().enabled = false;

                // set this dropped item's slot controller to be this slot
                equipmentItem.enabled = true;
                equipmentItem.EquipmentSlotController = this;

                // remove the dropped item from inventory slot
                inventoryItem.InventorySlotController.CurrentSlottedItem = null;
                inventoryItem.enabled = false;

                GameManager.instance.PlayerMechController.SwapPlayerMechPart(equipmentItem.ItemUIObject.MechComponentData);

            }
        }

        slotManager.HandleDrop(eventData, equipmentItem, this);
    }
}

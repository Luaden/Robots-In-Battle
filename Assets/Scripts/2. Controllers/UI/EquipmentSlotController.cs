using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotController : BaseSlotController<EquipmentUIController>
{
    private MechComponent mechComponentType;
    public MechComponent MechComponentType { get => mechComponentType; }
    public void SetSlotManager(BaseSlotManager<EquipmentUIController> slotManager)
    {
        this.slotManager = slotManager;
    }
    public void SetComponentType(MechComponent mechComponent)
    {
        this.mechComponentType = mechComponent;
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

        if(equipmentItem.MechComponentUIObject.MechComponentData.ComponentType != this.MechComponentType)
        {
            Debug.Log("the dropped item is not of the same component type as slot");
            return;
        }


        if (!equipmentItem.enabled)
        {
            InventoryUIController inventoryItem = equipmentItem.GetComponent<InventoryUIController>();
            if (inventoryItem != null)
            {
                Debug.Log("dropped item");
                // this current slottedItem
                EquipmentUIController currentItem = CurrentSlottedItem;

                //enable both of the UIControllers
                currentItem.GetComponent<EquipmentUIController>().enabled = true;
                currentItem.GetComponent<InventoryUIController>().enabled = true;

                // set current slot item to take this dropped item's place
                currentItem.GetComponent<InventoryUIController>().InventorySlotController = inventoryItem.InventorySlotController;
                currentItem.GetComponent<EquipmentUIController>().enabled = false;

                currentSlottedItem = null;

                // set this dropped item's slot controller to be this slot
                equipmentItem.enabled = true;
                equipmentItem.EquipmentSlotController = this;
                currentSlottedItem = equipmentItem;

                // remove the dropped item from inventory slot
                inventoryItem.InventorySlotController.CurrentSlottedItem = null;
                inventoryItem.enabled = false;

                GameManager.instance.PlayerMechController.SwapPlayerMechPart(equipmentItem.MechComponentUIObject.MechComponentData);

            }
        }

        slotManager.HandleDrop(eventData, equipmentItem, this);
    }
}

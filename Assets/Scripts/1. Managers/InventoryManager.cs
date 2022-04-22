using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private InventoryController inventoryController;

    //private InventoryItemUIController inventoryUIController;
    //private InventoryItemUIObject inventoryItemUIObject;
    //private InventoryItemSlotManager;
    //private InventoryItemSlotController;

    // List<Items>

    public void AddItemToInventory(SOItemDataObject mechComponent) => inventoryController.AddItemToInventory(mechComponent);

    public void AddItemToInventory(MechComponentDataObject mechComponent) => inventoryController.AddItemToInventory(mechComponent);
}

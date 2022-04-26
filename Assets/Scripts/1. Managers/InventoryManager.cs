using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] protected EquipmentController equipController;
    [SerializeField] protected InventoryController inventoryController;

    public EquipmentController EquipmentController { get => equipController; }
    public InventoryController InventoryController { get => inventoryController; }
    public void OpenAndClose()
    {
        UpdateItemsToDisplay();
        bool active = gameObject.activeInHierarchy;

        gameObject.SetActive(!active);
    }
    public void UpdateItemsToDisplay()
    {
        equipController.UpdateItemsToDisplay();
        inventoryController.UpdateItemsToDisplay();
    }
}

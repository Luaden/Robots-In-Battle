using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController
{
    public void AddItemToInventory(SOItemDataObject mechComponent)
    {
        if(mechComponent.ItemType != ItemType.Component)
        {
            Debug.Log("A card was attempted to be added to the inventory, but this was an incorrect location for it.");
            Debug.Log("Is " + mechComponent.ItemType + " the correct ItemType?");
            return;
        }

        MechComponentDataObject newComponent = new MechComponentDataObject(mechComponent);
        AddItemToInventory(newComponent);
    }

    public void AddItemToInventory(MechComponentDataObject mechComponent)
    {
        GameManager.instance.PlayerData.PlayerInventory.Add(mechComponent);
    }

    public void RemoveItemFromInventory(MechComponentDataObject mechComponent)
    {
        if (GameManager.instance.PlayerData.PlayerInventory.Contains(mechComponent))
            GameManager.instance.PlayerData.PlayerInventory.Remove(mechComponent);
        else
            Debug.Log("Attempted to remove " + mechComponent.ComponentName + " from the inventory, but it was not found.");
    }
}

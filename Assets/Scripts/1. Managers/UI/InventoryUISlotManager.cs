using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlotManager : BaseSlotManager<ShopItemUIController>
{
    [SerializeField] private GameObject slotContainer;
    [SerializeField] private GameObject slotPrefab;

    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            item.notInMech = true;
            item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
            slot.CurrentSlottedItem = item;
            item.ItemSlotController = slot;
            return;
        }
        else
        {
            foreach (BaseSlotController<ShopItemUIController> slotOption in slotList)
                if (slotOption.CurrentSlottedItem == null)
                {
                    if (item.ItemSlotController != null)
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);

                    item.notInMech = true;
                    slotOption.CurrentSlottedItem = item;
                    item.ItemSlotController = slotOption;
                    return;
                }
                else
                    continue;

            GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);
            ShopItemUISlotController slotController = newSlot.GetComponent<ShopItemUISlotController>();
            slotController.SetSlotManager(this);

            if (item.ItemSlotController != null)
                item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
            item.notInMech = true;
            slotController.CurrentSlottedItem = item;
            item.ItemSlotController = slotController;
        }

        UpdatePlayerInventory();
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        SlotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        if (newData == null)
        {
            Debug.Log("Could not find appropriate data for slot.");
            return;
        }

        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                slot.CurrentSlottedItem = null;
                GameManager.instance.PlayerInventoryController.RemoveItemFromInventory(item.BaseSOItemDataObject);
            }
    }

    private void Start()
    {
        Debug.Log("Building current mech Items.");
        foreach (MechComponentDataObject item in GameManager.instance.PlayerInventoryController.PlayerInventory)
            if (item.SOItemDataObject.ItemType == ItemType.Component)
                DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item.SOItemDataObject, this);
    }

    private void UpdatePlayerInventory()
    {
        List<SOItemDataObject> newSOItemDataObjectList = new List<SOItemDataObject>();

        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            newSOItemDataObjectList.Add(slot.CurrentSlottedItem.BaseSOItemDataObject);

        foreach (SOItemDataObject item in newSOItemDataObjectList)
            GameManager.instance.PlayerInventoryController.AddItemToInventory(new MechComponentDataObject(item));
    }
}

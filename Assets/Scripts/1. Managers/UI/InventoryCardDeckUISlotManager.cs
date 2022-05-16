using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCardDeckUISlotManager : BaseSlotManager<ShopItemUIController>
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
            AddSlotToList(slotController);

            if (item.ItemSlotController != null)
                item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);

            item.notInMech = true;
            slotController.CurrentSlottedItem = item;
            item.ItemSlotController = slotController;
        }
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        if(newSlot != null)
            slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                slot.CurrentSlottedItem = null;
                GameManager.instance.PlayerDeckController.RemoveCardFromPlayerDeck(item.BaseSOItemDataObject);
                return;
            }
    }

    private void Start()
    {
        foreach (SOItemDataObject item in GameManager.instance.PlayerDeckController.PlayerDeck)
            if (item.ItemType == ItemType.Card)
                DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item, this);
    }
}
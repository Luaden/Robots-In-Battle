using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlotManager : BaseSlotManager<ShopItemUIController>
{
    [SerializeField] private GameObject slotContainerInventory;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private GameObject cardDeckButton;
    [SerializeField] private GameObject inventoryButton;

    [SerializeField] private GameObject cardDeckPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject mechComponentsPanel;


    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> slot)
    {

        if (item.MechComponentDataObject == null)
        {
            item.MechComponentDataObject = new MechComponentDataObject(item.BaseSOItemDataObject);
            item.DisablePriceTag();
        }

        if (slot != null && slot.CurrentSlottedItem == null)
        {
            item.NotInMech = true;
            item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
            slot.CurrentSlottedItem = item;
            item.ItemSlotController = slot;
            item.DisablePriceTag();
            return;
        }
        else
        {
            foreach (BaseSlotController<ShopItemUIController> slotOption in slotList)
                if (slotOption.CurrentSlottedItem == null)
                {
                    if (item.ItemSlotController != null)
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);

                    item.NotInMech = true;
                    slotOption.CurrentSlottedItem = item;
                    item.ItemSlotController = slotOption;
                    item.DisablePriceTag();
                    return;
                }
                else
                    continue;

            GameObject newSlot = Instantiate(slotPrefab, slotContainerInventory.transform);
            ShopItemUISlotController slotController = newSlot.GetComponent<ShopItemUISlotController>();
            slotController.SetSlotManager(this);

            if (item.ItemSlotController != null)
                item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);

            item.NotInMech = true;
            slotController.CurrentSlottedItem = item;
            item.ItemSlotController = slotController;
            item.DisablePriceTag();
        }
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

        if (newData.MechComponentDataObject == null)
        {
            MechComponentDataObject newComponent = new MechComponentDataObject(newData.BaseSOItemDataObject);
            newData.MechComponentDataObject = newComponent;
        }

        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                slot.CurrentSlottedItem = null;
                GameManager.instance.PlayerInventoryController.RemoveItemFromInventory(item.MechComponentDataObject);
                return;
            }
    }

    public void SetInventoryActive()
    {
        inventoryButton.SetActive(false);
        inventoryPanel.SetActive(true);
        cardDeckButton.SetActive(true);
        cardDeckPanel.SetActive(false);
        mechComponentsPanel.SetActive(true);
    }

    public void SetCardDeckActive()
    {
        cardDeckButton.SetActive(false);
        cardDeckPanel.SetActive(true);
        inventoryButton.SetActive(true);
        inventoryPanel.SetActive(false);
        mechComponentsPanel.SetActive(false);
    }

    private void Start()
    {
        foreach (MechComponentDataObject item in GameManager.instance.PlayerInventoryController.PlayerInventory)
        {
            Debug.Log(item.ComponentName);
            DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item.SOItemDataObject, this, item);
        }

        DowntimeManager.OnLoadCombatScene += UpdatePlayerInventory;
    }

    private void OnDestroy()
    {
        DowntimeManager.OnLoadCombatScene -= UpdatePlayerInventory;
    }

    private void UpdatePlayerInventory()
    {
        List<ShopItemUIController> shopItemUIControllers = new List<ShopItemUIController>();

        foreach (BaseSlotController<ShopItemUIController> slot in slotList)
            shopItemUIControllers.Add(slot.CurrentSlottedItem);

        foreach (ShopItemUIController item in shopItemUIControllers)
        {
            if (item == null)
                continue;

            if (item.MechComponentDataObject != null)
                GameManager.instance.PlayerInventoryController.AddItemToInventory(item.MechComponentDataObject);
            else
                GameManager.instance.PlayerInventoryController.AddItemToInventory(new MechComponentDataObject(item.BaseSOItemDataObject));

            item.DisablePriceTag();
        }
    }

    [ContextMenu("Count Inventory")]
    private void CountInventory()
    {
        Debug.Log(GameManager.instance.PlayerInventoryController.PlayerInventory.Count);
    }
}

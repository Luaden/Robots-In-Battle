using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventoryCardDeckUISlotManager : BaseSlotManager<ShopItemUIController>
{
    [SerializeField] private GameObject slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject playerDeckPanel;
    [SerializeField] private GameObject opponentDeckPanel;

    [SerializeField] private GameObject fadeObject;
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
        if(DowntimeManager.instance != null)
        {
            foreach (SOItemDataObject item in GameManager.instance.PlayerDeckController.PlayerDeck)
                if (item.ItemType == ItemType.Card)
                    DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item, this);
        }
        else if(CombatManager.instance != null)
        {
            foreach (SOItemDataObject item in GameManager.instance.PlayerDeckController.PlayerDeck)
                if (item.ItemType == ItemType.Card)
                    CombatManager.instance.CardUIManager.BuildPlayerInventoryCard(item);
        }
    }

    public void SetActive()
    {
        if (!playerDeckPanel.activeInHierarchy)
        {
            fadeObject.SetActive(true);
            playerDeckPanel.SetActive(true);
            opponentDeckPanel.SetActive(false);
        }
        else
        {
            fadeObject.SetActive(false);
            playerDeckPanel.SetActive(false);
            opponentDeckPanel.SetActive(false);
        }
    }
}

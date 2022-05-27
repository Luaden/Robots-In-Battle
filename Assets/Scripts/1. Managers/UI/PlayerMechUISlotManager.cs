using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMechUISlotManager : BaseSlotManager<ShopItemUIController>
{
    [SerializeField] private BaseSlotController<ShopItemUIController> headSlot;
    [SerializeField] private BaseSlotController<ShopItemUIController> torsoSlot;
    [SerializeField] private BaseSlotController<ShopItemUIController> armsSlot;
    [SerializeField] private BaseSlotController<ShopItemUIController> legsSlot;

    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> slot)
    {
        if (slot == null)
        {
            switch (item.BaseSOItemDataObject.ComponentType)
            {
                case MechComponent.Head:
                    headSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = headSlot;
                    item.NotInMech = false;
                    item.DisablePriceTag();
                    break;

                case MechComponent.Torso:
                    torsoSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = torsoSlot;
                    item.NotInMech = false;
                    item.DisablePriceTag();
                    break;
                case MechComponent.Arms:
                    armsSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = armsSlot;
                    item.NotInMech = false;
                    item.DisablePriceTag();
                    break;

                case MechComponent.Legs:
                    legsSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = legsSlot;
                    item.NotInMech = false;
                    item.DisablePriceTag();
                    break;
            }

            return;
        }

        if (slot.CurrentSlottedItem == null)
        {
            if (item.ItemSlotController != null)
            {
                if (headSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Head)
                    {
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
                        slot.CurrentSlottedItem = item;
                        item.ItemSlotController = slot;
                        item.NotInMech = false;
                        item.DisablePriceTag();

                        CheckSlotItems();
                        UpdateCurrentMech();
                        return;
                    }
                }

                if (torsoSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Torso)
                    {
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
                        slot.CurrentSlottedItem = item;
                        item.ItemSlotController = slot;
                        item.NotInMech = false;
                        item.DisablePriceTag();

                        CheckSlotItems();
                        UpdateCurrentMech();
                        return;
                    }
                }

                if (armsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Arms)
                    {
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
                        slot.CurrentSlottedItem = item;
                        item.ItemSlotController = slot;
                        item.NotInMech = false;
                        item.DisablePriceTag();

                        CheckSlotItems();
                        UpdateCurrentMech();
                        return;
                    }
                }

                if (legsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Legs)
                    {
                        item.ItemSlotController.SlotManager.RemoveItemFromCollection(item);
                        slot.CurrentSlottedItem = item;
                        item.ItemSlotController = slot;
                        item.NotInMech = false;
                        item.DisablePriceTag();

                        CheckSlotItems();
                        UpdateCurrentMech();
                        return;
                    }
                }
            }
        }
        else
        {
            if (item.ItemSlotController != null)
            {
                if (headSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Head)
                    {
                        SwapComponentIntoInventory(item, headSlot);
                        return;
                    }
                }

                if (torsoSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Torso)
                    {
                        SwapComponentIntoInventory(item, torsoSlot);
                        return;
                    }
                }

                if (armsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Arms)
                    {
                        SwapComponentIntoInventory(item, armsSlot);
                        return;
                    }
                }

                if (legsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Legs)
                    {
                        SwapComponentIntoInventory(item, legsSlot);
                        return;
                    }
                }
            }
        }

        void SwapComponentIntoInventory(ShopItemUIController newItem, BaseSlotController<ShopItemUIController> slotToAddTo)
        {
            newItem.ItemSlotController.SlotManager.RemoveItemFromCollection(newItem);
            newItem.DisablePriceTag();

            DowntimeManager.instance.InventoryUISlotManager.AddItemToCollection(slotToAddTo.CurrentSlottedItem, slotToAddTo);

            newItem.NotInMech = false;
            slotToAddTo.CurrentSlottedItem = newItem;
            newItem.ItemSlotController = slot;
            CheckSlotItems();
            UpdateCurrentMech();
        }
    }

    private void CheckSlotItems()
    {
        if (headSlot.CurrentSlottedItem == null)
            Debug.Log("Missing head item.");
        if (torsoSlot.CurrentSlottedItem == null)
            Debug.Log("Missing torso item.");
        if (armsSlot.CurrentSlottedItem == null)
            Debug.Log("Missing arms item.");
        if (legsSlot.CurrentSlottedItem == null)
            Debug.Log("Missing legs item.");
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        SlotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        if (newData == null || !newData.NotInMech)
        {
            Debug.Log("Could not find appropriate data for slot.");
            return;
        }

        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        if (headSlot.CurrentSlottedItem == item)
            headSlot.CurrentSlottedItem = null;
        if (torsoSlot.CurrentSlottedItem == item)
            torsoSlot.CurrentSlottedItem = null;
        if (armsSlot.CurrentSlottedItem == item)
            armsSlot.CurrentSlottedItem = null;
        if (legsSlot.CurrentSlottedItem == item)
            legsSlot.CurrentSlottedItem = null;
    }

    private void Start()
    {
        MechObject playerMech = GameManager.instance.PlayerMechController.PlayerMech;
        List<MechComponentDataObject> currentParts = new List<MechComponentDataObject>();

        currentParts.Add(playerMech.MechHead);
        currentParts.Add(playerMech.MechTorso);
        currentParts.Add(playerMech.MechArms);
        currentParts.Add(playerMech.MechLegs);

        foreach (MechComponentDataObject item in currentParts)
        {
            DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item.SOItemDataObject, this, item);
        }
    }

    private void UpdateCurrentMech()
    {
        CheckSlotItems();

        MechObject newMech = new MechObject(headSlot.CurrentSlottedItem.MechComponentDataObject, 
                                            torsoSlot.CurrentSlottedItem.MechComponentDataObject,
                                            armsSlot.CurrentSlottedItem.MechComponentDataObject,
                                            legsSlot.CurrentSlottedItem.MechComponentDataObject);

        GameManager.instance.PlayerMechController.SetNewPlayerMech(newMech);
        DowntimeManager.instance.MechSpriteSwapManager.UpdateMechSprites(GameManager.instance.PlayerMechController.PlayerMech, CharacterSelect.Player);
    }
}

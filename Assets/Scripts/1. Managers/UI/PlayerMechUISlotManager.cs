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
        void SwapComponentIntoInventory(ShopItemUIController newItem, BaseSlotController<ShopItemUIController> slotToAddTo)
        {
            DowntimeManager.instance.InventoryUISlotManager.AddItemToCollection(slotToAddTo.CurrentSlottedItem, slotToAddTo);
            GameManager.instance.PlayerMechController.SwapPlayerMechPart(new MechComponentDataObject(newItem.BaseSOItemDataObject));

            newItem.ItemSlotController.SlotManager.RemoveItemFromCollection(newItem);
            item.notInMech = false;
            slotToAddTo.CurrentSlottedItem = newItem;
            newItem.ItemSlotController = slot;
        }

        if(slot == null)
        {
            switch (item.BaseSOItemDataObject.ComponentType)
            {
                case MechComponent.Head:
                    headSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = headSlot;
                    item.notInMech = false;
                    break;

                case MechComponent.Torso:
                    torsoSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = torsoSlot;
                    item.notInMech = false;

                    break;
                case MechComponent.Arms:
                    armsSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = armsSlot;
                    item.notInMech = false;
                    break;

                case MechComponent.Legs:
                    legsSlot.CurrentSlottedItem = item;
                    item.ItemSlotController = legsSlot;
                    item.notInMech = false;
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
                        item.notInMech = false;

                        //Update mech UI content
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
                        item.notInMech = false;

                        //Update mech UI content
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
                        item.notInMech = false;

                        //Update mech UI content
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
                        item.notInMech = false;

                        //Update mech UI content
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

                        //Update mech UI content
                        return;
                    }
                }

                if (torsoSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Torso)
                    {
                        SwapComponentIntoInventory(item, torsoSlot);

                        //Update mech UI content
                        return;
                    }
                }

                if (armsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Arms)
                    {
                        SwapComponentIntoInventory(item, armsSlot);

                        //Update mech UI content
                        return;
                    }
                }

                if (legsSlot == slot)
                {
                    if (item.BaseSOItemDataObject.ComponentType == MechComponent.Legs)
                    {
                        SwapComponentIntoInventory(item, legsSlot);

                        Debug.Log("New " + item.BaseSOItemDataObject.ComponentType + " added.");

                        //Update mech UI content
                        return;
                    }
                }
            }
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
            legsSlot = null;
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
            DowntimeManager.instance.ShopItemUIBuildController.BuildAndDisplayItemUI(item.SOItemDataObject, this);
    }

    private void UpdateCurrentMech()
    {
        
    }
}

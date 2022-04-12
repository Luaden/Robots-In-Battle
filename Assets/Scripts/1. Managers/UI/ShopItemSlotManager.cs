using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSlotManager : BaseSlotManager<ShopItemUIController>
{
    public override void AddItemToCollection(ShopItemUIController item, BaseSlotController<ShopItemUIController> slot)
    {
        throw new System.NotImplementedException();
    }

    public override void AddSlotToList(BaseSlotController<ShopItemUIController> newSlot)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleDrop(PointerEventData eventData, ShopItemUIController newData, BaseSlotController<ShopItemUIController> slot)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveItemFromCollection(ShopItemUIController item)
    {
        throw new System.NotImplementedException();
    }
}

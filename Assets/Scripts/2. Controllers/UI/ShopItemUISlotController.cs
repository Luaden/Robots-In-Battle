using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemUISlotController : BaseSlotController<ShopItemUIController>
{
    public void SetSlotManager(BaseSlotManager<ShopItemUIController> slotManager) 
    {
        this.slotManager = slotManager;
        slotManager.SlotList.Add(this);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<ShopItemUIController>() == null)
        {
            Debug.Log("OnDrop: ShopItemUIController is null");
            return;
        }

        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopItemUIController>(), this);
    }
}

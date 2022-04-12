using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCartItemSlotController : BaseSlotController<ShopCartItemController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<ShopCartItemController>() == null)
        {
            Debug.Log("Item was dropped in a slot that does not fit it.");
            return;
        }
        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<ShopCartItemController>(), this);
    }
}

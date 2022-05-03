using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeSlotManager : BaseSlotManager<NodeUIController>
{

    public override void AddItemToCollection(NodeUIController item, BaseSlotController<NodeUIController> slot)
    {
        if (slot != null && slot.CurrentSlottedItem == null)
        {
            slot.CurrentSlottedItem = item;
            item.NodeSlotController = slot;
            return;
        }
        foreach (BaseSlotController<NodeUIController> slotOption in slotList)
            if (slotOption.CurrentSlottedItem == null)
            {
                slotOption.CurrentSlottedItem = item;
                item.NodeSlotController = slotOption;
                return;
            }
    }

    public override void AddSlotToList(BaseSlotController<NodeUIController> newSlot)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleDrop(PointerEventData eventData, NodeUIController newData, BaseSlotController<NodeUIController> slot)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveItemFromCollection(NodeUIController item)
    {
        throw new System.NotImplementedException();
    }
}

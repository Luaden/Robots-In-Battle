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
        if (newData == null)
            return;

        newData.NodeSlotController.SlotManager.RemoveItemFromCollection(newData);
        AddItemToCollection(newData, slot);
    }

    public override void RemoveItemFromCollection(NodeUIController item)
    {
        foreach (NodeSlotController slot in slotList)
            if (slot.CurrentSlottedItem == item)
            {
                NodeDataObject slotNode = slot.GetComponent<NodeDataObject>();
                
                slotNode.HasBeenAssignedFighter = false;
                slot.CurrentSlottedItem = null;
            }
    }


}

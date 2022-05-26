using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NodeSlotController : BaseSlotController<NodeUIController>
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<NodeDataObject>() != null)
            return;

/*        if (this.GetComponent<NodeDataObject>().nodeType != NodeDataObject.NodeType.Starter &&
            this.GetComponent<NodeDataObject>().nodeType != NodeDataObject.NodeType.FighterStarter)
            return;
*/
        if (eventData.pointerDrag.GetComponent<NodeDataObject>().PreviousNode != null)
            return;

        // swapping items between two slots
        if (currentSlottedItem != null)
        {
            // the obj we are dragging
            NodeUIController draggedObjItem = eventData.pointerDrag.GetComponent<NodeUIController>();
            // temp cache of the current slotted item on this slot
            NodeUIController tempCurrentSlotItem = this.currentSlottedItem;

            // remove the dragged obj from its current slot
            draggedObjItem.NodeSlotController.SlotManager.RemoveItemFromCollection(draggedObjItem);
            // remove this slots item from this slot
            currentSlottedItem.NodeSlotController.SlotManager.RemoveItemFromCollection(currentSlottedItem);


            // add current slotted item to the slot of the dragged obj
            draggedObjItem.NodeSlotController.SlotManager.AddItemToCollection(tempCurrentSlotItem, draggedObjItem.NodeSlotController);
            // add the dragged obj to this slot
            slotManager.AddItemToCollection(draggedObjItem, this);

            // alert that we have assigned fighters to each slot
            OnFighterAssigned(tempCurrentSlotItem, tempCurrentSlotItem.NodeSlotController);
            OnFighterAssigned(currentSlottedItem, this);

            return;

        }

        OnFighterAssigned(eventData.pointerDrag.GetComponent<NodeUIController>(), this);
        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<NodeUIController>(), this);

    }

    public void OnFighterAssigned(NodeUIController item, BaseSlotController<NodeUIController> slot)
    {
        NodeDataObject slotNode = slot.GetComponent<NodeDataObject>();
        if (slotNode == null || item == null)
            return;

        slotNode.HasBeenAssignedFighter = true;
        NodeDataObject itemNode = item.GetComponent<NodeDataObject>();
        itemNode.SetParentNode(slotNode);


    }


}

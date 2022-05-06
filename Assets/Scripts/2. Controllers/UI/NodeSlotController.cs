using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NodeSlotController : BaseSlotController<NodeUIController>
{
    public event Action<NodeUIController, BaseSlotController<NodeUIController>> onAssignFighter;
    private void Start()
    {
        onAssignFighter += TournamentOverviewManager.instance.NodeSlotManager.OnFighterAssigned;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (this.GetComponent<NodeDataObject>().nodeType != NodeDataObject.NodeType.Starter &&
            this.GetComponent<NodeDataObject>().nodeType != NodeDataObject.NodeType.FighterStarter)
            return;

        if (currentSlottedItem != null)
        {
            // the obj we are dragging
            NodeUIController draggedObjItem = eventData.pointerDrag.GetComponent<NodeUIController>();
            // temp cache of the dragged obj
            NodeUIController tempDraggedObj = draggedObjItem;
            // temp cache of the current slotted item on this slot
            NodeUIController tempCurrentSlotItem = this.currentSlottedItem;

            // remove the dragged obj from its current slot
            draggedObjItem.NodeSlotController.SlotManager.RemoveItemFromCollection(draggedObjItem);
            // remove this slots item from this slot
            currentSlottedItem.NodeSlotController.SlotManager.RemoveItemFromCollection(currentSlottedItem);
            // add current slotted item to the slot of the dragged obj
            draggedObjItem.NodeSlotController.SlotManager.AddItemToCollection(tempCurrentSlotItem, tempDraggedObj.NodeSlotController);
            // add the dragged obj to this slot
            slotManager.AddItemToCollection(tempDraggedObj, this);

            // ping that we have assigned fighters
            onAssignFighter(tempCurrentSlotItem, tempCurrentSlotItem.NodeSlotController);
            onAssignFighter(tempDraggedObj, this);

            // if the object has a pair node..
            if (GetComponent<NodeDataObject>().PairNode != null)
            {
                // if the pair node has been assigned a pilot...
                if (this.GetComponent<NodeDataObject>().PairNode.HasBeenAssigned)
                {
                    Debug.Log("the pair has completed");
                }
            }

            return;

        }
        onAssignFighter(eventData.pointerDrag.GetComponent<NodeUIController>(), this);
        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<NodeUIController>(), this);

    }

    private void OnDestroy()
    {
        onAssignFighter -= TournamentOverviewManager.instance.NodeSlotManager.OnFighterAssigned;
    }
}

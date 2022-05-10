using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Linq;

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
                // if the item has been removed, it has no longer an assigned pilot
                if (TournamentOverviewManager.instance.GetActiveList().Contains(slotNode))
                    TournamentOverviewManager.instance.RemoveInActiveList(slotNode);
                slotNode.HasBeenAssigned = false;
                slot.CurrentSlottedItem = null;
            }
    }

    public void OnFighterAssigned(NodeUIController item, BaseSlotController<NodeUIController> slot)
    {
        NodeDataObject slotNode = slot.GetComponent<NodeDataObject>();
        if (slotNode == null || item == null)
            return;

        NodeDataObject itemNode = item.GetComponent<NodeDataObject>();
        itemNode.ParentNode = slotNode;

        slotNode.HasBeenAssigned = true;
        itemNode.HasBeenAssigned = true;

        if (!TournamentOverviewManager.instance.GetActiveList().Contains(slotNode))
            TournamentOverviewManager.instance.AddToActiveList(slotNode);

/*
        if(TournamentOverviewManager.instance.GetActiveList().Contains(slotNode))
        {
            if(slotNode.PairNode.HasBeenAssigned && slotNode.HasBeenAssigned)
            {
                TournamentOverviewManager.instance.AssignFighterToNodeSlot(slotNode, slotNode.PairNode);
            }
        }*/


        //TournamentOverviewManager.instance.AssignFighterToNodeSlot(item.GetComponent<NodeDataObject>());
    }
}

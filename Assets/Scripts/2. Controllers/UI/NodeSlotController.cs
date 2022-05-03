using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NodeSlotController : BaseSlotController<NodeUIController>
{
    public event Action<NodeUIController, BaseSlotController<NodeUIController>> onAssignPilot;
    private void Start()
    {
        onAssignPilot += TournamentOverviewManager.instance.NodeSlotManager.OnPilotAssigned;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        NodeDataObject nodeData = eventData.pointerEnter.GetComponent<NodeDataObject>();
        if (nodeData == null)
            return;

        if (nodeData.nodeType != NodeDataObject.NodeType.Starter && nodeData.nodeType != NodeDataObject.NodeType.None)
            return;

        onAssignPilot(eventData.pointerDrag.GetComponent<NodeUIController>(), this);
        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<NodeUIController>(), this);

        if(GetComponent<NodeDataObject>().PairNode != null)
        {
            if (this.GetComponent<NodeDataObject>().PairNode.IsCompleted)
            {
                Debug.Log("the pair is completed");
            }
        }

    }
}

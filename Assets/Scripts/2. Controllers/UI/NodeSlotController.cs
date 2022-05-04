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
        
        NodeDataObject nodeData = eventData.pointerEnter.GetComponent<NodeDataObject>();
        if (nodeData == null)
            return;

        if (nodeData.nodeType != NodeDataObject.NodeType.Starter && nodeData.nodeType != NodeDataObject.NodeType.FighterStarter)
            return;

        

        onAssignFighter(eventData.pointerDrag.GetComponent<NodeUIController>(), this);
        slotManager.HandleDrop(eventData, eventData.pointerDrag.GetComponent<NodeUIController>(), this);

        if(GetComponent<NodeDataObject>().PairNode != null)
        {
            // if the pair node has been assigned a pilot...
            if (this.GetComponent<NodeDataObject>().PairNode.HasBeenAssigned)
            {
                Debug.Log("the pair has completed");
            }
        }

    }

    private void OnDestroy()
    {
        onAssignFighter -= TournamentOverviewManager.instance.NodeSlotManager.OnFighterAssigned;
    }
}

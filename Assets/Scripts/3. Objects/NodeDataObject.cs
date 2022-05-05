using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    public NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;
    [SerializeField] protected NodeDataObject pairNode;
    [SerializeField] protected NodeDataObject parentNode;
    public bool hasBeenAssigned;
    public bool hasWonBattle;
    [SerializeField] protected bool isFinalNode;


    private FighterDataObject currentFighter;
    private NodeUIController nodeUIController;

    public FighterDataObject FighterDataObject { get => currentFighter; }
    public NodeUIController NodeUIController { get => nodeUIController; set => nodeUIController = value; }
    public NodeDataObject PreviousNode { get => previousNode; set => previousNode = value; }
    public NodeDataObject NextNode { get => nextNode; set => nextNode = value; }
    public NodeDataObject PairNode { get => pairNode; set => pairNode = value; }
    public NodeDataObject ParentNode { get => parentNode; set => parentNode = value; }
    public bool HasBeenAssigned { get => hasBeenAssigned; set => hasBeenAssigned = value; }
    public bool HasWonBattle { get => hasWonBattle; set => hasWonBattle = value; }
    public bool IsFinalNode { get => isFinalNode; set => isFinalNode = value; }

    // test
    private string fighterName;
    public string FighterName { get => fighterName; }

    public void Init()
    {
        // this would be inside a fighter obj?
        parentNode = transform.parent.GetComponent<NodeDataObject>();
        switch (nodeType)
        {
            case NodeType.Opponent:
                fighterName = "Fighter AI";
                break;
            case NodeType.Pilot:
                fighterName = "Player";
                break;
        }
    }
    //test
    public enum NodeType
    {
        Starter,
        Second,
        Third,
        Last,
        // where the fighter starts
        FighterStarter,
        // not supposed to be here
        Pilot,
        Opponent
    }
    public NodeType nodeType;

    public void MoveToNextNode()
    {
        nodeUIController.NodeSlotController = nextNode.GetComponent<NodeSlotController>();
        transform.position = nextNode.transform.position;

        Debug.Log(this.parentNode);
    }

    public void UpdateToParentNode(NodeDataObject parentNode)
    {
        this.parentNode = parentNode;
        nextNode = parentNode.nextNode;
        previousNode = parentNode.previousNode;
        hasBeenAssigned = parentNode.HasBeenAssigned;
        hasWonBattle = parentNode.HasWonBattle;

    }

}

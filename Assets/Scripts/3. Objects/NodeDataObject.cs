using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    private NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;

    private FighterDataObject currentFighter;
    private NodeUIController nodeUIController;
    public NodeUIController NodeUIController { get => nodeUIController; set => nodeUIController = value; }

    public NodeDataObject GetPreviousNode() { return previousNode; }
    public NodeDataObject GetNextNode() { return nextNode; }

    //test
    public enum NodeType
    {
        Starter,
        Second,
        Third,
        Last,
        None
    }
    public NodeType nodeType;

}

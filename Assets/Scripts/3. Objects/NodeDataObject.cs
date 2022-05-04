using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    public NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;
    [SerializeField] protected NodeDataObject pairNode;
    public bool hasBeenAssigned;
    public bool hasWonBattle;
    public bool isFinalNode;

    private FighterDataObject currentFighter;
    private GameObject nodeUIController;

    public FighterDataObject FighterDataObject { get => currentFighter; }
    public GameObject NodeUIController { get => nodeUIController; set => nodeUIController = value; }
    public NodeDataObject PreviousNode { get => previousNode; set => previousNode = value; }
    public NodeDataObject NextNode { get => nextNode; set => nextNode = value; }
    public NodeDataObject PairNode { get => pairNode; set => pairNode = value; }
    public bool HasBeenAssigned { get => hasBeenAssigned; set => hasBeenAssigned = value; }
    public bool HasWonBattle { get => hasWonBattle; set => hasWonBattle = value; }
    public bool IsFinalNode { get => isFinalNode; set => isFinalNode = value; }

    public NodeDataObject GetPreviousNode() { return previousNode; }
    public NodeDataObject GetNextNode() { return nextNode; }

    // test
    private string fighterName;
    public string FighterName { get => fighterName; }

    public void Init()
    {
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

}

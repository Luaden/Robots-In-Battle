using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    public NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;
    [SerializeField] protected NodeDataObject pairNode;
    public bool isCompleted;
    public bool hasWon;

    private FighterDataObject currentFighter;
    private GameObject nodeUIController;

    public FighterDataObject FighterDataObject { get => currentFighter; }
    public GameObject NodeUIController { get => nodeUIController; set => nodeUIController = value; }
    public NodeDataObject PreviousNode { get => previousNode; set => previousNode = value; }
    public NodeDataObject NextNode { get => nextNode; set => nextNode = value; }
    public NodeDataObject PairNode { get => pairNode; set => pairNode = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public bool HasWon { get => hasWon; set => hasWon = value; }


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
                fighterName = "AI";
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
        PilotStarter,
        Pilot,
        Opponent
    }
    public NodeType nodeType;

}

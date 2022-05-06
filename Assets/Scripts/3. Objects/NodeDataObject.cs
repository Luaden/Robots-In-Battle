using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    protected NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;
    [SerializeField] protected NodeDataObject pairNode;
    protected NodeDataObject parentNode;
    private bool hasBeenAssigned;
    private bool hasWonBattle;
    [SerializeField] protected bool isFinalNode;

    private FighterDataObject currentFighter;
    [SerializeField] NodeUIController nodeUIController;

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
        Player,
        Opponent
    }
    public NodeType nodeType;

    public void Init()
    {
        parentNode = transform.parent.GetComponent<NodeDataObject>();
        switch (nodeType)
        {
            case NodeType.Opponent:
                fighterName = "Fighter AI";
                break;
            case NodeType.Player:
                fighterName = "Player";
                break;
        }
    }

    public void SetActive()
    {
        this.enabled = true;
    }
    public void SetInactive()
    {
        this.enabled = false;
    }
    public void MoveToNextNode()
    {
        if(nodeUIController != null && !nodeUIController.enabled)
        {
            Vector3 position = new Vector3(parentNode.transform.position.x, parentNode.transform.position.y);
            StartCoroutine(MoveObject(transform.position, position, Time.deltaTime * 15));
        }

    }

    public void UpdateToParentNode(NodeDataObject parentNode)
    {
        this.parentNode = parentNode;
        transform.SetParent(parentNode.transform);
        nextNode = parentNode.nextNode;
        previousNode = parentNode.previousNode;
        hasBeenAssigned = parentNode.HasBeenAssigned;
        hasWonBattle = parentNode.HasWonBattle;
        nodeUIController.NodeSlotController = parentNode.GetComponent<NodeSlotController>();

    }

    private IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float speed)
    {
        float i = 0.0f;
        float rate = 1.0f / speed;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        transform.position = endPos;
        StopCoroutine(nameof(MoveObject));
    }

}

using System.Collections;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    protected NodeDataObject previousNode;
    [SerializeField] protected NodeDataObject nextNode;
    [SerializeField] protected NodeDataObject pairNode;
    protected NodeDataObject parentNode;
    [SerializeField] private bool hasBeenAssignedToSlot;
    [SerializeField] private bool hasBeenAssignedFighterPair;
    [SerializeField] private bool hasWonBattle;
    [SerializeField] protected bool isFinalNode;
    [SerializeField] private int nodeIndex;

    [SerializeField] protected FighterDataObject currentFighter;
    [SerializeField] NodeUIController nodeUIController;

    public FighterDataObject FighterDataObject { get => currentFighter; }
    public NodeUIController NodeUIController { get => nodeUIController; set => nodeUIController = value; }
    public NodeDataObject PreviousNode { get => previousNode; set => previousNode = value; }
    public NodeDataObject NextNode { get => nextNode; set => nextNode = value; }
    public NodeDataObject PairNode { get => pairNode; set => pairNode = value; }
    public NodeDataObject ParentNode { get => parentNode; set => parentNode = value; }
    public bool HasBeenAssigned { get => hasBeenAssignedToSlot; set => hasBeenAssignedToSlot = value; }
    public bool HasWonBattle { get => hasWonBattle; set => hasWonBattle = value; }
    public bool IsFinalNode { get => isFinalNode; set => isFinalNode = value; }
    public int NodeIndex { get => nodeIndex; set => nodeIndex = value; }

    // test
    [SerializeField] private FighterDataObject pairNodeFighterData;
    public FighterDataObject PairNodeFighterData { get => pairNodeFighterData; set => pairNodeFighterData = value; }

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
        // the fighter node object can get parent node
        parentNode = transform.parent.GetComponent<NodeDataObject>();
        
        nodeIndex = parentNode.NodeIndex;
        switch (nodeType)
        {
            case NodeType.Player:
                fighterName = "Player";
                break;
            case NodeType.Opponent:
                fighterName = "Fighter AI";
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

    public void UpdateToParentNode(NodeDataObject parentNode)
    {
        this.parentNode = parentNode;
        transform.SetParent(parentNode.transform);
        nextNode = parentNode.nextNode;
        pairNode = parentNode.pairNode;
        previousNode = parentNode.previousNode;
        hasBeenAssignedToSlot = parentNode.HasBeenAssigned;
        hasWonBattle = parentNode.HasWonBattle;
        nodeUIController.NodeSlotController = parentNode.GetComponent<NodeSlotController>();

    }

    public void MoveToNextNode()
    {
        if(nodeUIController != null && !nodeUIController.enabled)
        {
            Vector3 position = new Vector3(parentNode.transform.position.x, parentNode.transform.position.y);
            StartCoroutine(MoveObject(transform.position, position, Time.deltaTime * 15));
        }

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

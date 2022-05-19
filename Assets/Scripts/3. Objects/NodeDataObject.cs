using System.Collections;
using UnityEngine;

public class NodeDataObject : MonoBehaviour
{
    protected NodeDataObject previousNode;
    [SerializeField] private NodeDataObject nextNode;
    [SerializeField] private NodeDataObject pairNode;
    [SerializeField] private NodeDataObject parentNode;
    [SerializeField] private bool hasBeenAssignedFighter;
    [SerializeField] private bool hasWonBattle;
    [SerializeField] private bool isFinalNode;
    [SerializeField] private int nodeIndex;
    [SerializeField] private bool active;

    [SerializeField] private FighterDataObject fighterData;
    private NodeUIController nodeUIController;
    [SerializeField] private FighterDataObject pairNodeFighterData;

    public bool Active { get => active; set => UpdateStatus(value); }
    public FighterDataObject FighterDataObject { get => fighterData; }
    public NodeUIController NodeUIController { get => nodeUIController; set => nodeUIController = value; }
    public NodeDataObject PreviousNode { get => previousNode; set => previousNode = value; }
    public NodeDataObject NextNode { get => nextNode; set => nextNode = value; }
    public NodeDataObject PairNode { get => pairNode; set => pairNode = value; }
    public NodeDataObject ParentNode { get => parentNode; set => parentNode = value; }
    public bool HasBeenAssignedFighter { get => hasBeenAssignedFighter; set => hasBeenAssignedFighter = value; }
    public bool HasWonBattle { get => hasWonBattle; set => hasWonBattle = value; }
    public bool IsFinalNode { get => isFinalNode; set => isFinalNode = value; }
    public int NodeIndex { get => nodeIndex; set => nodeIndex = value; }
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
        FighterStarter,
        Player,
        Opponent
    }

    public NodeType nodeType;

    public void Init(FighterDataObject fighter)
    {
        parentNode = transform.parent.GetComponent<NodeDataObject>();
        fighterData = fighter;

        nodeIndex = parentNode.NodeIndex;
        fighterData.FighterNodeIndex = nodeIndex;
    }

    public void SetActive()
    {
        this.enabled = true;
    }

    public void SetInactive()
    {
        this.enabled = false;
    }

    public void SetParentNode(NodeDataObject parentNode)
    {
        this.parentNode = parentNode;
        transform.SetParent(parentNode.transform);

        parentNode.fighterData = fighterData;
        fighterData.FighterNodeIndex = parentNode.NodeIndex;

        nextNode = parentNode.NextNode;
        pairNode = parentNode.PairNode;
        previousNode = parentNode.PreviousNode;
        nodeIndex = parentNode.NodeIndex;

        hasBeenAssignedFighter = parentNode.HasBeenAssignedFighter;
        hasWonBattle = parentNode.HasWonBattle;

        if(pairNode?.FighterDataObject != null)
            pairNodeFighterData = pairNode.FighterDataObject;

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

    private void UpdateStatus(bool value)
    {
        if(value)
        {

            return;
        }
        MoveToNextNode();

        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<NodeDataObject>().NodeUIController.SetInactive();
        }
        else
            Debug.Log(nodeIndex + " is missing a child object");
    }

}

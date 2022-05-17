using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] protected List<NodeDataObject> allNodes;
    [SerializeField] protected List<NodeDataObject> activeNodes;

    [Header("Debug")]
    [SerializeField] protected List<FighterPairObject> fighterPairs;

    public List<NodeDataObject> GetAllNodes() { return allNodes; }
    public List<NodeDataObject> GetAllActiveNodes() { return activeNodes; }

    //test
    public List<FighterPairObject> FighterPairs { get => fighterPairs; set => fighterPairs = value; }

    private void Awake()
    {
        fighterPairs = new List<FighterPairObject>();
        for (int i = 0; i < allNodes.Count; i++)
            allNodes[i].NodeIndex = i;
    }

    public void ProgressFighters()
    {
        List<NodeDataObject> tempList = new List<NodeDataObject>();
        tempList.AddRange(activeNodes);

        foreach (NodeDataObject currentNode in tempList)
        {
            if (currentNode != null)
            {
                NodeDataObject fighterNode = currentNode.transform.GetChild(0).GetComponent<NodeDataObject>();

                // if we have a winner
                if (fighterNode != null && currentNode.HasWonBattle)
                {
                    NodeDataObject newCurrentNode = currentNode.NextNode;
                    newCurrentNode.PreviousNode = currentNode;
                    fighterNode.UpdateToParentNode(newCurrentNode);
                    fighterNode.MoveToNextNode();
                    activeNodes.Remove(newCurrentNode.PreviousNode);
                    activeNodes.Add(newCurrentNode);
                    newCurrentNode.HasBeenAssignedFighter = true;
                }

                if(activeNodes.Contains(currentNode.PairNode) && !currentNode.PairNode.HasWonBattle)
                {
                    NodeDataObject otherNode = currentNode.PairNode;
                    if(otherNode.NextNode == currentNode.NextNode)
                    {
                        // --------------------------
                        otherNode.transform.GetChild(0).GetComponent<NodeDataObject>().NodeUIController.SetInactive();
                        activeNodes.Remove(otherNode);
                    }
                }
            }

        }
    }

    public void AssignActiveNodes()
    {
        activeNodes.Clear();

        foreach(NodeDataObject node in GetAllNodes())
            foreach(FighterDataObject fighter in GameManager.instance.Player.OtherFighters)
            {
                if (fighter.FighterNodeIndex == node.NodeIndex)
                    activeNodes.Add(node);
            }
    }

}

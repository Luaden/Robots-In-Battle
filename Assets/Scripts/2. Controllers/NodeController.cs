using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeController : MonoBehaviour
{
    [SerializeField] protected List<NodeDataObject> allNodes;
    [SerializeField] protected List<NodeDataObject> activeNodes;

    protected List<FighterPairObject> fighterPairs;

    public List<NodeDataObject> GetAllNodes() { return allNodes; }
    public List<NodeDataObject> GetAllActiveNodes() { return activeNodes; }

    //test
    public List<FighterPairObject> FighterPairs { get => fighterPairs; }

    private void Awake()
    {
        fighterPairs = new List<FighterPairObject>();
    }

    public void ProgressFighters()
    {
        List<NodeDataObject> tempList = new List<NodeDataObject>();
        tempList.AddRange(activeNodes);
        // test, assign the winners
        for (int i = 0; i < activeNodes.Count;)
        {
            activeNodes[i].HasWonBattle = true;
            //TournamentOverviewManager.instance.AssignFighterToNodeSlot(activeNodes[i], activeNodes[i].PairNode);
            i += 2;
        }


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
                    newCurrentNode.HasBeenAssigned = true;
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

}

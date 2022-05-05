using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeController : MonoBehaviour
{
    [SerializeField] protected List<NodeDataObject> allNodes;
    [SerializeField] protected List<NodeDataObject> activeNodes;

    //test
    [SerializeField] protected List<FighterPairObject> fighterPairs;

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
        //List<FighterDataObject> fighters = new List<FighterDataObject>();

        List<NodeDataObject> tempList = new List<NodeDataObject>();
        tempList.AddRange(activeNodes);

        for(int i = 0; i < activeNodes.Count;)
        {
            activeNodes[i].HasWonBattle = true;
            i += 2;
        }

        foreach (NodeDataObject currentNode in tempList)
        {
            if (currentNode != null)
            {
                NodeDataObject fighterNode = currentNode.transform.GetChild(0).GetComponent<NodeDataObject>();

                if (fighterNode != null && currentNode.HasWonBattle)
                {
                    fighterNode.MoveToNextNode();
                    activeNodes.Remove(currentNode.PreviousNode);
                    NodeDataObject newCurrentNode = currentNode.NextNode;
                    activeNodes.Add(newCurrentNode);
                    fighterNode.UpdateToParentNode(newCurrentNode);
                }

                NodeDataObject otherNode = activeNodes.FirstOrDefault(n => n.NextNode == currentNode.NextNode);
                if (otherNode != null)
                {
                    if (otherNode.NextNode == currentNode.NextNode)
                    {
                        // --------------
                        activeNodes.Remove(otherNode);
                    }
                }
            }

        }
    }




}

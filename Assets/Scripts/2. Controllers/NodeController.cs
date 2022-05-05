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

        foreach (NodeDataObject node in tempList)
        {
            if (node == null)
                continue;

            NodeDataObject fighterNode = node.transform.GetChild(0).GetComponent<NodeDataObject>();
            
            if(fighterNode != null)
                fighterNode.MoveToNextNode();
            activeNodes.Remove(node.PreviousNode);
            NodeDataObject otherNode = activeNodes.FirstOrDefault(n => n.NextNode == node.NextNode);
            if(otherNode != null)
            {
                if (otherNode.NextNode == node.NextNode)
                {
                    // --------------
                    activeNodes.Remove(otherNode);
                }
            }


        }
    }




}

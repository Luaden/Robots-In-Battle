using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                    currentNode.HasWonBattle = false;
                    currentNode.HasBeenAssignedFighter = false;

                    NodeDataObject newCurrentNode = currentNode.NextNode;
                    newCurrentNode.PreviousNode = currentNode;
                    newCurrentNode.HasBeenAssignedFighter = true;

                    fighterNode.SetParentNode(newCurrentNode);
                    fighterNode.MoveToNextNode();

                    activeNodes.Remove(newCurrentNode.PreviousNode);
                    activeNodes.Add(newCurrentNode);

                    FighterPairObject pairObj = fighterPairs.SingleOrDefault(p => p.FighterA == fighterNode.FighterDataObject || p.FighterB == fighterNode.FighterDataObject);
                    if (pairObj != null)
                    {
                        Debug.Log("removed: " + pairObj.FighterA.FighterNodeIndex);
                        Debug.Log("removed: " + pairObj.FighterB.FighterNodeIndex);
                        fighterPairs.Remove(pairObj);
                    }
                }

                NodeDataObject pairNode = currentNode.PairNode;

                if (!pairNode.HasWonBattle)
                {
                    if (pairNode.transform.childCount > 0)
                    {
                        // --------------------------
                        pairNode.Active = false;
                        activeNodes.Remove(pairNode);
                    }
                }
            }
        }

        NodeDataObject[] node = activeNodes.ToArray();
        for (int i = 0; i < node.Length - 1; i += 2)
            DowntimeManager.instance.TournamentManager.AssignFighterPairs(node[i], node[i + 1]);
    }

    public void AssignActiveNodes()
    {
        activeNodes.Clear();

        foreach(NodeDataObject node in GetAllNodes())
        {
            foreach (FighterDataObject fighter in GameManager.instance.Player.OtherFighters)
            {
                if (fighter.FighterNodeIndex == node.NodeIndex && node.HasWonBattle)
                    activeNodes.Add(node);
            }

            if (!activeNodes.Contains(node) && GameManager.instance.Player.PlayerFighterData.FighterNodeIndex == node.NodeIndex && node.HasWonBattle)
                activeNodes.Add(node);
        }

    }

    public void AssignWinners()
    {
        List<NodeDataObject> nodes = GetAllNodes();
        foreach(FighterPairObject fighterPair in FighterPairs)
        {
            foreach(NodeDataObject node in nodes)
            {
                if (node.PairNode == null || (node.PairNode.HasWonBattle || node.HasWonBattle) || node.PairNode.NodeIndex == node.NodeIndex)
                    continue;

                if(node.NodeIndex == fighterPair.FighterA.FighterNodeIndex || node.NodeIndex == fighterPair.FighterB.FighterNodeIndex)
                {
                    int fighterAWinChance = Random.Range(0, 101);
                    int fighterBWinChance = Random.Range(0, 101);

                    bool n = (fighterAWinChance > fighterBWinChance) ? true : false;

                    node.HasWonBattle = n;
                    node.PairNode.HasWonBattle = !n;

                    Debug.Log(node.NodeIndex + " has " + n + " won");
                    Debug.Log(node.PairNode.NodeIndex + " has " + !n + " won");
                }
            }

        }
    }

}

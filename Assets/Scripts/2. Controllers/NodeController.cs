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


    // moves the fighters on the tournament board
    public void ProgressFighters()
    {
        fighterPairs.Clear();
        List<NodeDataObject> tempList = new List<NodeDataObject>();
        tempList.AddRange(activeNodes);

        Debug.Log(tempList.Count);

        foreach (NodeDataObject activeNode in tempList)
        {
            if (activeNode != null)
            {
                if(activeNode.transform.childCount > 0)
                {
                    NodeDataObject fighterNode = activeNode.transform.GetChild(0).GetComponent<NodeDataObject>();

                    if(fighterNode.nodeType == NodeDataObject.NodeType.Player)
                    {
                        Debug.Log("progress fighters player node");

                        activeNode.HasWonBattle = false;
                        activeNode.HasBeenAssignedFighter = false;

                        NodeDataObject newCurrentNode = activeNode.NextNode;
                        newCurrentNode.PreviousNode = activeNode;
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

                        NodeDataObject playerPairNode = activeNode.PairNode;
                        playerPairNode.Active = false;
                        activeNodes.Remove(playerPairNode);

                        continue;
                    }

                    // if we have a winner
                    if (fighterNode != null && activeNode.HasWonBattle)
                    {
                        Debug.Log("we have a winner at: " + activeNode.NodeIndex);
                        activeNode.HasWonBattle = false;
                        activeNode.HasBeenAssignedFighter = false;

                        NodeDataObject newCurrentNode = activeNode.NextNode;
                        newCurrentNode.PreviousNode = activeNode;
                        newCurrentNode.HasBeenAssignedFighter = true;

                        fighterNode.SetParentNode(newCurrentNode);
                        if(fighterNode.Active)
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

                    NodeDataObject pairNode = activeNode.PairNode;

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
        }

        NodeDataObject[] node = activeNodes.ToArray();
        for (int i = 0; i < node.Length - 1; i += 2)
            DowntimeManager.instance.TournamentManager.AssignFighterPairs(node[i], node[i + 1]);
    }

    public void AssignActiveNodes()
    {
        activeNodes.Clear();

        foreach (NodeDataObject n in GetAllNodes())
            if (GameManager.instance.Player.PlayerFighterData.FighterNodeIndex == n.NodeIndex)
                activeNodes.Add(n);

        foreach(NodeDataObject node in GetAllNodes())
            foreach (FighterDataObject fighter in GameManager.instance.Player.OtherFighters)
                if (fighter.FighterNodeIndex == node.NodeIndex)
                    activeNodes.Add(node);


        List<NodeDataObject> tempList = new List<NodeDataObject>();
        tempList.AddRange(activeNodes);

        NodeDataObject.NodeType playerNodeType = GetAllNodes()[GameManager.instance.Player.PlayerFighterData.FighterNodeIndex].nodeType;
        Debug.Log("AssignActiveNodes_PlayerNodeType: " + playerNodeType);

        foreach (FighterDataObject fighter in GameManager.instance.Player.OtherFighters)
        {
            foreach (NodeDataObject a_node in tempList)
            {
                if (fighter.FighterNodeIndex == a_node.NodeIndex)
                {
                    Debug.Log("AssignActiveNodes_a_node: " + a_node.nodeType);

                    if (a_node.nodeType != playerNodeType)
                    {
                        activeNodes.Remove(a_node);
                        a_node.Active = false;
                    }
                }
            }
        }
    }

    public void AssignWinners()
    {
        List<NodeDataObject> nodes = GetAllNodes();

        Debug.Log("AssignWinners FighterPairs count: " + fighterPairs.Count);

        foreach(FighterPairObject fighterPair in FighterPairs)
        {
            foreach(NodeDataObject node in nodes)
            {
                NodeDataObject pairNode = node.PairNode;
                if (pairNode == null || (pairNode.HasWonBattle || node.HasWonBattle))
                    continue;

                FighterDataObject player = GameManager.instance.Player.PlayerFighterData;
                if (node.NodeIndex == player.FighterNodeIndex)
                {
                    node.HasBeenAssignedFighter = true;
                    pairNode.HasBeenAssignedFighter = true;

                    node.HasWonBattle = true;
                    pairNode.HasWonBattle = false;
                    continue;
                }

                if ((node.NodeIndex == fighterPair.FighterA.FighterNodeIndex || node.NodeIndex == fighterPair.FighterB.FighterNodeIndex))
                {
                    int fighterAWinChance = Random.Range(0, 101);
                    int fighterBWinChance = Random.Range(0, 101);

                    if(fighterAWinChance >= fighterBWinChance)
                    {
                        node.HasBeenAssignedFighter = true;
                        pairNode.HasBeenAssignedFighter = true;

                        node.HasWonBattle = true;
                        pairNode.HasWonBattle = false;

                        Debug.Log(node.NodeIndex + " has won");
                        Debug.Log(pairNode.NodeIndex + " has lost");
                        continue;
                    }
                    if (fighterAWinChance < fighterBWinChance)
                    {
                        pairNode.HasBeenAssignedFighter = true;
                        node.HasBeenAssignedFighter = true;

                        pairNode.HasWonBattle = true;
                        node.HasWonBattle = false;

                        Debug.Log(pairNode.NodeIndex + " has won");
                        Debug.Log(node.NodeIndex + " has lost");
                        continue;
                    }
                }
            }
        }
    }
}

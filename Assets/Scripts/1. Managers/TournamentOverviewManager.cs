using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

public class TournamentOverviewManager : MonoBehaviour
{
    public static TournamentOverviewManager instance;

    private NodeSlotManager nodeSlotManager;
    private NodeController nodeController;
    [SerializeField] protected GameObject pilotPrefab;

    private bool playedFirstBattle;
    public NodeSlotManager NodeSlotManager { get => nodeSlotManager; }
    /* 
     * 
     * reference to playerfights
    Node tree 
    * List<nodes> containing info about fighters, placements
        * each node contains parent/previous node
            * fighter pair objects
     UI controllers to be able to move player & enemy 
    
    fight button for completing task

    stats displayed (hp, money, time, ??)

    pilots that can be dragged to first set of nodes
     
     */
    private void OnDestroy()
    {
        Destroy(NodeSlotManager);
    }

    private void Awake()
    {
        nodeSlotManager = FindObjectOfType<NodeSlotManager>();
        nodeController = FindObjectOfType<NodeController>();

        if (instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    private void Start()
    {
        // test
        bool player = true;

        foreach(NodeDataObject n in nodeController.GetAllNodes())
        {
            if (n.nodeType == NodeDataObject.NodeType.Starter)
                nodeController.GetAllActiveNodes().Add(n);

            if(n.nodeType == NodeDataObject.NodeType.FighterStarter)
            {
                GameObject nodeUIGameObject;
                nodeUIGameObject = Instantiate(pilotPrefab, n.transform.position, Quaternion.identity, n.transform);
                // set the UI object of the pilot data to be the gameobject
                
                NodeUIController nodeUIObject = nodeUIGameObject.GetComponent<NodeUIController>();
                n.NodeUIController = nodeUIObject;
                NodeDataObject nodeDataObject = nodeUIGameObject.GetComponent<NodeDataObject>();
                if (player)
                {
                    nodeDataObject.nodeType = NodeDataObject.NodeType.Pilot;
                    player = false;
                }
                else 
                    nodeDataObject.nodeType = NodeDataObject.NodeType.Opponent;

                nodeSlotManager.AddItemToCollection(nodeUIObject, n.GetComponent<NodeSlotController>());
                nodeDataObject.Init();
                nodeUIObject.InitUI(nodeDataObject);

                nodeUIGameObject.SetActive(true);
            }
        }

    }
    public void AddToActiveList(NodeDataObject node)
    {
        nodeController.GetAllActiveNodes().Add(node);
    }
    public void RemoveInActiveList(NodeDataObject node)
    {
        nodeController.GetAllActiveNodes().Remove(node);
    }
    public List<NodeDataObject> GetActiveList()
    {
        return nodeController.GetAllActiveNodes();
    }

    // FighterData?
    public void AssignFighterToNodeSlot(NodeDataObject nodeA, NodeDataObject nodeB)
    {
        FighterPairObject fighterPairObject = new FighterPairObject(nodeA.FighterDataObject, nodeB.FighterDataObject);
        nodeController.FighterPairs.Add(fighterPairObject);
    }

    public void GoToBattle()
    {
        // press the button, change scene to battle
        // who wins?

        if(nodeController.GetAllActiveNodes().Any(n => n.HasBeenAssigned == false))
        {
            Debug.Log("pairs are not completed");
            return;
        }

        
        nodeController.ProgressFighters();

        foreach (NodeDataObject node in nodeController.GetAllNodes())
        {
            if (GetActiveList().Contains(node))
                RemoveInActiveList(node);
            /*            if (node.HasBeenAssigned)
                            AddToActiveList(node);*/
        }

        /*if(!playedFirstBattle)
        {
            foreach (NodeDataObject n in nodeController.GetAllActiveNodes())
            {
                // if the node has a child (fighter object), set its ui controller to be disabled
                if (n.transform.GetChild(0) != null)
                    n.transform.GetChild(0).GetComponent<NodeUIController>().SetInactive();
            }

            playedFirstBattle = true;
        }
         
        List<NodeDataObject> newActiveNodes = new List<NodeDataObject>();

        for (int i = 0; i < nodeController.GetAllActiveNodes().Count; i++)
        {
            NodeDataObject currentNode = nodeController.GetAllActiveNodes()[i];
            // testing
            // if i is even
            if (i % 2 == 0)
            {
                // assigning it to have won the battle
                currentNode.HasWonBattle = true;

                // the nodes previous node is the current node
                // the pilot (player/opponent)
                NodeDataObject fighterObj = currentNode.transform.GetChild(0).GetComponent<NodeDataObject>();
                // set the fighters current node to be this node
                fighterObj.CurrentNode = currentNode;
                // change its parent to be the next node
                fighterObj.transform.SetParent(currentNode.NextNode.transform);
                // change its position to be the next nodes position
                fighterObj.transform.position = currentNode.NextNode.transform.position;

                // assign fighters slotcontroller to be the next nodes
                fighterObj.GetComponent<NodeUIController>().NodeSlotController = currentNode.NextNode.GetComponent<NodeSlotController>();
                // the next node has been assigned a pilot
                currentNode.NextNode.HasBeenAssigned = true;
                // the next nodes previous node is this current node
                currentNode.NextNode.PreviousNode = currentNode;
                // add next node to active list
                //newActiveNodes.Add(currentNode.NextNode);
                fighterObj.GetComponent<Image>().color = Color.green;
            }
            else
            {
                // if i is uneven
                // assign it to have lost battle
                currentNode.HasWonBattle = false;
                NodeDataObject pilotObject = currentNode.transform.GetChild(0).GetComponent<NodeDataObject>();
                pilotObject.GetComponent<Image>().color = Color.red;
            }
        }

        // is the final node in the active nodes
        if (nodeController.GetAllActiveNodes().Any(x => x.IsFinalNode))
        {
            Debug.Log("final boss");
            return;
        }

        Debug.Log("battle performed");
*/
    }

    // for testing, assigning fighters on startup
/*    public void AssignAllFightersToStarterNodes()
    {
        
        foreach(NodeDataObject n in nodeController.GetAllNodes())
        {
            // the node where the fighter starts..
            if (n.nodeType == NodeDataObject.NodeType.FighterStarter)
            {
                // get all nodes
                for(int i = 0; i < nodeController.GetAllActiveNodes().Count; i++)
                {
                    // get the starter nodes.. where the fighters are supposed to fit in
                    NodeDataObject starterNode = nodeController.GetAllActiveNodes()[i];
                    if (starterNode.HasBeenAssigned || n.transform.childCount < 1)
                        continue;

                    NodeDataObject fighterObj = n.transform.GetChild(0).GetComponent<NodeDataObject>();
                    fighterObj.CurrentNode = starterNode;
                    fighterObj.transform.SetParent(starterNode.transform);
                    fighterObj.transform.position = starterNode.transform.position;
                    fighterObj.GetComponent<NodeUIController>().NodeSlotController = starterNode.GetComponent<NodeSlotController>();
                    starterNode.HasBeenAssigned = true;
                }
            }
        }
    }*/

}

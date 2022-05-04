using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TournamentOverviewManager : MonoBehaviour
{
    public static TournamentOverviewManager instance;

    private NodeSlotManager nodeSlotManager;
    private NodeController nodeController;
    [SerializeField] protected GameObject pilotPrefab;

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

            if(n.nodeType == NodeDataObject.NodeType.PilotStarter)
            {
                GameObject nodeUIGameObject;
                nodeUIGameObject = Instantiate(pilotPrefab, n.transform.position, Quaternion.identity, n.transform);
                // set the UI object of the pilot data to be the gameobject
                
                n.NodeUIController = nodeUIGameObject;
                NodeUIController nodeUIObject = nodeUIGameObject.GetComponent<NodeUIController>();
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

        AssignAllPilotsToStarterNodes();

    }

    public void AssignFighterToNodeSlot(NodeDataObject nodeData)
    {
/*        FighterPairObject fighterPairObject = new FighterPairObject(nodeData.FighterDataObject, nodeData.FighterDataObject);
        nodeController.FighterPairs.Add(fighterPairObject);*/
    }

    public void NextBattle()
    {
        if(nodeController.GetAllActiveNodes().Any(n => n.IsCompleted == false))
        {
            Debug.Log("pair is not completed");
            return;
        }

        List<NodeDataObject> newActiveNodes = new List<NodeDataObject>();


        for (int i = 0; i < nodeController.GetAllActiveNodes().Count; i++)
        {
            // active node
            NodeDataObject currentNode = nodeController.GetAllActiveNodes()[i];
            currentNode.HasWon = true;
            // pair node has lost..


            currentNode.PreviousNode = currentNode;
            NodeDataObject pilotObject = currentNode.transform.GetChild(0).GetComponent<NodeDataObject>();
            pilotObject.transform.SetParent(currentNode.NextNode.transform);
            pilotObject.transform.position = currentNode.NextNode.transform.position;
            pilotObject.GetComponent<NodeUIController>().NodeSlotController = currentNode.NextNode.GetComponent<NodeSlotController>();

            newActiveNodes.Add(currentNode.NextNode);
            /*            nodeController.GetAllActiveNodes().Add(currentNode.NextNode);

                        NodeDataObject pairNode = nodeController.GetAllActiveNodes()[i].PairNode;
                        nodeController.GetAllActiveNodes().Remove(pairNode);

                        nodeController.GetAllActiveNodes().Remove(currentNode);*/
        }

        nodeController.GetAllActiveNodes().Clear();
        nodeController.GetAllActiveNodes().AddRange(newActiveNodes);

/*        for(int i = 0; i < nodeController.GetAllNodes().Count; i++)
        {
            if (nodeController.GetAllNodes()[i].HasWon)
                nodeController.GetAllActiveNodes().Add(nodeController.GetAllNodes()[i]);
        }
*/

        Debug.Log("battle performed");

    }

    // for testing
    public void AssignAllPilotsToStarterNodes()
    {
        
        foreach(NodeDataObject n in nodeController.GetAllNodes())
        {
            // the node where pilot starts..
            if(n.nodeType == NodeDataObject.NodeType.PilotStarter)
            {

                // get all nodes
                for(int i = 0; i < nodeController.GetAllActiveNodes().Count; i++)
                {
                    // get the starter nodes.. where they are supposed to fit in
                    NodeDataObject starterNode = nodeController.GetAllActiveNodes()[i];
                    if (starterNode.isCompleted || n.transform.childCount < 1)
                        continue;

                    NodeDataObject pilotObject = n.transform.GetChild(0).GetComponent<NodeDataObject>();
                    pilotObject.transform.SetParent(starterNode.transform);
                    pilotObject.transform.position = starterNode.transform.position;
                    pilotObject.GetComponent<NodeUIController>().NodeSlotController = starterNode.GetComponent<NodeSlotController>();
                    starterNode.isCompleted = true;
                }


/*                foreach(NodeDataObject starterNode in nodeController.GetAllActiveNodes())
                {
                    NodeDataObject pilotObject = n.transform.GetChild(0).GetComponent<NodeDataObject>();
                    pilotObject.transform.position = starterNode.transform.position;
                    pilotObject.transform.SetParent(starterNode.transform);
                    pilotObject.GetComponent<NodeUIController>().NodeSlotController = starterNode.GetComponent<NodeSlotController>();
                    starterNode.isCompleted = true;
                }*/
            }
        }
    }
}

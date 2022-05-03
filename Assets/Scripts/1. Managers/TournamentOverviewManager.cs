using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            if(n.nodeType == NodeDataObject.NodeType.None)
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
    }

    public void AssignFighterToNodeSlot(NodeDataObject nodeData)
    {
/*        FighterPairObject fighterPairObject = new FighterPairObject(nodeData.FighterDataObject, nodeData.FighterDataObject);
        nodeController.FighterPairs.Add(fighterPairObject);*/
    }
}

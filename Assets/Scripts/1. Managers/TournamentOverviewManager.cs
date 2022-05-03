using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentOverviewManager : MonoBehaviour
{
    private NodeSlotManager nodeSlotManager;
    private NodeController nodeController;
    [SerializeField] protected GameObject pilotGameObject;
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
    }
    private void Start()
    {
        // test
        foreach(NodeDataObject n in nodeController.GetAllNodes())
        {
            if(n.nodeType == NodeDataObject.NodeType.None)
            {
                GameObject newGameObject = Instantiate(pilotGameObject, n.transform.position, Quaternion.identity, n.transform);
                newGameObject.GetComponent<NodeUIController>().InitUI(n);
                newGameObject.GetComponent<NodeUIController>().NodeSlotController = n.GetComponent<NodeSlotController>();
                //Instantiate(pilotGameObject, n.transform.position, Quaternion.identity, n.transform);
                nodeSlotManager.AddItemToCollection(newGameObject.GetComponent<NodeUIController>(), n.GetComponent<NodeSlotController>());
            }
        }
        Destroy(pilotGameObject);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class TournamentOverviewManager : MonoBehaviour
{
    public static TournamentOverviewManager instance;

    private NodeSlotManager nodeSlotManager;
    private NodeController nodeController;
    private PopupUIManager popupUIManager;

    [SerializeField] protected GameObject pilotPrefab;
    [SerializeField] protected TMP_Text playerMoney;
    [SerializeField] protected TMP_Text playerHealth;
    [SerializeField] protected TMP_Text playerTime;


    public NodeSlotManager NodeSlotManager { get => nodeSlotManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

    private void OnDestroy()
    {
        Destroy(NodeSlotManager);
    }

    private void Awake()
    {

        if (instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        nodeSlotManager = FindObjectOfType<NodeSlotManager>();
        nodeController = FindObjectOfType<NodeController>();
        popupUIManager = FindObjectOfType<PopupUIManager>();

        

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
                NodeDataObject nodeDataObject = nodeUIGameObject.GetComponent<NodeDataObject>();
                if (player)
                {
                    nodeDataObject.nodeType = NodeDataObject.NodeType.Player;
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

        //AssignAllFightersToStarterNodes();
        DisplayStatsOverview();

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

    }

    // for testing, assigning fighters on startup
    public void AssignAllFightersToStarterNodes()
    {

        foreach (NodeDataObject n in nodeController.GetAllNodes())
        {
            // the node where the fighter starts..
            if (n.nodeType == NodeDataObject.NodeType.FighterStarter)
            {
                // get all nodes
                for (int i = 0; i < nodeController.GetAllActiveNodes().Count; i++)
                {
                    // get the starter nodes.. where the fighters are supposed to fit in
                    NodeDataObject starterNode = nodeController.GetAllActiveNodes()[i];
                    if (starterNode.HasBeenAssigned || n.transform.childCount < 1)
                        continue;

                    NodeDataObject fighterObj = n.transform.GetChild(0).GetComponent<NodeDataObject>();
                    fighterObj.ParentNode = starterNode;
                    fighterObj.transform.SetParent(starterNode.transform);
                    fighterObj.transform.position = starterNode.transform.position;
                    fighterObj.GetComponent<NodeUIController>().NodeSlotController = starterNode.GetComponent<NodeSlotController>();
                    starterNode.HasBeenAssigned = true;
                }
            }
        }
    }

    public void DisplayStatsOverview() 
    {
        if (GameManager.instance == null)
            return;

        // test
        GameManager.instance.LoadPlayer();
        Debug.Log(GameManager.instance.PlayerBankController);

        playerMoney.text = ("Money: ") + GameManager.instance.PlayerBankController.GetPlayerCurrency().ToString();
        playerHealth.text = ("Health: ") +GameManager.instance.PlayerMechController.PlayerMech.MechCurrentHP.ToString();
        playerTime.text = ("Time: ") + GameManager.instance.PlayerBankController.GetPlayerTime().ToString();
    }

}

using System.Collections.Generic;
using UnityEngine;
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

    [Header("Tournament Overview Stats")]
    [SerializeField] protected Image playerMoneyImage;
    [SerializeField] protected TMP_Text playerMoneyText;
    [SerializeField] protected Image playerHealthImage;
    [SerializeField] protected TMP_Text playerHealthText;
    [SerializeField] protected Image playerTimeImage;
    [SerializeField] protected TMP_Text playerTimeText;


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
        bool player = true;
        int number = 0;
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

                nodeDataObject.NodeIndex = number++;
                nodeDataObject.name = "pilot " + number;
                // add item to the fighter starter slots
                nodeSlotManager.AddItemToCollection(nodeUIObject, n.GetComponent<NodeSlotController>());
                nodeDataObject.Init();
                nodeUIObject.InitUI(nodeDataObject);

                nodeUIGameObject.SetActive(true);
            }
        }

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

    public void AssignFighterPairs(NodeDataObject nodeA, NodeDataObject nodeB)
    {
        FighterPairObject fighterPairObject = new FighterPairObject(nodeA.FighterDataObject, nodeB.FighterDataObject);
        nodeController.FighterPairs.Add(fighterPairObject);
    }

    public void GoToBattle()
    {
        nodeController.FighterPairs.Clear();
        // press the button, confirm all the changes

        if(nodeController.GetAllActiveNodes().Any(n => n.HasBeenAssigned == false))
        {
            Debug.Log("pairs are not completed");
            return;
        }
        NodeDataObject[] node = GetActiveList().ToArray();
        for (int i = 0; i < node.Length - 1; i += 2)
        {
            AssignFighterPairs(node[i], node[i + 1]);
        }

        nodeController.ProgressFighters();

    }
    public void DisplayStatsOverview() 
    {
        if (GameManager.instance == null)
            return;

        // test
        GameManager.instance.LoadPlayer();
        Debug.Log(GameManager.instance.PlayerBankController);

        playerMoneyText.text = GameManager.instance.PlayerBankController.GetPlayerCurrency().ToString();
        playerHealthText.text = GameManager.instance.PlayerMechController.PlayerMech.MechCurrentHP.ToString();
        playerTimeText.text = GameManager.instance.PlayerBankController.GetPlayerTime().ToString() + (" days ");
    }

}

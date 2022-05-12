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
        InitTournamentScreen();
    }

    public void InitTournamentScreen()
    {
        foreach (NodeDataObject n in nodeController.GetAllNodes())
        {
            if (n.nodeType == NodeDataObject.NodeType.Starter)
                nodeController.GetAllActiveNodes().Add(n);

            if (n.nodeType == NodeDataObject.NodeType.FighterStarter)
            {
                GameObject nodeUIGameObject;
                nodeUIGameObject = Instantiate(pilotPrefab, n.transform.position, Quaternion.identity, n.transform);
                // set the UI object of the pilot data to be the gameobject

                NodeUIController nodeUIObject = nodeUIGameObject.GetComponent<NodeUIController>();
                NodeDataObject nodeDataObject = nodeUIGameObject.GetComponent<NodeDataObject>();

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
        NodeDataObject childA = nodeA.transform.GetChild(0).GetComponent<NodeDataObject>();
        NodeDataObject childB = nodeB.transform.GetChild(0).GetComponent<NodeDataObject>();


        childA.FighterDataObject.FighterNodeIndex = nodeA.NodeIndex;
        childB.FighterDataObject.FighterNodeIndex = nodeB.NodeIndex;

        FighterPairObject fighterPairObject = new FighterPairObject(childA.FighterDataObject, childB.FighterDataObject);

        nodeController.FighterPairs.Add(fighterPairObject);
    }

    public void ConfirmBattleChoices()
    {
        nodeController.FighterPairs.Clear();
        nodeController.FighterPairs = new List<FighterPairObject>();

        if(nodeController.GetAllActiveNodes().Any(n => n.HasBeenAssignedFighter == false))
        {
            Debug.Log("pairs are not completed");
            return;
        }
        NodeDataObject[] node = GetActiveList().ToArray();
        for (int i = 0; i < node.Length - 1; i += 2)
        {
            AssignFighterPairs(node[i], node[i + 1]);
        }

        // should be called when battles have been completed
        //nodeController.ProgressFighters();

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

    public List<FighterPairObject> GetFighterPairs()
    {
        return nodeController.FighterPairs;
    }

}

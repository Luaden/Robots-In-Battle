using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class TournamentManager : MonoBehaviour
{
    private NodeSlotManager nodeSlotManager;
    private NodeController nodeController;
    private PopupUIManager popupUIManager;

    [SerializeField] protected GameObject tournamentPanel;
    [SerializeField] protected GameObject shopPanel;
    [SerializeField] protected GameObject pilotPrefab;

    public NodeSlotManager NodeSlotManager { get => nodeSlotManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

    private void OnDestroy()
    {
        Destroy(NodeSlotManager);
    }

    private void Awake()
    {
        nodeSlotManager = FindObjectOfType<NodeSlotManager>(true);
        nodeController = FindObjectOfType<NodeController>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
    }

    private void Start()
    {
        InitTournamentScreen();
    }

    public void InitTournamentScreen()
    {
        bool player = true;
        int fighterIndex = 0;

        for(int i = 0; i < nodeController.GetAllNodes().Count; i++)
        {
            NodeDataObject node = nodeController.GetAllNodes()[i];
            if (node.nodeType == NodeDataObject.NodeType.Starter)
                nodeController.GetAllActiveNodes().Add(node);

            if (node.nodeType == NodeDataObject.NodeType.FighterStarter)
            {
                GameObject nodeUIGameObject;
                nodeUIGameObject = Instantiate(pilotPrefab, node.transform.position, Quaternion.identity, node.transform);
                // set the UI object of the pilot data to be the gameobject

                NodeUIController nodeUIObject = nodeUIGameObject.GetComponent<NodeUIController>();
                NodeDataObject nodeDataObject = nodeUIGameObject.GetComponent<NodeDataObject>();

                // add item to the fighter starter slots
                nodeSlotManager.AddItemToCollection(nodeUIObject, node.GetComponent<NodeSlotController>());

                FighterDataObject fighter;
                if (player)
                {
                    fighter = GameManager.instance.Player.PlayerFighterData;
                    player = false;
                }
                else
                {
                    fighter = GameManager.instance.Player.OtherFighters[fighterIndex];
                    fighterIndex++;
                }

                nodeDataObject.Init(fighter);
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
            Debug.Log("You have not assigned all fighters to a slot. Fighter Pairs are not completed");
            return;
        }
        NodeDataObject[] node = GetActiveList().ToArray();
        for (int i = 0; i < node.Length - 1; i += 2)
        {
            AssignFighterPairs(node[i], node[i + 1]);
        }

        OnAssignAllFighters();
    }

    public List<FighterPairObject> GetFighterPairs()
    {
        return nodeController.FighterPairs;
    }

    public void SetFighterPairs(List<FighterPairObject> fighterPairs)
    {
        nodeController.FighterPairs = fighterPairs;
    }

    public void OpenTournamentScreen()
    {
        if(!tournamentPanel.activeInHierarchy)
        {
            tournamentPanel.SetActive(true);
            shopPanel.SetActive(false);
        }
        else
        {
            tournamentPanel.SetActive(false);
            shopPanel.SetActive(true);
        }
    }

    public void OnAssignAllFighters()
    {
        Debug.Log("All fighters have been assigned to a slot!");
    }

}

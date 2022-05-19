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
    [SerializeField] protected GameObject fightButton;

    public NodeSlotManager NodeSlotManager { get => nodeSlotManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

    private void Awake()
    {
        nodeSlotManager = FindObjectOfType<NodeSlotManager>(true);
        nodeController = FindObjectOfType<NodeController>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
    }

    private void Start()
    {
        AssignNodeIndices();

        if (GameManager.instance.Player.CurrentWinCount == 0 && GameManager.instance.Player.OtherFighters.All(opponent => opponent.FighterNodeIndex == 0))
            InitTournamentScreen();
        else
        {
            Debug.Log("start");
            InitFighters();
            nodeController.AssignWinners();
            nodeController.AssignActiveNodes();
            nodeController.ProgressFighters();

            //fightButton.SetActive(false);
        }
    }

    private void AssignNodeIndices()
    {
        for (int i = 0; i < nodeController.GetAllNodes().Count; i++)
            nodeController.GetAllNodes()[i].NodeIndex = i;
    }

    private void InitFighters()
    {
        int fighterIndex = 0;
        bool player = true;

        for (int j = 0; j < GameManager.instance.Player.OtherFighters.Count; j++)
            GameManager.instance.Player.OtherFighters[j].FighterNodeIndex = j;

        GameManager.instance.Player.PlayerFighterData.FighterNodeIndex = 7;

        int playerFighterIndex = GameManager.instance.Player.PlayerFighterData.FighterNodeIndex;
        AddToActiveList(nodeController.GetAllNodes()[playerFighterIndex]);

        for (int i = 0; i < nodeController.GetAllNodes().Count; i++)
        {
            NodeDataObject node = nodeController.GetAllNodes()[i];
            if (GameManager.instance.Player.OtherFighters.Any(opponent => opponent.FighterNodeIndex == node.NodeIndex))
                AddToActiveList(node);
        }

        for(int k = 0; k < GetActiveList().Count; k++)
        {
            NodeDataObject node = GetActiveList()[k];
            GameObject nodeUIGameObject;
            nodeUIGameObject = Instantiate(pilotPrefab, node.transform.position, Quaternion.identity, node.transform);

            NodeUIController nodeUIObject = nodeUIGameObject.GetComponent<NodeUIController>();
            NodeDataObject nodeDataObject = nodeUIGameObject.GetComponent<NodeDataObject>();

            nodeSlotManager.AddItemToCollection(nodeUIObject, node.GetComponent<NodeSlotController>());

            FighterDataObject fighter;
            if (player)
            {
                nodeDataObject.nodeType = NodeDataObject.NodeType.Player;
                fighter = GameManager.instance.Player.PlayerFighterData;
                nodeDataObject.pilotType = fighter.FighterCompleteCharacter.PilotType;
                player = false;
            }
            else
            {
                nodeDataObject.nodeType = NodeDataObject.NodeType.Opponent;
                fighter = GameManager.instance.Player.OtherFighters[fighterIndex];
                nodeDataObject.pilotType = fighter.FighterCompleteCharacter.PilotType;
                fighterIndex++;
            }

            nodeDataObject.Init(fighter);
            nodeDataObject.HasBeenAssignedFighter = true;

            nodeUIObject.InitUI(nodeDataObject);

            nodeUIGameObject.SetActive(true);
        }

        foreach (NodeDataObject node in GetActiveList())
        {
            if (!GetFighterPairs().Any(n => n.FighterA.FighterNodeIndex == node.NodeIndex || n.FighterB.FighterNodeIndex == node.NodeIndex))
                AssignFighterPairs(node, node.PairNode);
        }

    }
    private void InitTournamentScreen()
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
                    nodeDataObject.nodeType = NodeDataObject.NodeType.Player;
                    fighter = GameManager.instance.Player.PlayerFighterData;
                    player = false;
                }
                else
                {
                    nodeDataObject.nodeType = NodeDataObject.NodeType.Opponent;
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
        if (nodeController.GetAllActiveNodes().Any(fighterNode => fighterNode.HasBeenAssignedFighter == false))
        {
            Debug.Log("You have not assigned all fighters to a slot. Fighter Pairs are not completed");
            return;
        }

        if (GameManager.instance.Player.CurrentWinCount > 0)
        {
            nodeController.AssignWinners();
            nodeController.AssignActiveNodes();
        }
        else
        {
            NodeDataObject[] node = GetActiveList().ToArray();
            for (int i = 0; i < node.Length - 1; i += 2)
                AssignFighterPairs(node[i], node[i + 1]);
        }

        OnAssignAllFighters();
        nodeController.ProgressFighters();
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

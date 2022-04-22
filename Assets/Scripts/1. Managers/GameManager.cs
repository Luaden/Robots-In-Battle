using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private MechBuilderController mechBuilderController;
    private DeckBuilderController deckBuilderController;

    public MechBuilderController MechBuilderController { get => mechBuilderController; }
    public DeckBuilderController DeckBuilderController { get => deckBuilderController; }

    #region Playtesting
    [SerializeField] private SOItemDataObject starterMechHead;
    [SerializeField] private SOItemDataObject starterMechTorso;
    [SerializeField] private SOItemDataObject starterMechArms;
    [SerializeField] private SOItemDataObject starterMechLegs;
    [SerializeField] private List<SOItemDataObject> starterDeck;

    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        MechObject playerMech = mechBuilderController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerMech, playerPilot, deckBuilderController.BuildDeck(starterDeck));



        MechObject opponentMech = mechBuilderController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        PilotDataObject opponentPilot = new PilotDataObject();
        FighterDataObject opponentFighter = new FighterDataObject(opponentMech, opponentPilot, deckBuilderController.BuildDeck(starterDeck));

        CombatManager.instance.PlayerFighter = playerFighter;
        CombatManager.instance.MechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
        CombatManager.instance.MechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy);
        CombatManager.instance.OpponentFighter = opponentFighter;
        CombatManager.instance.MechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
        CombatManager.instance.MechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy);

        DrawCardPrefab();
    }

    private void DrawCardPrefab()
    {
        CombatManager.instance.DeckManager.SetPlayerDeck(starterDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawPlayerCard();

        CombatManager.instance.DeckManager.SetOpponentDeck(starterDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawOpponentCard();

    }
    #endregion

    private InventoryManager inventoryManager;
    private PlayerDataObject playerData;

    public static GameManager instance;

    public InventoryManager InventoryController { get => inventoryManager; }
    public PlayerDataObject PlayerData { get => playerData; set => playerData = value; }
    

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        mechBuilderController = new MechBuilderController();
        deckBuilderController = new DeckBuilderController();
    }

    public void LoadPlayer(PlayerDataObject playerDataObject = null)
    {
        if (playerDataObject != null)
        {
            playerData = playerDataObject;
            return;
        }

        playerData = new PlayerDataObject();
        MechObject newMech = mechBuilderController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        List<CardDataObject> newDeck = deckBuilderController.BuildDeck(starterDeck);

        playerData.PlayerDeck = newDeck;
        playerData.PlayerMech = newMech;
    }
}

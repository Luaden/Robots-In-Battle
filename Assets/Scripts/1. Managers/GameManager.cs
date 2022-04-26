using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private DowntimeMechBuilderController playerMechController;
    private DowntimeInventoryController playerInventoryController;
    private DowntimeDeckController playerDeckController;
    private DowntimeBankController playerBankController;
    private PlayerDataObject playerData;

    public static GameManager instance;

    public DowntimeInventoryController PlayerInventoryController { get => playerInventoryController; }
    public DowntimeMechBuilderController PlayerMechController { get => playerMechController; }
    public DowntimeDeckController PlayerDeckController { get => playerDeckController; }
    public DowntimeBankController PlayerBankController { get => playerBankController; }

    #region Playtesting
    [SerializeField] private SOItemDataObject starterMechHead;
    [SerializeField] private SOItemDataObject starterMechTorso;
    [SerializeField] private SOItemDataObject starterMechArms;
    [SerializeField] private SOItemDataObject starterMechLegs;
    [SerializeField] private List<SOItemDataObject> starterDeck;


    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        LoadPlayer();
        PlayerMechController.BuildNewPlayerMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerData.PlayerMech, playerPilot, starterDeck);



        MechObject opponentMech = PlayerMechController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        PilotDataObject opponentPilot = new PilotDataObject();
        FighterDataObject opponentFighter = new FighterDataObject(opponentMech, opponentPilot, starterDeck);

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
        playerInventoryController = new DowntimeInventoryController();
        playerMechController = new DowntimeMechBuilderController();
        playerDeckController = new DowntimeDeckController();
    }

    public void LoadPlayer(PlayerDataObject playerDataObject = null)
    {
        if (playerDataObject != null)
        {
            playerData = playerDataObject;
            return;
        }

        playerData = new PlayerDataObject();
        MechObject newMech = PlayerMechController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);

        playerData.PlayerDeck = starterDeck;
        playerData.PlayerMech = newMech;
    }


    public class DowntimeDeckController
    {
        public List<SOItemDataObject> PlayerDeck { get => instance.playerData.PlayerDeck; }

        public void AddCardToPlayerDeck(SOItemDataObject newCard)
        {
            List<SOItemDataObject> currentDeck = new List<SOItemDataObject>(PlayerDeck);

            if (newCard.ItemType != ItemType.Card)
            {
                Debug.Log("A component was attempted to be added to the player deck, but this was an incorrect location for it.");
                Debug.Log("Is " + newCard.ItemType + " the correct ItemType for " + newCard.ItemName + "?");
                return;
            }

            currentDeck.Add(newCard);

            instance.playerData.PlayerDeck = currentDeck;
        }

        public void RemoveCardFromPlayerDeck(SOItemDataObject newCard)
        {
            List<SOItemDataObject> currentDeck = new List<SOItemDataObject>(PlayerDeck);

            if (!PlayerDeck.Contains(newCard))
            {
                Debug.Log("Attempted to remove " + newCard.ComponentName + " from the player deck, but it was not found there.");
                return;
            }

            currentDeck.Remove(newCard);

            instance.playerData.PlayerDeck = currentDeck;
        }
    }

    public class DowntimeMechBuilderController
    {
        public MechObject BuildNewMech(SOItemDataObject mechHead, SOItemDataObject mechTorso, SOItemDataObject mechArms, SOItemDataObject mechLegs)
        {
            MechComponentDataObject head = new MechComponentDataObject(mechHead);
            MechComponentDataObject torso = new MechComponentDataObject(mechTorso);
            MechComponentDataObject legs = new MechComponentDataObject(mechLegs);
            MechComponentDataObject arms = new MechComponentDataObject(mechArms);

            MechObject newMech = new MechObject(head, torso, arms, legs);

            return newMech;
        }

        public void BuildNewPlayerMech(SOItemDataObject mechHead, SOItemDataObject mechTorso, SOItemDataObject mechArms, SOItemDataObject mechLegs)
        {
            MechComponentDataObject head = new MechComponentDataObject(mechHead);
            MechComponentDataObject torso = new MechComponentDataObject(mechTorso);
            MechComponentDataObject legs = new MechComponentDataObject(mechLegs);
            MechComponentDataObject arms = new MechComponentDataObject(mechArms);
            MechObject newMech = new MechObject(head, torso, arms, legs);

            instance.playerData.PlayerMech = newMech;
        }

        public void SwapPlayerMechPart(SOItemDataObject SOMechComponentDataObject)
        {
            MechComponentDataObject newComponent = new MechComponentDataObject(SOMechComponentDataObject);

            SwapPlayerMechPart(newComponent);
        }

        public void SwapPlayerMechPart(MechComponentDataObject newComponent)
        {
            MechComponentDataObject oldComponent;

            oldComponent = instance.playerData.PlayerMech.ReplaceComponent(newComponent);
            instance.PlayerInventoryController.AddItemToInventory(oldComponent);
        }
    }

    public class DowntimeInventoryController
    {
        public void AddItemToInventory(SOItemDataObject mechComponent)
        {
            if (mechComponent.ItemType != ItemType.Component)
            {
                Debug.Log("A card was attempted to be added to the inventory, but this was an incorrect location for it.");
                Debug.Log("Is " + mechComponent.ItemType + " the correct ItemType for " + mechComponent.ItemName + "?");
                return;
            }

            MechComponentDataObject newComponent = new MechComponentDataObject(mechComponent);
            AddItemToInventory(newComponent);
        }

        public void AddItemToInventory(MechComponentDataObject mechComponent)
        {
            instance.playerData.PlayerInventory.Add(mechComponent);
        }

        public void RemoveItemFromInventory(MechComponentDataObject mechComponent)
        {
            if (!instance.playerData.PlayerInventory.Contains(mechComponent))
            {
                Debug.Log("Attempted to remove " + mechComponent.ComponentName + " from the inventory, but it was not found there.");
                return;
            }

            instance.playerData.PlayerInventory.Remove(mechComponent);
        }
    }

    public class DowntimeBankController
    {
        public int GetPlayerCurrency()
        {
            return instance.playerData.CurrencyToSpend;
        }

        public float GetPlayerTime()
        {
            return instance.playerData.TimeLeftToSpend;
        }

        public void SpendPlayerCurrency(int currencyToSpend)
        {
            if (currencyToSpend > instance.playerData.CurrencyToSpend)
            {
                Debug.Log("Tried to spend more money than the player had! Please check the player currency with GameManager.instance.PlayerBank.GetPlayerCurrency() first!");
                return;
            }

            instance.playerData.CurrencyToSpend -= currencyToSpend;
        }

        public void SpendPlayerTime(float timeToSpend)
        {
            if (timeToSpend > instance.playerData.TimeLeftToSpend)
            {
                Debug.Log("Tried to spend more time than the player had! Please check the player time with GameManager.instance.PlayerBank.GetPlayerTime() first!");
                return;
            }

            instance.playerData.TimeLeftToSpend -= timeToSpend;
        }
    }
}




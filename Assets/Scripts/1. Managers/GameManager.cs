using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private DowntimeMechBuilderController playerMechController;
    private DowntimeInventoryController playerInventoryController;
    private DowntimeDeckController playerDeckController;
    private DowntimeBankController playerBankController;
    private PlayerDataObject playerData;
    private SceneController sceneController;

    public static GameManager instance;

    public DowntimeInventoryController PlayerInventoryController { get => playerInventoryController; }
    public DowntimeMechBuilderController PlayerMechController { get => playerMechController; }
    public DowntimeDeckController PlayerDeckController { get => playerDeckController; }
    public DowntimeBankController PlayerBankController { get => playerBankController; }
    public SceneController SceneController { get => sceneController; }

    public delegate void onUpdatePlayerCurrencies();
    public static event onUpdatePlayerCurrencies OnUpdatePlayerCurrencies;

    #region Playtesting
    [SerializeField] private SOItemDataObject starterMechHead;
    [SerializeField] private SOItemDataObject starterMechTorso;
    [SerializeField] private SOItemDataObject starterMechArms;
    [SerializeField] private SOItemDataObject starterMechLegs;
    [SerializeField] private List<SOItemDataObject> starterDeck;
    [SerializeField] protected int starterPlayerCurrency;
    [SerializeField] protected float timeBetweenFights;
    [SerializeField] protected int playerCurrencyGainOnWin;

    public int PlayerCurrencyGainOnWin { get => playerCurrencyGainOnWin; }


    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        LoadPlayer();
        
        MechObject opponentMech = instance.PlayerMechController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);
        PilotDataObject opponentPilot = new PilotDataObject();
        FighterDataObject opponentFighter = new FighterDataObject(opponentMech, opponentPilot, starterDeck);

        CombatManager.instance.PlayerFighter = new FighterDataObject(playerData.PlayerMech, new PilotDataObject(), playerData.PlayerDeck);
        CombatManager.instance.OpponentFighter = opponentFighter;

        CombatManager.instance.StartGame();
    }
    #endregion

    public void ReloadGame()
    {
        sceneController.LoadTitleScene();
        Destroy(this);
    }

    public void LoadShoppingScene()
    {
        sceneController.LoadDowntimeScene();
        playerBankController.AddPlayerCurrency(playerCurrencyGainOnWin);
        playerBankController.ResetPlayerTime();
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

        playerInventoryController = new DowntimeInventoryController();
        playerMechController = new DowntimeMechBuilderController();
        playerDeckController = new DowntimeDeckController();
        playerBankController = new DowntimeBankController();
    }

    private void Start()
    {
        sceneController = GetComponent<SceneController>();

        instance.LoadPlayer();
    }

    public void LoadPlayer(PlayerDataObject playerDataObject = null)
    {
        if (playerData != null)
            return;

        if (playerDataObject != null)
        {
            playerData = playerDataObject;
            return;
        }

        Debug.Log("Creating a new pilot.");

        playerData = new PlayerDataObject(starterPlayerCurrency, timeBetweenFights);
        MechObject newMech = instance.PlayerMechController.BuildNewMech(starterMechHead, starterMechTorso, starterMechArms, starterMechLegs);

        playerData.PlayerDeck = starterDeck;
        playerData.PlayerMech = newMech;

        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerData.PlayerMech, playerPilot, playerData.PlayerDeck);

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
        public MechObject PlayerMech { get => instance.playerData.PlayerMech; }

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

        public void SetNewPlayerMech(MechObject newMech)
        {
            MechObject newPlayerMech = new MechObject(newMech.MechHead, newMech.MechTorso, newMech.MechArms, newMech.MechLegs);
            instance.playerData.PlayerMech = newPlayerMech;
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
        public List<MechComponentDataObject> PlayerInventory { get => instance.playerData.PlayerInventory; }
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

            OnUpdatePlayerCurrencies?.Invoke();
        }

        public void SpendPlayerTime(float timeToSpend)
        {
            if (timeToSpend > instance.playerData.TimeLeftToSpend)
            {
                Debug.Log("Tried to spend more time than the player had! Please check the player time with GameManager.instance.PlayerBank.GetPlayerTime() first!");
                return;
            }

            instance.playerData.TimeLeftToSpend -= timeToSpend;

            OnUpdatePlayerCurrencies?.Invoke();
        }

        public void AddPlayerCurrency(int currencyToGain)
        {
            instance.playerData.CurrencyToSpend += currencyToGain;
            OnUpdatePlayerCurrencies?.Invoke();
        }

        public void ResetPlayerTime()
        {
            instance.playerData.TimeLeftToSpend = instance.timeBetweenFights;
            OnUpdatePlayerCurrencies?.Invoke();
        }
    }
}




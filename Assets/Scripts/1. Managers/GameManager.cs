using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] protected float timeBetweenFights;
    [SerializeField] protected int playerCurrencyGainOnWin;
    [Space]
    [Header("Character Settings")]
    [SerializeField] private SOCompleteCharacter starterPilot;
    [SerializeField] private List<SOCompleteCharacter> potentialAIBuilds;

    private int currencyGainModifier = 0;
    private int enemyHealthModifier = 0;
    private List<SOEventObject> activeEvents;

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

    public int EnemyHealthModifier { get => enemyHealthModifier; set => enemyHealthModifier = value; }
    public int PlayerWins { get => playerData.CurrentWinCount; }
    public int PlayerCurrencyGainOnWin { get => playerCurrencyGainOnWin + currencyGainModifier; set => currencyGainModifier = value; }
    public List<SOEventObject> ActiveEvents { get => activeEvents; }
    public SOCompleteCharacter StarterPilot { get => starterPilot; }

    public delegate void onUpdatePlayerCurrencies();
    public static event onUpdatePlayerCurrencies OnUpdatePlayerCurrencies;

    public void StashCurrentEvents(List<SOEventObject> newEvents)
    {
        activeEvents = new List<SOEventObject>();

        foreach (SOEventObject newEvent in newEvents)
            activeEvents.Add(newEvent);
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        LoadPlayer();
        FighterDataObject opponentFighter = new FighterDataObject(potentialAIBuilds[0]);

        CombatManager.instance.PlayerFighter = new FighterDataObject(playerData);
        CombatManager.instance.OpponentFighter = opponentFighter;

        CombatManager.instance.StartGame();
    }

    public void ReloadGame()
    {
        sceneController.LoadTitleScene();
        Destroy(this);
    }

    public void LoadShoppingScene()
    {
        sceneController.LoadDowntimeScene();
        playerBankController.AddPlayerCurrency(PlayerCurrencyGainOnWin);
        playerBankController.ResetPlayerTime();
    }

    public void UpdatePlayerAfterFight(MechObject newMech)
    {
        PlayerMechController.SetNewPlayerMech(newMech);
        playerData.CurrentWinCount += 1;
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

        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        playerInventoryController = new DowntimeInventoryController();
        playerMechController = new DowntimeMechBuilderController();
        playerDeckController = new DowntimeDeckController();
        playerBankController = new DowntimeBankController();
        activeEvents = new List<SOEventObject>();
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
        else
        {
            if (starterPilot != null)
            {
                Debug.Log("Creating new pilot.");
                instance.playerData = new PlayerDataObject(starterPilot);
                return;
            }

            Debug.Log("No starter pilot was found.");
        }
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
            MechComponentDataObject arms = new MechComponentDataObject(mechArms);
            MechComponentDataObject legs = new MechComponentDataObject(mechLegs);

            MechObject newMech = new MechObject(head, torso, arms, legs);

            return newMech;
        }

        public MechObject BuildNewMech(SOMechObject newMech)
        {
            MechComponentDataObject head = new MechComponentDataObject(newMech.MechHead);
            MechComponentDataObject torso = new MechComponentDataObject(newMech.MechTorso);
            MechComponentDataObject arms = new MechComponentDataObject(newMech.MechArms);
            MechComponentDataObject legs = new MechComponentDataObject(newMech.MechLegs);

            MechObject newMechDataObject = new MechObject(head, torso, arms, legs);

            return newMechDataObject;
        }

        public void SetNewPlayerMech(MechObject newMech)
        {
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
            MechComponentDataObject componentToRemove = null;

            foreach (MechComponentDataObject component in instance.playerData.PlayerInventory)
                if (component == mechComponent)
                    componentToRemove = component;

            if (componentToRemove == null)
                return;

            instance.playerData.PlayerInventory.Remove(componentToRemove);
        }

        public void RemoveItemFromInventory(SOItemDataObject mechComponent)
        {
            MechComponentDataObject componentToRemove = null;

            foreach (MechComponentDataObject component in instance.playerData.PlayerInventory)
                if (component.SOItemDataObject == mechComponent)
                    componentToRemove = component;

            if (componentToRemove == null)
                return;

            instance.playerData.PlayerInventory.Remove(componentToRemove);
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

        public void AddPlayerTime(int timeToGain)
        {
            instance.playerData.CurrencyToSpend += timeToGain;
            OnUpdatePlayerCurrencies?.Invoke();
        }

        public void ResetPlayerTime()
        {
            instance.playerData.TimeLeftToSpend = instance.timeBetweenFights;
            OnUpdatePlayerCurrencies?.Invoke();
        }
    }
}




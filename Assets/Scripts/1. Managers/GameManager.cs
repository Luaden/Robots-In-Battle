using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public bool isTrailerMaking;
    [SerializeField] protected float timeBetweenFights;
    [SerializeField] protected int playerCurrencyGainOnWin;
    [Space]
    [Header("Character Settings")]
    [SerializeField] private SOCompleteCharacter starterPilot;
    [Header("Bosses")]
    [SerializeField] private List<SOCompleteCharacter> bossCharacters;


    private int currencyGainModifier = 0;
    private int enemyHealthModifier = 0;
    private List<SOEventObject> activeEvents;
    private FighterDataObject nextFighter;
    private Canvas currentMainCanvas;

    private FighterBuildController fighterBuildController;
    private DowntimeMechBuilderController playerMechController;
    private DowntimeInventoryController playerInventoryController;
    private DowntimeDeckController playerDeckController;
    private DowntimeBankController playerBankController;
    private PlayerDataObject playerData;

    public static GameManager instance;

    public FighterBuildController FighterBuildController { get => fighterBuildController; }
    public DowntimeInventoryController PlayerInventoryController { get => playerInventoryController; }
    public DowntimeMechBuilderController PlayerMechController { get => playerMechController; }
    public DowntimeDeckController PlayerDeckController { get => playerDeckController; }
    public DowntimeBankController PlayerBankController { get => playerBankController; }
    public Canvas CurrentMainCanvas { get => currentMainCanvas; set => AssignCurrentMainCanvas(value); }

    public int EnemyHealthModifier { get => enemyHealthModifier; set => enemyHealthModifier = value; }
    public int PlayerWins { get => playerData.CurrentWinCount; }
    public int PlayerCurrencyGainOnWin { get => playerCurrencyGainOnWin + currencyGainModifier; set => currencyGainModifier = value; }
    public List<SOEventObject> ActiveEvents { get => activeEvents; }
    public PlayerDataObject Player { get => playerData; }
    public FighterDataObject NextFighter { get => nextFighter; set => nextFighter = value; }

    public delegate void onUpdatePlayerCurrencies();
    public static event onUpdatePlayerCurrencies OnUpdatePlayerCurrencies;

    public delegate void onUpdatedMainCanvas();
    public static event onUpdatedMainCanvas OnUpdatedMainCanvas;

    public void StashCurrentEvents(List<SOEventObject> newEvents)
    {
        activeEvents = new List<SOEventObject>();

        foreach (SOEventObject newEvent in newEvents)
            activeEvents.Add(newEvent);
    }

    public void CreatePlayerAndFighters()
    {
        LoadPlayer();

        List<FighterDataObject> bossFighters = new List<FighterDataObject>();
        List<FighterDataObject> newFighters = new List<FighterDataObject>();

        foreach(SOCompleteCharacter boss in bossCharacters)
        {
            FighterDataObject newBoss = new FighterDataObject(boss);
            newBoss.FighterUIObject = new FighterPilotUIObject(boss.FighterPilotUIObject.FighterHair,
                                                               boss.FighterPilotUIObject.FighterEyes,
                                                               boss.FighterPilotUIObject.FighterNose,
                                                               boss.FighterPilotUIObject.FighterMouth,
                                                               boss.FighterPilotUIObject.FighterClothes,
                                                               boss.FighterPilotUIObject.FighterBody);
            newBoss.FighterDeck = PlayerDeckController.BuildFighterDeck(boss.DeckList);
            bossFighters.Add(newBoss);
            Debug.Log("Creating new boss: " + boss.PilotName);
        }
        
        for(int i = 0; i < 7; i++)
        {
            FighterDataObject opponentFighter = fighterBuildController.GetRandomFighter();
            opponentFighter.FighterDeck = PlayerDeckController.BuildFighterDeck(opponentFighter.FighterCompleteCharacter.DeckList);
            newFighters.Add(opponentFighter);
        }

        playerData.BossFighters = bossFighters;
        playerData.OtherFighters = newFighters;
    }


    public void LoadTitleScene()
    {
        SceneManager.instance.LoadTitleScene();
        instance = null;
        Destroy(gameObject);
    }

    public void LoadCombatScene()
    {
        SceneManager.instance.LoadCombatScene();
    }

    public void LoadWorkshopScene()
    {
        SceneManager.instance.LoadWorkshopScene();
        playerBankController.AddPlayerCurrency(PlayerCurrencyGainOnWin);
        playerBankController.ResetPlayerTime();
    }

    public void UpdatePlayerAfterFight(MechObject newMech)
    {
        PlayerMechController.SetNewPlayerMech(newMech);
        playerData.CurrentWinCount += 1;
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
                playerData = new PlayerDataObject(starterPilot);
                playerData.PlayerFighterData.FighterDeck = PlayerDeckController.BuildFighterDeck(starterPilot.DeckList);
                return;
            }

            Debug.Log("No starter pilot was found.");
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        fighterBuildController = GetComponent<FighterBuildController>();
        playerInventoryController = new DowntimeInventoryController();
        playerMechController = new DowntimeMechBuilderController();
        playerDeckController = new DowntimeDeckController();
        playerBankController = new DowntimeBankController();
        activeEvents = new List<SOEventObject>();
        CreatePlayerAndFighters();
    }

    private void AssignCurrentMainCanvas(Canvas canvas)
    {
        currentMainCanvas = canvas;
        OnUpdatedMainCanvas?.Invoke();
    }

    public class DowntimeDeckController
    {
        public List<CardDataObject> PlayerDeck { get => instance.playerData.PlayerFighterData.FighterDeck; }

        public void AddCardToPlayerDeck(SOItemDataObject newCard)
        {
            List<CardDataObject> currentDeck = new List<CardDataObject>(PlayerDeck);

            if (newCard.ItemType != ItemType.Card)
            {
                Debug.Log("A component was attempted to be added to the player deck, but this was an incorrect location for it.");
                Debug.Log("Is " + newCard.ItemType + " the correct ItemType for " + newCard.ItemName + "?");
                return;
            }

            CardDataObject newCardDataObject = new CardDataObject(newCard);
            currentDeck.Add(newCardDataObject);

            instance.playerData.PlayerFighterData.FighterDeck = currentDeck;
        }

        public bool CheckPlayerHasCard(SOItemDataObject cardToCheck)
        {
            if (!PlayerDeck.Select(x => x.SOItemDataObject).Contains(cardToCheck))
            {
                return false;
            }

            return true;
        }

        public void RemoveCardFromPlayerDeck(SOItemDataObject newCard)
        {
            CardDataObject cardToRemove = null;

            foreach(CardDataObject cardDataObject in PlayerDeck)
            {
                if (cardDataObject.CardName == newCard.CardName)
                {
                    cardToRemove = cardDataObject;
                    Debug.Log(cardToRemove.CardName);
                }
            }

            if (cardToRemove != null)
                PlayerDeck.Remove(cardToRemove);
            else
                Debug.Log("Could not find " + newCard.CardName + " in the Player's deck.");
        }

        public List<CardDataObject> BuildFighterDeck(List<SOItemDataObject> cardDeck)
        {
            List<CardDataObject> fighterDeck = new List<CardDataObject>();

            CardDataObject newCard;

            foreach (SOItemDataObject newCardSO in cardDeck)
            {
                if (newCardSO.ItemType != ItemType.Card)
                {
                    Debug.Log(newCardSO.ItemName + " was found in the deck, but it is not a Card ItemType. Was this a mistake or is this item classified incorrectly?");
                    continue;
                }

                newCard = new CardDataObject(newCardSO);
                fighterDeck.Add(newCard);
            }

            return fighterDeck;
        }
    }

    public class DowntimeMechBuilderController
    {
        public MechObject PlayerMech { get => instance.playerData.PlayerFighterData.FighterMech; }

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
            instance.playerData.PlayerFighterData.FighterMech = newMech;
        }

        public void SwapPlayerMechPart(SOItemDataObject SOMechComponentDataObject)
        {
            MechComponentDataObject newComponent = new MechComponentDataObject(SOMechComponentDataObject);

            SwapPlayerMechPart(newComponent);
        }

        public void SwapPlayerMechPart(MechComponentDataObject newComponent)
        {
            MechComponentDataObject oldComponent;

            oldComponent = instance.playerData.PlayerFighterData.FighterMech.ReplaceComponent(newComponent);
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
            AudioController.instance.PlaySound(SoundType.CashRegister);
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




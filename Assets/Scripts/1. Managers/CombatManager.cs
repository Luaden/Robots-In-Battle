using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Combat Modifiers")]
    [SerializeField] private int mechEnergyGain;
    [SerializeField] private float counterDamageMultiplier;
    [SerializeField] private float guardDamageMultiplier;
    [SerializeField] private float acidComponentDamageMultiplier;
    [SerializeField] private int iceChannelEnergyReductionModifier;

    #region Debug
    [Header("Debug / Testing")]
    [SerializeField] private GameObject winLossPanel;
    [SerializeField] private GameObject reloadGameButton;
    [SerializeField] private GameObject loadShoppingButton;
    [SerializeField] private bool narrateCardSelection;
    [SerializeField] private bool narrateCombatChoices;
    [SerializeField] private bool narrateAIDecisionMaking;

    public bool NarrateCardSelection { get => narrateCardSelection; }
    public bool NarrateCombat { get => narrateCombatChoices; }
    public bool NarrateAIDecisionMaking { get => narrateAIDecisionMaking; }
    #endregion

    public static CombatManager instance;

    private CombatDeckManager deckManager;
    private HandManager handManager;
    private PlayerHandUISlotManager playerHandSlotManager;
    private OpponentHandUISlotManager opponentHandSlotManager;
    private ChannelsUISlotManager channelsUISlotManager;
    private CardPlayManager cardPlayManager;
    private CardUIManager cardUIManager;
    private MechHUDManager mechHUDManager;
    private PopupUIManager popupUIManager;
    private BuffUIManager buffUIManager;
    private MechAnimationManager mechAnimationManager;
    private BurnPileController burnPileController;

    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;

    public FighterDataObject PlayerFighter { get => playerFighter; set => InitPlayerFighter(value); }
    public FighterDataObject OpponentFighter { get => opponentFighter; set => InitOpponentFighter(value); }
    public CombatDeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandUISlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandUISlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public ChannelsUISlotManager ChannelsUISlotManager { get => channelsUISlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }
    public BuffUIManager BuffUIManager { get => buffUIManager; }
    public MechAnimationManager MechAnimationManager { get => mechAnimationManager; }
    public BurnPileController BurnPileController { get => burnPileController; }

    public int MechEnergyGain { get => mechEnergyGain; }
    public float CounterDamageMultiplier { get => counterDamageMultiplier; }
    public float GuardDamageMultiplier { get => guardDamageMultiplier; }
    public float AcidComponentDamageMultiplier { get => acidComponentDamageMultiplier; }
    public int IceChannelEnergyReductionModifier { get => iceChannelEnergyReductionModifier; }

    public delegate void onDestroyScene();
    public static event onDestroyScene OnDestroyScene;

    public delegate void onStartNewTurn();
    public static event onStartNewTurn OnStartNewTurn;

    #region Debug
    public void StartGame()
    {

        StartNewTurn();
    }
    #endregion

    public void RemoveHealthFromMech(CharacterSelect character, int damage)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentHP = Mathf.Clamp(playerFighter.FighterMech.MechCurrentHP - damage, 0, int.MaxValue);
            mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }

        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentHP = Mathf.Clamp(opponentFighter.FighterMech.MechCurrentHP - damage, 0, int.MaxValue);
            mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }
    }

    public void AddHealthToMech(CharacterSelect character, int health)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentHP += health;
            mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }

        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentHP += health;
            mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }
    }

    public void RemoveEnergyFromMech(CharacterSelect character, int energyToRemove, bool queueEnergyRemoval = false)
    {
        if (character == CharacterSelect.Player)
        {
            if(queueEnergyRemoval)
            {
                mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, 
                    playerFighter.FighterMech.MechCurrentEnergy - energyToRemove, 
                    queueEnergyRemoval);
                return;
            }

            playerFighter.FighterMech.MechCurrentEnergy -= energyToRemove;
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
        }
        if (character == CharacterSelect.Opponent)
        {
            if (queueEnergyRemoval)
            {
                mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, 
                    opponentFighter.FighterMech.MechCurrentEnergy - energyToRemove, 
                    queueEnergyRemoval);
                return;
            }

            opponentFighter.FighterMech.MechCurrentEnergy -= energyToRemove;
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
        }
    }

    public void AddEnergyToMech(CharacterSelect character, int energyToAdd)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentEnergy = Mathf.Clamp(playerFighter.FighterMech.MechCurrentEnergy + energyToAdd, 0, playerFighter.FighterMech.MechMaxEnergy);
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
        }
        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentEnergy = Mathf.Clamp(opponentFighter.FighterMech.MechCurrentEnergy + energyToAdd, 0, opponentFighter.FighterMech.MechMaxEnergy);
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, opponentFighter.FighterMech.MechCurrentEnergy, false);
        }
    }

    private void Awake()
    {
        instance = this;

        deckManager = FindObjectOfType<CombatDeckManager>(true);
        handManager = FindObjectOfType<HandManager>(true);
        playerHandSlotManager = FindObjectOfType<PlayerHandUISlotManager>(true);
        opponentHandSlotManager = FindObjectOfType<OpponentHandUISlotManager>(true);
        channelsUISlotManager = FindObjectOfType<ChannelsUISlotManager>(true);
        cardPlayManager = FindObjectOfType<CardPlayManager>(true);
        cardUIManager = FindObjectOfType<CardUIManager>(true);
        mechHUDManager = FindObjectOfType<MechHUDManager>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
        buffUIManager = FindObjectOfType<BuffUIManager>(true);
        mechAnimationManager = FindObjectOfType<MechAnimationManager>(true);
        burnPileController = FindObjectOfType<BurnPileController>(true);
    }

    private void Start()
    {
        CardPlayManager.OnCombatComplete += StartNewTurn;
    }

    private void OnDestroy()
    {
        OnDestroyScene?.Invoke();
        instance = null;
        CardPlayManager.OnCombatComplete -= StartNewTurn;
    }

    private void InitPlayerFighter(FighterDataObject newPlayerFighter)
    {
        playerFighter = newPlayerFighter;
        deckManager.SetPlayerDeck(newPlayerFighter.FighterDeck);
        mechHUDManager.SetPlayerMaxStats(playerFighter.FighterMech.MechMaxHP, playerFighter.FighterMech.MechMaxHP);
        mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
    }

    private void InitOpponentFighter(FighterDataObject newOpponentFighter)
    {
        opponentFighter = newOpponentFighter;
        deckManager.SetOpponentDeck(newOpponentFighter.FighterDeck);
        mechHUDManager.SetOpponentMaxStats(opponentFighter.FighterMech.MechMaxHP, opponentFighter.FighterMech.MechMaxEnergy);
    }

    private void StartNewTurn()
    {
        deckManager.DrawPlayerCard(5 - HandManager.PlayerHand.CharacterHand.Count);
        deckManager.DrawOpponentCard(5 - HandManager.OpponentHand.CharacterHand.Count);

        AddEnergyToMech(CharacterSelect.Opponent, mechEnergyGain + OpponentFighter.FighterMech.MechEnergyGain);
        AddEnergyToMech(CharacterSelect.Player, mechEnergyGain + PlayerFighter.FighterMech.MechEnergyGain);

        OnStartNewTurn?.Invoke();
    }

    private void CheckForWinLoss()
    {
        if (playerFighter.FighterMech.MechCurrentHP <= 0)
        {
            winLossPanel.SetActive(true);
            MechAnimationManager.SetMechAnimation(CharacterSelect.Player, AnimationType.Lose, CharacterSelect.Opponent, AnimationType.Win);
            reloadGameButton.SetActive(true);

            GameManager.instance.PlayerMechController.SetNewPlayerMech(playerFighter.FighterMech);
            GameManager.instance.PlayerBankController.AddPlayerCurrency(GameManager.instance.PlayerCurrencyGainOnWin);
            return;
        }
        if (opponentFighter.FighterMech.MechCurrentHP <= 0)
        {
            winLossPanel.SetActive(true);
            loadShoppingButton.SetActive(true);
            MechAnimationManager.SetMechAnimation(CharacterSelect.Player, AnimationType.Win, CharacterSelect.Opponent, AnimationType.Lose);
            return;
        }

    }
}

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
    public MechHUDManager MechHUDManager { get => mechHUDManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }
    public BuffUIManager BuffUIManager { get => buffUIManager; }
    public MechAnimationManager MechAnimationManager { get => mechAnimationManager; }

    public int MechEnergyGain { get => mechEnergyGain; }
    public float CounterDamageMultiplier { get => counterDamageMultiplier; }
    public float GuardDamageMultiplier { get => guardDamageMultiplier; }
    public float AcidComponentDamageMultiplier { get => acidComponentDamageMultiplier; }
    public int IceChannelEnergyReductionModifier { get => iceChannelEnergyReductionModifier; }

    public delegate void onDestroyScene();
    public static event onDestroyScene OnDestroyScene;

    public void DealDamageToMech(CharacterSelect character, int damage)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentHP -= damage;
            mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }

        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentHP -= damage;
            mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }

    }

    public void RemoveEnergyFromMech(CharacterSelect character, int energyToRemove)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentEnergy -= energyToRemove;
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy);
        }
        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentEnergy -= energyToRemove;
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy);
        }
    }

    public void AddEnergyToMech(CharacterSelect character, int energyToAdd)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentEnergy += energyToAdd;
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy);
        }
        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentEnergy += energyToAdd;
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy);
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

        mechHUDManager.SetPlayerMaxStats(playerFighter.FighterMech.MechMaxHP, playerFighter.FighterMech.MechMaxHP);
    }

    private void InitOpponentFighter(FighterDataObject newOpponentFighter)
    {
        opponentFighter = newOpponentFighter;

        mechHUDManager.SetOpponentMaxStats(opponentFighter.FighterMech.MechMaxHP, opponentFighter.FighterMech.MechMaxEnergy);
    }

    private void StartNewTurn()
    {
        deckManager.DrawPlayerCard(5 - HandManager.PlayerHand.CharacterHand.Count);
        deckManager.DrawOpponentCard(5 - HandManager.OpponentHand.CharacterHand.Count);

        AddEnergyToMech(CharacterSelect.Opponent, OpponentFighter.FighterMech.MechEnergyGain);
        AddEnergyToMech(CharacterSelect.Player, PlayerFighter.FighterMech.MechEnergyGain);
    }

    private void CheckForWinLoss()
    {
        if (playerFighter.FighterMech.MechCurrentHP <= 0)
        {
            winLossPanel.SetActive(true);
            MechAnimationManager.SetMechAnimation(CharacterSelect.Player, AnimationType.Lose, CharacterSelect.Opponent, AnimationType.Win);
        }
        if (opponentFighter.FighterMech.MechCurrentHP <= 0)
        {
            winLossPanel.SetActive(true);
            MechAnimationManager.SetMechAnimation(CharacterSelect.Player, AnimationType.Win, CharacterSelect.Opponent, AnimationType.Lose);
        }

    }
}

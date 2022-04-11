using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private int mechEnergyGain;
    [SerializeField] private int cardDrawOnTurn;

    public static CombatManager instance;

    private DeckManager deckManager;
    private HandManager handManager;
    private PlayerHandUISlotManager playerHandSlotManager;
    private OpponentHandUISlotManager opponentHandSlotManager;
    private ChannelsUISlotManager channelsUISlotManager;
    private CardPlayManager cardPlayManager;
    private CardUIManager cardUIManager;
    private MechHUDManager mechHUDManager;
    private PopupUIManager popupUIManager;

    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;

    public FighterDataObject PlayerFighter { get => playerFighter; set => InitPlayerFighter(value); }
    public FighterDataObject OpponentFighter { get => opponentFighter; set => InitOpponentFighter(value); }
    public DeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandUISlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandUISlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public ChannelsUISlotManager ChannelsUISlotManager { get => channelsUISlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }
    public MechHUDManager MechHUDManager { get => mechHUDManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

    public int MechEnergyGain { get => mechEnergyGain; }

    public void DealDamageToMech(CharacterSelect character, int damage)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentHP -= damage;
            mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
        }
        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentHP -= damage;
            mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
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

        deckManager = FindObjectOfType<DeckManager>(true);
        handManager = FindObjectOfType<HandManager>(true);
        playerHandSlotManager = FindObjectOfType<PlayerHandUISlotManager>(true);
        opponentHandSlotManager = FindObjectOfType<OpponentHandUISlotManager>(true);
        channelsUISlotManager = FindObjectOfType<ChannelsUISlotManager>(true);
        cardPlayManager = FindObjectOfType<CardPlayManager>(true);
        cardUIManager = FindObjectOfType<CardUIManager>(true);
        mechHUDManager = FindObjectOfType<MechHUDManager>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
    }

    private void Start()
    {
        CardPlayManager.OnCombatComplete += StartNewTurn;
    }

    private void OnDestroy()
    {
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
        deckManager.DrawPlayerCard(cardDrawOnTurn);
        deckManager.DrawOpponentCard(cardDrawOnTurn);

        AddEnergyToMech(CharacterSelect.Opponent, OpponentFighter.FighterMech.MechEnergyGain);
        AddEnergyToMech(CharacterSelect.Player, PlayerFighter.FighterMech.MechEnergyGain);
    }
}

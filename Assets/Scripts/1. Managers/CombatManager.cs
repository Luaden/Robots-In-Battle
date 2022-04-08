using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<SOCardDataObject> testDeck;

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

    public DeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandUISlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandUISlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public ChannelsUISlotManager ChannelsUISlotManager { get => channelsUISlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }
    public MechHUDManager MechHUDManager { get => mechHUDManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

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
       
    }

    private void OnDisable()
    {
        instance = null;
    }

    [ContextMenu("Draw Card Prefab")]
    public void DrawCardPrefab()
    {
        deckManager.SetPlayerDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            deckManager.DrawPlayerCard();

        deckManager.SetOpponentDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            deckManager.DrawOpponentCard();
    }

    public FighterDataObject GetFighterData(CharacterSelect mechToGet)
    {
        if (mechToGet == CharacterSelect.Player)
            return playerFighter;

        return opponentFighter;
    }

    public void DealDamageToMech(CharacterSelect character, int damage)
    {
        if (character == CharacterSelect.Player)
            mechHUDManager.PlayerHUDBarController.UpdateHealthBar(damage);
        if (character == CharacterSelect.Opponent)
            mechHUDManager.OpponentHUDBarController.UpdateHealthBar(damage);
    }
}

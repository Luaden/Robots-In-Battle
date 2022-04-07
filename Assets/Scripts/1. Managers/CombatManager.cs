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

    public DeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandUISlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandUISlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public ChannelsUISlotManager ChannelsUISlotManager { get => channelsUISlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }

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
}

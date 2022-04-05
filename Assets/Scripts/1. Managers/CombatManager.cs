using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<SOCardDataObject> testDeck;

    public static CombatManager instance;

    private DeckManager deckManager;
    private HandManager handManager;
    private PlayerHandSlotManager playerHandSlotManager;
    private OpponentHandSlotManager opponentHandSlotManager;
    private CardPlayManager cardPlayManager;
    private CardUIManager cardUIManager;

    public DeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandSlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandSlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }

    private void Awake()
    {
        instance = this;

        deckManager = FindObjectOfType<DeckManager>(true);
        handManager = FindObjectOfType<HandManager>(true);
        playerHandSlotManager = FindObjectOfType<PlayerHandSlotManager>(true);
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
    }
}

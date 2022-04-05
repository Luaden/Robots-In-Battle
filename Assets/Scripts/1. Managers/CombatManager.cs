using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    private DeckManager deckManager;
    private HandManager handManager;
    private CardPlayManager cardPlayManager;
    private CardUIManager cardUIManager;

    public DeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => HandManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }

    private void Awake()
    {
        instance = this;

        deckManager = FindObjectOfType<DeckManager>(true);
        handManager = FindObjectOfType<HandManager>(true);
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
}

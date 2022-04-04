using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    private DeckManager deckManager;
    private CardPlayManager cardPlayManager;

    public DeckManager DeckManager { get => deckManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }

    private void Awake()
    {
        instance = this;

        deckManager = FindObjectOfType<DeckManager>(true);
        cardPlayManager = FindObjectOfType<CardPlayManager>(true);
    }

    private void Start()
    {
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    private CardShopManager cardShopManager;
    private ComponentShopManager componentShopManager;
    private ShopCollectionRandomizeManager shopCollectionRandomizeManager;
    private MechBuilderController mechBuilderController;
    private DeckBuilderController deckBuilderController;

    public static DowntimeManager instance;
    public CardShopManager CardShopManager { get => cardShopManager; }
    public ComponentShopManager ComponentShopManager { get => componentShopManager; }
    public ShopCollectionRandomizeManager ShopCollectionRandomizeManager { get => shopCollectionRandomizeManager; }
    public MechBuilderController MechBuilderController { get => mechBuilderController; }
    public DeckBuilderController DeckBuilderController { get => deckBuilderController; }


    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        cardShopManager = GetComponentInChildren<CardShopManager>(true);
        componentShopManager = GetComponentInChildren<ComponentShopManager>(true);
        shopCollectionRandomizeManager = GetComponentInChildren<ShopCollectionRandomizeManager>(true);
        mechBuilderController = GetComponent<MechBuilderController>();
        deckBuilderController = GetComponent<DeckBuilderController>();

        instance.CardShopManager.InitializeShop();
        instance.ComponentShopManager.InitializeShop();

    }
}

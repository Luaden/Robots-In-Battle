using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    [SerializeField] private int repairCost;
    private CardShopManager cardShopManager;
    private ComponentShopManager componentShopManager;
    private ShopCollectionRandomizeManager shopCollectionRandomizeManager;
    private InventoryManager inventoryManager;

    public static DowntimeManager instance;

    public int RepairCost { get => repairCost; }

    public CardShopManager CardShopManager { get => cardShopManager; }
    public ComponentShopManager ComponentShopManager { get => componentShopManager; }
    public ShopCollectionRandomizeManager ShopCollectionRandomizeManager { get => shopCollectionRandomizeManager; }

    public InventoryManager InventoryManager { get => inventoryManager; }

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
        inventoryManager = GetComponentInChildren<InventoryManager>(true);


        instance.CardShopManager.InitializeShop();
        instance.ComponentShopManager.InitializeShop();

    }

    public void LoadCombatScene()
    {
        GameManager.instance.SceneController.LoadCombatScene();
    }

    public void RepairEquippedItems()
    {
        if (GameManager.instance.PlayerBankController.GetPlayerCurrency() >= repairCost)
        {
            GameManager.instance.PlayerMechController.PlayerMech.ResetHealth();
            GameManager.instance.PlayerBankController.SpendPlayerCurrency(repairCost);
        }
    }
}

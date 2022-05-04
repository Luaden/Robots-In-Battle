using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject shopCartWindow;

    [SerializeField] private int repairCost;
    [SerializeField] private int minimumShopItemCount;

    private ShopManager shopManager;
    private ShopUISlotManager shopUISlotManager;
    private ShopItemUIBuildController shopItemUIBuildController;
    private ShoppingCartManager shoppingCartManager;
    private ShoppingCartUISlotManager shoppingCartUISlotManager;
    private InventoryUISlotManager inventoryUISlotManager;
    private PopupUIManager popupUIManager;

    public static DowntimeManager instance;

    public int RepairCost { get => repairCost; }
    public int MinimumShopItemCount { get => minimumShopItemCount; }

    public ShopManager ShopManager { get => shopManager; }
    public ShopUISlotManager ShopUISlotManager { get => shopUISlotManager; }
    public ShopItemUIBuildController ShopItemUIBuildController { get => shopItemUIBuildController; }
    public ShoppingCartManager ShoppingCartManager { get => shoppingCartManager; }
    public ShoppingCartUISlotManager ShoppingCartUISlotManager { get => shoppingCartUISlotManager; }
    public InventoryUISlotManager InventoryUISlotManager { get => inventoryUISlotManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }

    public delegate void onLoadCombatScene();
    public static event onLoadCombatScene OnLoadCombatScene;

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        shopManager = FindObjectOfType<ShopManager>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
        shopUISlotManager = FindObjectOfType<ShopUISlotManager>();
        shoppingCartManager = FindObjectOfType<ShoppingCartManager>(true);
        shoppingCartUISlotManager = FindObjectOfType<ShoppingCartUISlotManager>(true);
        shopItemUIBuildController = FindObjectOfType<ShopItemUIBuildController>();
        inventoryUISlotManager = FindObjectOfType<InventoryUISlotManager>();

    }

    public void Start()
    {
        instance.ShopManager.InitializeShop();
    }

    public void LoadCombatScene()
    {
        OnLoadCombatScene?.Invoke();
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

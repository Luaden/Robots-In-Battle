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
    private InventoryUISlotManager inventoryUISlotManager;
    private PopupUIManager popupUIManager;
    private ShopCameraBoomMoveController cameraBoomMoveController;

    public static DowntimeManager instance;

    private bool shopInitialized = false;
    public int RepairCost { get => repairCost; }
    public int MinimumShopItemCount { get => minimumShopItemCount; }
    public bool ShopInitialized { get => shopInitialized; set => shopInitialized = value; }

    public ShopManager ShopManager { get => shopManager; }
    public ShopUISlotManager ShopUISlotManager { get => shopUISlotManager; }
    public ShopItemUIBuildController ShopItemUIBuildController { get => shopItemUIBuildController; }
    public InventoryUISlotManager InventoryUISlotManager { get => inventoryUISlotManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }
    public ShopCameraBoomMoveController CameraBoomMoveController { get => cameraBoomMoveController; }

    public delegate void onLoadCombatScene();
    public static event onLoadCombatScene OnLoadCombatScene;

    public delegate void onMoveToInventory();
    public static event onMoveToInventory OnMoveToInventory;

    public delegate void onMoveToShop();
    public static event onMoveToShop OnMoveToShop;

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
        shopUISlotManager = FindObjectOfType<ShopUISlotManager>(true);
        shopItemUIBuildController = FindObjectOfType<ShopItemUIBuildController>(true);
        inventoryUISlotManager = FindObjectOfType<InventoryUISlotManager>(true);
        cameraBoomMoveController = FindObjectOfType<ShopCameraBoomMoveController>(true);
    }

    public void InitializeShop()
    {
        instance.ShopManager.InitializeShop();
    }

    public void MoveToInventory()
    {
        OnMoveToInventory?.Invoke();
    }

    public void MoveToShop()
    {
        OnMoveToShop?.Invoke();
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

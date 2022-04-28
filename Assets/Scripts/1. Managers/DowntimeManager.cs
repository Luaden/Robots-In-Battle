using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DowntimeManager : MonoBehaviour
{
    private CardShopManager cardShopManager;
    private ComponentShopManager componentShopManager;
    private ShopCollectionRandomizeManager shopCollectionRandomizeManager;
    private InventoryManager inventoryManager;
    [SerializeField] protected int repairCost;

    public static DowntimeManager instance;
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
        Debug.Log("click");
        MechComponentDataObject[] mechDatas = new MechComponentDataObject[4] { 
            GameManager.instance.PlayerMechController.PlayerMech.MechHead,
            GameManager.instance.PlayerMechController.PlayerMech.MechTorso,
            GameManager.instance.PlayerMechController.PlayerMech.MechArms,
            GameManager.instance.PlayerMechController.PlayerMech.MechLegs
        };

        if (GameManager.instance.PlayerBankController.GetPlayerCurrency() >= repairCost)
        {
            GameManager.instance.PlayerMechController.BuildNewMech(mechDatas[0].SOItemDataObject, mechDatas[1].SOItemDataObject, mechDatas[2].SOItemDataObject, mechDatas[3].SOItemDataObject);
            GameManager.instance.PlayerBankController.SpendPlayerCurrency(repairCost);
            Debug.Log("repairing mech");
        }
    }
}
